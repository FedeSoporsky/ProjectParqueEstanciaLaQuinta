using Assets.Scripts.Model.Interfaces;
using System.Collections.Generic;

public class PlayerData : IDataPackage
{
    public HashSet<Friend> Friends; //Not loaded when calling GetPlayerData()

    public int Id;

    public int PublicId;

    public HashSet<Achievement> Achivements; //Not loaded when calling GetPlayerData()

    public HashSet<int> UnlockedPets;

    public Skin SkinApplied; //Not loaded when calling GetPlayerData()

    public Pet Pet; //Not loaded when calling GetPlayerData()

    public HashSet<int> CompletedVistas;

    public HashSet<int> CompletedSecretVistas;

    public HashSet<int> UnlockedTrivias;

    public HashSet<int> CompletedTrivias;

    public HashSet<int> UnlockedArchiveItems;

    public SharedPlayerInfo SharedPlayerInfo; //Not loaded when calling GetPlayerData()

    public bool PlayerOnVistaPosition; //Not loaded when calling GetPlayerData()

    public ObjectPosition PlayerPosition; //Not loaded when calling GetPlayerData()

    public HashSet<int> UnlockedCosmetics;
}