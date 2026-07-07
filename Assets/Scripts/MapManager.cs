using Assets.Scripts.Model;
using Assets.Scripts.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Model.Types;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    DataProvider dataProvider;
    [SerializeField]
    GameObject physicalMapItemPrefab;
    [SerializeField]
    GameObject vistaPointPrefab;
    [SerializeField]
    GameObject triviaPointPrefab;
    [SerializeField]
    GameObject rotatingItemPrefab;
    [SerializeField]
    GameObject playerIconPrefab;

    Canvas canvas;
    GameObject physicalPlayerObject;
    Player player;
    GameManager gameManager;

    readonly string CanvasTag = "Canvas";
    readonly string PlayerTag = "Player";
    readonly string GameManagerTag = "GameManager";
    readonly string DataProviderTag = "DataProvider";
    readonly string RotatingItems = "RotatingItems";

    private void Start()
    {
        StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        while (dataProvider == null)
        {
            dataProvider = GameObject.FindGameObjectWithTag(DataProviderTag).GetComponent<DataProvider>();
            if (dataProvider == null)
            {
                Debug.Log("Failed to load dataProvider at MapManager' script.");
                yield return null;
            }
        }

        while (canvas == null)
        {
            var canvases = GameObject.FindGameObjectsWithTag(CanvasTag);
            canvas = canvases.Where(x => x.scene.name == "Map").FirstOrDefault().GetComponent<Canvas>();

            if (canvas == null)
            {
                Debug.Log("Failed to load Canvas at MapManager' script.");
                yield return null;
            }
        }

        while (player == null)
        {
            physicalPlayerObject = GameObject.FindGameObjectWithTag(PlayerTag);
            player = physicalPlayerObject.GetComponent<Player>();

            if (player == null)
            {
                Debug.Log("Failed to load Player at MapManager' script.");
                yield return null;
            }
        }

        while (!player.IsDataReady)
        {
            yield return null;
        }

        while (gameManager == null)
        {
            var gameManagerObject = GameObject.FindWithTag(GameManagerTag);
            gameManager = gameManagerObject.GetComponent<GameManager>();

            if (gameManager == null)
            {
                Debug.Log("Failed to load GameManager at MapManager' script.");
                yield return null;
            }
        }


        while (GameManager.Instance.getMainMapMap().centerLatLon == null)
        {
            Debug.Log("centerLatLon is not ready.");
            yield return null;
        }

        InitMap();
    }

    public async void InitMap()
    {
        var (vistas, trivias) = await dataProvider.GetCompletableItems();
        var rotatingItems = await dataProvider.GetRotatingItems();


        gameManager.SecretVistas = (vistas as HashSet<VistaPosition>)
            .Where(vista => vista.IsSecret)
            .Select(vista => vista.Id)
            .ToHashSet();

        InstantiatePlayerIcon();
        InstantiateCompletableItemPoints(vistas);
        InstantiateCompletableItemPoints(trivias);
    }


    public void LoadMap()
    {
        // TODO implement here
    }

    private void CheckCompletedAndUnlockedItems()
    {
        // TODO implement here
    }

    private void GetAvailableCompletableItems()
    {
        // TODO implement here
    }

    private void InstantiatePlayerIcon()
    {
        var playerIcon = Instantiate(playerIconPrefab);
        playerIcon.transform.SetParent(canvas.transform);

        var mapItemBehavior = playerIcon.GetComponent<MapItemBehavior>().physicMapItem = physicalPlayerObject;

        Debug.Log("PlayerIcon instantiated.");
    }

    private void InstantiateCompletableItemPoints(IEnumerable<IDataPackage> completableItems)
    {
        foreach (ICompletableItem completableItem in completableItems.Cast<ICompletableItem>())
        {
            GameObject completableItemPoint = null;
            bool IsCompleted = false;
            bool IsUnlocked = false;

            if (completableItem.Type == CompletableItemType.Vista)
            {
                completableItemPoint = Instantiate(vistaPointPrefab);
                completableItemPoint.name = "VistaPoint" + completableItem.Id;
                IsCompleted = player.playerData.CompletedVistas.Contains(completableItem.Id) ||
                     player.playerData.CompletedSecretVistas.Contains(completableItem.Id);
            }

            if (completableItem.Type == CompletableItemType.Trivia)
            {
                completableItemPoint = Instantiate(triviaPointPrefab);
                completableItemPoint.name = "TriviaPoint" + completableItem.Id;
                IsCompleted = player.playerData.CompletedTrivias.Contains(completableItem.Id);
                IsUnlocked = player.playerData.UnlockedTrivias.Contains(completableItem.Id);
            }

            completableItemPoint.transform.SetParent(canvas.transform);

            var physicalCompletableItem = Instantiate(physicalMapItemPrefab);
            physicalCompletableItem.name = "PhysicalCompletableItem";
            physicalCompletableItem.name += completableItem.Type == CompletableItemType.Vista ? "Vista" : "Trivia";
            physicalCompletableItem.name += completableItem.Id;

            var position = new GeoPoint(completableItem.Coordinates.Lat, completableItem.Coordinates.Lon);
            if (position == null)
            {
                throw new Exception("Position is null;");
            }
            physicalCompletableItem.GetComponent<ObjectPosition>().setPositionOnMap(position);

            var mapItemBehavior = completableItemPoint.GetComponent<MapItemBehavior>();
            mapItemBehavior.physicMapItem = physicalCompletableItem;

            var completableItemPointBehavior = completableItemPoint.GetComponent<CompletableItemPointBehavior>();

            completableItemPointBehavior.Completed = IsCompleted;
            completableItemPointBehavior.Id = completableItem.Id;
            completableItemPointBehavior.Lat = completableItem.Coordinates.Lat;
            completableItemPointBehavior.Lon = completableItem.Coordinates.Lon;
            completableItemPointBehavior.IsSecret = completableItem.IsSecret;


            var image = completableItemPoint.GetComponent<Image>();

            if (IsCompleted)
            {
                UpdateVisualsOfCompletedItemPoint(completableItemPoint);
            }
            else if (completableItem.Type == CompletableItemType.Trivia)
            {
                if (!IsUnlocked)
                {
                    image.enabled = false;
                }
            }

            if (completableItem.IsSecret)
            {
                var secretVistaPointBehavior = completableItemPoint.AddComponent<SecretVistaPointBehavior>();
                secretVistaPointBehavior.player = player;
                secretVistaPointBehavior.pointBehavior = completableItemPointBehavior;
                secretVistaPointBehavior.image = image;
                image.enabled = false;
            }
        }
    }

    private async Task InstantiateRotatingSprites(IEnumerable<IDataPackage> rotatingItems)
    {
        foreach (RotatingItem rotatingItem in rotatingItems.Cast<RotatingItem>())
        {
            var rotatingItemSprite = Instantiate(rotatingItemPrefab);
            rotatingItemSprite.name = "RotatingItem" + rotatingItem.Id;

            var physicalCompletableItem = Instantiate(physicalMapItemPrefab);
            physicalCompletableItem.name = "PhysicalRotatingItem";
            physicalCompletableItem.name += rotatingItem.Id;

            var position = new GeoPoint(rotatingItem.Coordinates.Lat, rotatingItem.Coordinates.Lon);
            if (position == null)
            {
                throw new Exception("Position is null;");
            }
            physicalCompletableItem.GetComponent<ObjectPosition>().setPositionOnMap(position);

            var mapItemBehavior = rotatingItemSprite.GetComponent<MapItemBehavior>();
            mapItemBehavior.physicMapItem = physicalCompletableItem;

            rotatingItemSprite.transform.SetParent(canvas.transform);

            var rawImageComponent = rotatingItemSprite.GetComponent<RawImage>();
            rawImageComponent.texture = await dataProvider //This could be easily get it from Resources, but this help to reduce the size of the apk.
                .GetImage(RotatingItems + rotatingItem.Id);
            rawImageComponent.SetNativeSize();

            var rectTransform = rotatingItemSprite.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(rectTransform.localScale.x * 4, rectTransform.localScale.y * 4, rectTransform.localScale.z);
        }
    }

    private void CheckPlayerPosition()
    {
        // TODO implement here
    }

    private void LoadPlayerPoint()
    {
        // TODO implement here
    }

    public void UpdateVisualsOfCompletedItemPoint(GameObject competableItemPoint)
    {
        competableItemPoint.GetComponent<Image>().color = new Color(0.7176471f, 0.7176471f, 0.7176471f, 0.8352941f);
    }

    //Dev method
    public void AddNewTestingVista(VistaPosition vistaPosition)
    {
        InstantiateCompletableItemPoints(new HashSet<VistaPosition>() { vistaPosition });
    }

    public async Task AddRotatingItem(GeoPoint playerPosition)
    {
        var offset = UnityEngine.Random.Range(0.0001f, 0.0003f);
        var playerPosCoordinates = new Coordinate() { Lat = playerPosition.lat_d + offset, Lon = playerPosition.lon_d + offset };
        await InstantiateRotatingSprites(new HashSet<RotatingItem>() { new RotatingItem() { Id = 1, Coordinates = playerPosCoordinates } });
    }
}