
using Assets.Scripts.Model.Interfaces;
using System.Collections.Generic;

public class VistaData : IDataPackage
{
    public VistaData()
    {
    }

    public int VistaId;

    public float Position;

    public string Title;

    public string Subtitle;

    public HashSet<int> ImageCollection;

    public HashSet<int> VideoCollection;

    public CategoryEnum Category;

    public string Description;

    public string BriefDescription;

    public int BackgroundImage;

}