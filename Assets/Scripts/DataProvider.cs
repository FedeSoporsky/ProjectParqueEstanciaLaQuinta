using Assets.Scripts.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Assets.Scripts.Model.Types;

public class DataProvider : MonoBehaviour
{
    public HashSet<PrimaryVistaInfo> AvailableVistas;

    public HashSet<TriviaPosition> AvaiableTrivias;

    public HashSet<VistaData> AvailableVistaDatas;

    public HashSet<PetData> AvailablePetData;

    public async Task<IEnumerable<IDataPackage>> LoadData(string assetKey)
    {
        var jsonTextFile = await Addressables.LoadAssetAsync<TextAsset>(assetKey).Task;

        if (jsonTextFile != null)
        {
            switch (assetKey)
            {
                case DataTypes.VistasData:
                    return JsonConvert.DeserializeObject<HashSet<VistaData>>(jsonTextFile.text);
                case DataTypes.TriviasData:
                    return JsonConvert.DeserializeObject<HashSet<TriviaData>>(jsonTextFile.text);
                case DataTypes.VistaPositions:
                    return JsonConvert.DeserializeObject<HashSet<VistaPosition>>(jsonTextFile.text);
                case DataTypes.TriviaPositions:
                    return JsonConvert.DeserializeObject<HashSet<TriviaPosition>>(jsonTextFile.text);
                case DataTypes.PlayersData:
                    return JsonConvert.DeserializeObject<HashSet<PlayerData>>(jsonTextFile.text);
                case DataTypes.RotatingItems:
                    return JsonConvert.DeserializeObject<HashSet<RotatingItem>>(jsonTextFile.text);
                default:
                    break;
            }
        }

        return null;
    }

    /// <summary>
    /// @param int vistaId 
    /// @return
    /// </summary>
    public async Task<IDataPackage> GetVistaData(int vistaId)
    {
        IEnumerable<IDataPackage> vistasData = new HashSet<VistaData>();
        vistasData = await LoadData(DataTypes.VistasData);
        var vistaData = (vistasData as HashSet<VistaData>)
                            .Where(x => x.VistaId == vistaId).Select(x => x)
                            .FirstOrDefault();
        return vistaData;
    }

    /// <summary>
    /// @param int triviaId 
    /// @return
    /// </summary>
    public async Task<IDataPackage> GetTriviaData(int triviaId)
    {
        IEnumerable<IDataPackage> triviasData = new HashSet<TriviaData>();
        triviasData = await LoadData(DataTypes.TriviasData);
        var triviaData = (triviasData as HashSet<TriviaData>)
                            .Where(x => x.TriviaId == triviaId).Select(x => x)
                            .FirstOrDefault();
        return triviaData;
    }

    /// <summary>
    /// @param int[] ids   
    /// @return
    /// </summary>
    public HashSet<PetData> GetPetData(int[] ids)
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @param int publicId 
    /// @return
    /// </summary>
    public Friend GetFriend(int publicId)
    {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @return
    /// </summary>
    public async Task<(IEnumerable<IDataPackage>, IEnumerable<IDataPackage>)> GetCompletableItems()
    {
        IEnumerable<IDataPackage> VistaPositions = await LoadData(DataTypes.VistaPositions);
        foreach (ICompletableItem item in VistaPositions.Cast<ICompletableItem>())
        {
            item.Type = CompletableItemType.Vista;
        }
        IEnumerable<IDataPackage> TriviaPositions = await LoadData(DataTypes.TriviaPositions);
        foreach (ICompletableItem item in TriviaPositions.Cast<ICompletableItem>())
        {
            item.Type = CompletableItemType.Trivia;
        }
        return (VistaPositions, TriviaPositions);
    }

    public async Task<IEnumerable<IDataPackage>> GetRotatingItems()
    {
        return await LoadData(DataTypes.RotatingItems);
    }

    /// <summary>
    /// @param int[] ids 
    /// @return
    /// </summary>
    public Dictionary<HashSet<ImageAsset>, HashSet<Video>> GetMedia(int[] vistaIds = null, int[] imageIds = null, int[] videoIds = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// </summary>
    public async Task<Texture2D> GetImage(string mediaLabel)
    {
        return await Addressables.LoadAssetAsync<Texture2D>(mediaLabel).Task;
    }

    /// <summary>
    /// @param int[] ids  
    /// @return
    /// </summary>
    public Dictionary<HashSet<ImageAsset>, HashSet<Video>> GetMedia(int[] imageIds = null)
    {
        throw new NotImplementedException();
    }
    public HashSet<Cosmetic> GetCosmetics(int[] cosmeticIds)
    {
        throw new NotImplementedException();
    }

    public async Task<IDataPackage> GetPlayerData()
    {
        return (await LoadData(DataTypes.PlayersData)).FirstOrDefault();
    }

    internal static class DataTypes
    {
        internal const string VistasData = "VistasData";
        internal const string TriviasData = "TriviasData";
        internal const string VistaPositions = "VistaPositions";
        internal const string TriviaPositions = "TriviaPositions";
        internal const string PlayersData = "PlayersData";
        internal const string RotatingItems = "RotatingItems";
    }
}