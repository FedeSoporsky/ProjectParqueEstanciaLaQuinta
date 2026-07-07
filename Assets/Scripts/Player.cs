using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    DataProvider dataProvider;

    public PlayerData playerData;

    readonly float geoPositionOffset = 1.0f;

    readonly string DataProviderTag = "DataProvider";

    bool _IsDataReady;

    public bool IsDataReady
    {
        get { return _IsDataReady; }
        set { _IsDataReady = value; }
    }

    [SerializeField]
    bool persistData = false; //This flag is for avoid persisting data in testing.

    void Start()
    {
        IsDataReady = false;
        StartCoroutine(Initialization());
    }

    private void Update()
    {

    }

    IEnumerator Initialization()
    {
        while (dataProvider == null)
        {
            dataProvider = GameObject.FindWithTag(DataProviderTag).GetComponent<DataProvider>();
            if (dataProvider == null)
            {
                Debug.Log("Failed to load dataProvider at Player' script.");
                yield return null;
            }
        }

        if (playerData == null)
        {
            var t = dataProvider.GetPlayerData();
            yield return new WaitUntil(() => t.IsCompleted);
            playerData = (PlayerData)t.Result;

            if (playerData == null)
            {
                Debug.Log("Failed to load playerData at Player' script.");
                yield return null;
            }
        }

        IsDataReady = true;
    }

    public bool IsAtSamePosition(GeoPoint objetive)
    {
        var objectivePosition = GameManager.Instance.getMainMapMap().getPositionOnMap(objetive);
        //GameManager returns positions based on x & z.
        var vector2Position = new Vector2(transform.position.x, transform.position.z);
        var distance = Vector2.Distance(vector2Position, objectivePosition); //same as (a-b).magnitude

        if (distance < geoPositionOffset)
        {
            return true;
        };
        return false;
    }

    /// <summary>
    /// @return
    /// </summary>
    public HashSet<Friend> GetFriends()
    {
        // TODO implement here
        return null;
    }

    public void GetPublicInfo()
    {
        // TODO implement here
    }

    /// <summary>
    /// <param name="friends">Testing</param>
    /// </summary>
    public void SetFriends(Friend[] friends)
    {
        SaveChanges();
        // TODO implement here
    }

    public HashSet<int> GetCompletedSecretVistas()
    {
        return playerData.CompletedSecretVistas;
    }
    /// <summary>
    /// @return
    /// </summary>
    public HashSet<Achievement> GetAchivements(AchievementType achivementType)
    {
        return playerData.Achivements
            .Where(x => x.Type == achivementType)
            .ToHashSet();
    }

    public void SetAchivement(int vistaId, AchievementType achivementType)
    {
        var achivements = GetAchivements(achivementType).Where(x => x.Id == vistaId);

        if (achivements != null)
        {
            playerData.Achivements.Add(new Achievement
            {
                Type = achivementType,
                Id = vistaId
            });
        }

        SaveChanges();
    }

    private void SaveChanges()
    {
        if (persistData)
        {
            var json = JsonConvert.SerializeObject(playerData);

            var dir = Application.dataPath + "/AddressableContent/";
            File.WriteAllText(dir + "testing2.json", json);
        }
    }

    public HashSet<int> GetCompletedVistas()
    {
        return playerData.CompletedVistas;
    }

    public void SetCompletedVista(int vistaId)
    {
        playerData.CompletedVistas.Add(vistaId);
        SaveChanges();
    }

    /// <summary>
    /// @return
    /// </summary>
    public Skin GetSkinApplied()
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @return
    /// </summary>
    public PetSkin GetPetSkinApplied()
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @param Cosmetic cosmetic
    /// </summary>
    public void SetPetSkin(Cosmetic cosmetic)
    {
        // TODO implement here
        SaveChanges();
    }

    /// <summary>
    /// @param Cosmetic cosmetic
    /// </summary>
    public void SetSkin(Cosmetic cosmetic)
    {
        // TODO implement here
        SaveChanges();
    }

    /// <summary>
    /// @return
    /// </summary>
    public HashSet<Pet> GetUnlockedPets()
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @param CollectionPet pets
    /// </summary>
    public void SetUnlockedPets(HashSet<Pet> pets)
    {
        // TODO implement here
        SaveChanges();
    }

    /// <summary>
    /// @return
    /// </summary>
    public HashSet<int> GetUnlockedArchiveItems()
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @return
    /// </summary>
    public HashSet<Cosmetic> GetUnlockedCosmetics()
    {
        // TODO implement here
        return null;
    }

}