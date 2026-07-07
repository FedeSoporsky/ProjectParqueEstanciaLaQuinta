using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBarIconBehavior : MonoBehaviour
{
    ScenesManager scenesManager;

    readonly string scenesManagerTag = "ScenesManager";
    [SerializeField]
    string scene;

    void Start()
    {
        var scenesManagers = GameObject.FindGameObjectsWithTag(scenesManagerTag);
        if (scenesManagers.Count() >= 2)
        {
            scenesManager = scenesManagers.Where(x => x.scene.name != SceneManager.GetActiveScene().name)
                .FirstOrDefault()
                .GetComponent<ScenesManager>();
        }
        else
        {
            //Only from Editor execution.
            scenesManager = scenesManagers.FirstOrDefault().GetComponent<ScenesManager>();
        }

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        scenesManager.LoadScene(scene);
    }
}
