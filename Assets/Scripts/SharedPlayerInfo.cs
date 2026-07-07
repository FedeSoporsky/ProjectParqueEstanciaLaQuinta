using System.Collections.Generic;

public class SharedPlayerInfo
{
    public SharedPlayerInfo()
    {
    }

    public int PublicId;

    public string Name;

    public Pet Pet;

    public HashSet<int> Achievements;

    public Skin SkinApplied;
}