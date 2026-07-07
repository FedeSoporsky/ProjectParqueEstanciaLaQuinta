using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryVistaInfo", menuName = "Create PrimaryVistaInfo")]
public class PrimaryVistaInfo : ScriptableObject
{
    public int VistaId;

    public bool IsSecret;

    public float Lat;

    public float Lon;
}