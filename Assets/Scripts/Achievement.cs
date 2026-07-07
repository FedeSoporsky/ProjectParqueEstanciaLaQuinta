public enum AchievementType
{
    Vista
}
public class Achievement
{

    public Achievement()
    {
    }

    public int Id;
    /// <summary>
    /// The image of the achievement.
    /// </summary>
    public int Badge;

    public AchievementType Type;

}