using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    readonly string VistaMainPictures = "VistaMainPictures";
    readonly string VistaImageContentMiniatures = "VistaImageContentMiniatures";
    readonly string VistaVideoContentMiniatures = "VistaVideoContentMiniatures";
    readonly string VistaMediaContentFullSize = "VistaMediaContentFullSize";
    readonly string Image = "Image";
    readonly string Video = "Video";

    [SerializeField]
    int ScreenWidth;
    [SerializeField]
    int ScreenHeight;

    private GameObject PreviousPanel;

    private GameObject ActivePanel;

    public Panel[] TriviaPanels;

    [SerializeField]
    Player player;
    [SerializeField]
    GameObject SecretVistaBar;
    [SerializeField]
    GameObject MainIconsBar;

    [SerializeField]
    Canvas canvas;
    [SerializeField]
    DataProvider dataProvider;
    [SerializeField]
    MapManager mapManager;
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GameObject MediaContentPrefab;
    [SerializeField]
    GameObject answerPrefab;

    [SerializeField]
    GameObject VistaPanelPrefab;
    [SerializeField]
    GameObject VistaPanelFullPrefab;
    [SerializeField]
    GameObject FullVisualizationPrefab;
    [SerializeField]
    public GameObject SecretVistaPrefab;
    [SerializeField]
    GameObject SecretVistaButtonPrefab;
    [SerializeField]
    GameObject NewUnlockedPetPrefab;
    [SerializeField]
    GameObject TriviaOpeningPrefab;
    [SerializeField]
    GameObject TriviaPrefab;
    [SerializeField]
    GameObject FinalResultTriviaPrefab;

    /// <summary>
    /// @param int vistaId 
    /// @param bool IsSecret
    /// </summary>
    public void LoadVistaPanel(int vistaId, bool IsSecret, GeoPoint geoPoint, GameObject vistaPoint)
    {
        bool isAtSamePosition = player.IsAtSamePosition(geoPoint);

        if (IsSecret && isAtSamePosition)
        {
            LoadNewUnlockedPetPanel();
        }
        else if (!IsSecret)
        {
            LoadCommonVistaPanel(vistaId, isAtSamePosition, vistaPoint);
        }
    }

    public async void LoadCommonVistaPanel(int vistaId, bool IsCompletedOrAtSamePosition, GameObject vistaPoint)
    {
        GameObject prefab = IsCompletedOrAtSamePosition ? VistaPanelFullPrefab : VistaPanelPrefab;

        var vistaData = (VistaData)await dataProvider.GetVistaData(vistaId);

        var panel = InstantiatePanel(prefab);
        panel.name = "UIVista" + vistaId;
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //fix position.

        var panelMask = panel.transform.Find("Mask");
        panelMask.Find("Title").GetComponent<UnityEngine.UI.Text>().text = vistaData.Title;
        panelMask.Find("Subtitle").GetComponent<UnityEngine.UI.Text>().text = vistaData.Subtitle;

        panelMask.Find("VistaPhoto").GetComponent<UnityEngine.UI.RawImage>().texture = await dataProvider
            .GetImage(VistaMainPictures + vistaData.BackgroundImage);

        if (IsCompletedOrAtSamePosition)
        {
            panelMask.Find("BriefDescription").GetComponent<UnityEngine.UI.Text>().text = vistaData.BriefDescription;
            await FillMediaContentGrid(vistaData.ImageCollection, panel.transform, MediaContentPrefab, Image);
            await FillMediaContentGrid(vistaData.VideoCollection, panel.transform, MediaContentPrefab, Video);

            player.SetAchivement(vistaId, AchievementType.Vista);
            vistaPoint.GetComponent<CompletableItemPointBehavior>().Completed = true;
            player.SetCompletedVista(vistaId);
            mapManager.UpdateVisualsOfCompletedItemPoint(vistaPoint);
        }

        ArrangePanels(panel);
    }

    public void LoadNewUnlockedPetPanel()
    {
        var panel = InstantiatePanel(NewUnlockedPetPrefab);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //After this, the fullVistaPanel should be open it.
        ArrangePanels(panel);
    }

    /// <summary>
    /// @param int triviaId
    /// </summary>
    public async void LoadOpeningTriviaPanel(int triviaId, GeoPoint geoPoint, GameObject triviaPoint)
    {
        bool isAtSamePosition = player.IsAtSamePosition(geoPoint);

        if (isAtSamePosition)
        {
            var triviaData = (TriviaData)await dataProvider.GetTriviaData(triviaId);


            ShowSecretVistaBar(false);

            var panel = InstantiatePanel(TriviaOpeningPrefab);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //fix position
            panel.transform.Find("Subtitle").GetComponent<UnityEngine.UI.Text>().text = triviaData.Title;
            panel.GetComponent<TriviaOpeningPanelBehavior>().TriviaId = triviaId;
            panel.GetComponent<TriviaOpeningPanelBehavior>().triviaPoint = triviaPoint;

            ArrangePanels(panel);
        }
    }

    public void LoadTriviaPanel(Question question, TriviaManager triviaManager)
    {
        ShowSecretVistaBar(false);
        ShowMainIconPool(false);

        var panel = InstantiatePanel(TriviaPrefab);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //fix position
        panel.GetComponent<TriviaPanelBehavior>().TriviaManager = triviaManager;
        panel.transform.Find("Title").GetComponent<UnityEngine.UI.Text>().text = question.QuestionText;

        var answerContainer = panel.transform.Find("AnswerContainer");
        foreach (var answer in question.Answers)
        {
            var answerGO = Instantiate(answerPrefab);
            answerGO.transform.SetParent(answerContainer.transform);

            var triviaAnswerButtonBehavior = answerGO.GetComponent<TriviaAnswerButtonBehavior>();
            triviaAnswerButtonBehavior.Answer = answer;
            triviaAnswerButtonBehavior.TriviaManager = triviaManager;
            answerGO.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = answer.Text;
        }

        ArrangePanels(panel);
    }

    /// <summary>
    /// @param Result result 
    /// @param bool IsFinalResult 
    /// @param float stars
    /// </summary>
    public void LoadResultTriviaPanel(TriviaResult result, bool IsFinalResult, float stars)
    {
        // TODO implement here
    }

    public void LoadResultTriviaPanel(bool isVictory)
    {
        string image = isVictory ? "Victory" : "Defeat";

        var panel = InstantiatePanel(FinalResultTriviaPrefab);

        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //fix position
        panel.GetComponent<FinalResultTriviaPanelBehavior>().panelManager = this;
        panel.transform.Find(image).gameObject.SetActive(true);
        panel.transform.Find("Title").GetComponent<UnityEngine.UI.Text>().text = isVictory ? "¡Ganaste!" : "¡Perdiste!";

        ArrangePanels(panel);
    }

    /// <summary>
    /// @param string panelPrefab
    /// </summary>
    public void LoadPanel(string panelPrefab)
    {
        // TODO implement here
    }

    /// <summary>
    /// @param int imageId
    /// </summary>
    public void ChangeSecretVistaBackgroundImage(int imageId)
    {
        // TODO implement here
    }

    /// <summary>
    /// </summary>
    public void LoadAddFriendPanel()
    {
        // TODO implement here
    }

    public void LoadChangingRoomPanel()
    {
        // TODO implement here
    }

    /// <summary>
    /// @param int archiveItemId
    /// </summary>
    public void LoadArchivePanel(int archiveItemId)
    {
        // TODO implement here
    }

    /// <summary>
    /// @param int fullImageId
    /// </summary>
    public async void LoadFullVisualizationPanel(int fullImageId)
    {
        var fullVisualizationPanel = InstantiatePanel(FullVisualizationPrefab);

        var imageSelected = fullVisualizationPanel.transform.Find("ImageSelected");
        var rawImageCmp = imageSelected.GetComponent<UnityEngine.UI.RawImage>();
        var rectTransform = imageSelected.GetComponent<RectTransform>();

        rawImageCmp.texture = await dataProvider.GetImage(VistaMediaContentFullSize + fullImageId);
        rawImageCmp.SetNativeSize();

        if (rectTransform.sizeDelta.x > ScreenWidth)
        {
            var scale = ScreenWidth / rectTransform.sizeDelta.x;

            rectTransform.sizeDelta = new Vector2(ScreenWidth, rectTransform.sizeDelta.y * scale);
        }

        ArrangePanels(fullVisualizationPanel);
    }

    public async void LoadSecretVistaPanel()
    {
        var secretVistaPanel = InstantiatePanel(SecretVistaPrefab);
        var mediaContentGrid = secretVistaPanel.transform.Find("MediaScroll/MediaViewport/MediaContent");
        secretVistaPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //fix position.


        var availableSecretVistas = gameManager.SecretVistas.Except(player.GetCompletedSecretVistas()).ToHashSet();

        var panelMask = secretVistaPanel.transform.Find("Mask");
        panelMask.Find("VistaPhoto").GetComponent<UnityEngine.UI.RawImage>().texture = await dataProvider
            .GetImage(VistaMainPictures + availableSecretVistas.First());
        panelMask.Find("Title").GetComponent<UnityEngine.UI.Text>().text = "Secret Vista " + availableSecretVistas.First();

        var secretVistaPanelBehavior = secretVistaPanel.GetComponent<SecretVistaPanelBehavior>();

        foreach (var secretVista in availableSecretVistas)
        {
            var secretVistaButton = Instantiate(SecretVistaButtonPrefab);
            secretVistaButton.name = "SecretVistaButton" + secretVista;
            secretVistaButton.transform.SetParent(mediaContentGrid.transform);

            var secretVistaButtonBehavior = secretVistaButton.GetComponent<SecretVistaButtonBehavior>();
            secretVistaButtonBehavior.panelBehavior = secretVistaPanelBehavior;
            secretVistaButtonBehavior.ImageId = secretVista;

            //Could this be just one call?
            secretVistaPanelBehavior.BackgroundImages.Add(await dataProvider
            .GetImage(VistaMainPictures + secretVista));
        }

        ArrangePanels(secretVistaPanel);
    }

    public void GoBackToPrevious()
    {
        Destroy(ActivePanel);
        ActivePanel = PreviousPanel;
        if (ActivePanel != null)
        {
            ActivePanel.SetActive(true);
        }
    }

    void ArrangePanels(GameObject newPanel)
    {
        PreviousPanel = ActivePanel;
        ActivePanel = newPanel;

        if (PreviousPanel != null)
        {
            PreviousPanel.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        PreviousPanel = ActivePanel;
        Destroy(ActivePanel);
    }

    GameObject InstantiatePanel(GameObject panel)
    {
        var panelGO = Instantiate(panel);
        panelGO.transform.SetParent(canvas.transform);

        return panelGO;
    }

    internal void ShowSecretVistaBar(bool value)
    {
        SecretVistaBar.SetActive(value);
    }
    public void ShowMainIconPool(bool value)
    {
        MainIconsBar.SetActive(value);
    }

    async Task FillMediaContentGrid(HashSet<int> mediaCollection, Transform panel, GameObject prefab, string mediaType)
    {
        var mediaContentGrid = panel.transform.Find("MediaScroll/MediaViewport/MediaContent");

        foreach (var mediaId in mediaCollection)
        {
            var mediaContentObject = Instantiate(prefab);
            mediaContentObject.name = mediaType + mediaId;
            mediaContentObject.transform.SetParent(mediaContentGrid.transform);

            var route = mediaType == Video ? VistaVideoContentMiniatures : VistaImageContentMiniatures;
            mediaContentObject.GetComponent<UnityEngine.UI.RawImage>().texture = await dataProvider
                .GetImage(route + mediaId);

            if (mediaType == Image)
            {
                mediaContentObject.GetComponent<FullVisualizationBehavior>().FullImageId = mediaId;
            }
            else
            {
                //Video things...
            }
        }
    }
}