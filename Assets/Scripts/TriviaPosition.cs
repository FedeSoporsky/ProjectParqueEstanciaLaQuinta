
using Assets.Scripts.Model;
using Assets.Scripts.Model.Interfaces;

public class TriviaPosition : IDataPackage, ICompletableItem
{
    private int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    private bool isSecret;
    public bool IsSecret
    {
        get { return isSecret; }
        set { isSecret = value; }
    }

    private Coordinate coordinates;
    public Coordinate Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }

    private string type;

    public string Type
    {
        get { return type; }
        set { type = value; }
    }
}