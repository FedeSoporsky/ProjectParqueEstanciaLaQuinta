using UnityEngine;

public class FinalResultTriviaPanelBehavior : MonoBehaviour
{
    [SerializeField]
    public PanelManager panelManager;

    public void ClosePanel()
    {
        panelManager.ClosePanel();
    }
}
