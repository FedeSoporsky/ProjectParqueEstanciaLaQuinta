using UnityEngine;

public class FullVisualizationBehavior : MonoBehaviour
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

    public void GoBackToPrevious()
    {
        panelManager.GoBackToPrevious();
    }
}