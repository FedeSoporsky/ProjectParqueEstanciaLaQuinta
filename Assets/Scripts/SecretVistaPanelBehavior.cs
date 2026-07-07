using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SecretVistaPanelBehavior : MonoBehaviour
{
    public HashSet<Texture2D> BackgroundImages;

    void Awake()
    {
        BackgroundImages = new HashSet<Texture2D>();
    }

    public void ChangeSecretVista(int imageId)
    {
        var panelMask = transform.Find("Mask");
        panelMask.Find("VistaPhoto").GetComponent<UnityEngine.UI.RawImage>().texture = BackgroundImages
            .Where(image => image.name == imageId.ToString()).FirstOrDefault();

        panelMask.Find("Title").GetComponent<UnityEngine.UI.Text>().text = "Secret Vista " + imageId;
    }
}