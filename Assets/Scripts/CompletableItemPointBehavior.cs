using UnityEngine;

public class CompletableItemPointBehavior : MonoBehaviour
{
    public bool Completed { get; set; }

    [SerializeField]
    PanelManager panelManager;

    [SerializeField]
    private int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public bool IsSecret;

    public float Lat;

    public float Lon;

    readonly string PanelManager = "PanelManager";

    internal GeoPoint geoPoint;

    private void Start()
    {
        panelManager = GameObject.FindGameObjectWithTag(PanelManager).GetComponent<PanelManager>();
        geoPoint = new GeoPoint(Lat, Lon);
    }

    /// <summary>
    /// </summary>
    public void GetVistaInfoAndCallLoadPanel()
    {
        IsSecret = false;
        if (!Completed)
        {
            panelManager.LoadVistaPanel(Id, IsSecret, new GeoPoint(Lat, Lon), transform.gameObject);
        }
        else
        {
            panelManager.LoadCommonVistaPanel(Id, Completed, transform.gameObject);
        }
    }

    public void LoadTriviaPanel()
    {
        panelManager.LoadOpeningTriviaPanel(Id, new GeoPoint(Lat, Lon), transform.gameObject);
    }
}