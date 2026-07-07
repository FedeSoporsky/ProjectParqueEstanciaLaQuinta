using UnityEngine;

public class ScreenTransitionBehavior : MonoBehaviour
{
    [SerializeField]
    PanelManager panelManager;  

    int _FullImageId;

    public int FullImageId
    {
        get { return _FullImageId; }
        set { _FullImageId = value; }
    }

    readonly string PanelManager = "PanelManager";

    void Start()
    {
        panelManager = GameObject.FindWithTag(PanelManager).GetComponent<PanelManager>();
    }

    public void OpenPanel()
    {
        panelManager.LoadFullVisualizationPanel(FullImageId);
    }

    public int? SceneId;

    public ScreenTransitionData ScreenTransitionData;

}