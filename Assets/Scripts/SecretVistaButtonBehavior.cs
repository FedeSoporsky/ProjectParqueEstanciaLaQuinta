using UnityEngine;

public class SecretVistaButtonBehavior : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public SecretVistaPanelBehavior panelBehavior;

    int _ImageId;
    public int ImageId
    {
        get { return _ImageId; }
        set { _ImageId = value; }
    }

    public void ChangeSecretVista()
    {
        panelBehavior.ChangeSecretVista(ImageId);
    }
}
