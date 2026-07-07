using System.Collections;
using UnityEngine;

public class SecretVistaBarBehavior : MonoBehaviour
{
    [SerializeField]
    PanelManager panelManager;

    readonly string PanelManager = "PanelManager";

    IEnumerator Start()
    {
        while (panelManager == null)
        {
            var panelManagerObject = GameObject.FindWithTag(PanelManager);
            panelManager = panelManagerObject.GetComponent<PanelManager>();

            if (panelManager == null)
            {
                Debug.Log("Failed to load PanelManager at SecretVistaBarBehavior' script.");
                yield return null;
            }

            while (panelManager.SecretVistaPrefab == null)
            {
                Debug.Log("Failed to load SecretVistaPrefab at PanelManager.");
                yield return null;
            }
        }
    }

    public void OpenPanel()
    {
        panelManager.LoadSecretVistaPanel();
    }
}