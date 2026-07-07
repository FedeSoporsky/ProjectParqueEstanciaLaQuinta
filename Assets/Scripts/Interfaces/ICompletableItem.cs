namespace Assets.Scripts.Model.Interfaces
{
    public interface ICompletableItem
    {
        int Id { get; set; }

        Coordinate Coordinates { get; set; }

        bool IsSecret { get; set; }

        string Type { get; set; }
    }
}