using UnityEngine;

public class MapItemBehavior : MonoBehaviour
{
    [SerializeField]
    public GameObject physicMapItem;

    [SerializeField]
    Camera cam;

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

    readonly string MainCamera = "MainCamera";

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag(MainCamera).GetComponent<Camera>();
    }

    void Update()
    {
        if (physicMapItem != null)
        {
            PhysicVistaTracking();
        }
    }

    //Update position of buttons in the screen.
    void PhysicVistaTracking()
    {
        var screenPos = cam.WorldToScreenPoint(physicMapItem.transform.position);
        GetComponent<RectTransform>().position = screenPos;
    }
}
