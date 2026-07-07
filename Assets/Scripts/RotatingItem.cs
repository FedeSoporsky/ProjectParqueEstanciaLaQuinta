using Assets.Scripts.Model.Interfaces;
using Assets.Scripts.Model;

public class RotatingItem : IDataPackage
{
    private int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    private Coordinate coordinates;
    public Coordinate Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }
}