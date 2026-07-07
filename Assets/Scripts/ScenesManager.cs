using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    public readonly string Map = "Map";
    public readonly string Intro = "Intro";
    public readonly string PanelManager = "PanelManager";
    public readonly string InsiderLoadingBar = "InsiderLoadingBar";
    public readonly string SecretVistaBar = "SecretVistaBar";
    GameObject insiderLoadingBar;
    GameObject canvas;
    PanelManager panelManager;

    [SerializeField]
    public Scene sceneSelected;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == Intro)
        {
            LoadScene(Map);
        }
    }

    private void LoadLoadingBar()
    {
        if (SceneManager.GetActiveScene().name == Map)
        {
            panelManager = GameObject.FindGameObjectWithTag(PanelManager).GetComponent<PanelManager>();
        }
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        insiderLoadingBar = canvas.transform.Find("LoadingPanel").Find(InsiderLoadingBar).gameObject;
    }

    public void LoadScene(string scene)
    {
        LoadLoadingBar();
        if (SceneManager.GetActiveScene().name != scene)
        {
            StartCoroutine(LoadGameSceneAsync(scene));
        }
        else
        {
            if (scene == Map)
            {
                panelManager.ClosePanel();
                panelManager.ShowSecretVistaBar(true);
                panelManager.ShowMainIconPool(true);
            }
        }
    }

    private IEnumerator LoadGameSceneAsync(string scene)
    {
        UnityEngine.SceneManagement.Scene loaderScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        insiderLoadingBar.transform.parent.gameObject.SetActive(true);
        var InsideLoadingBarRTransform = insiderLoadingBar.GetComponent<RectTransform>();
        float loadingBarFullSize = 1187.9f;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress);
            var size = new Vector2(loadingBarFullSize * progress, InsideLoadingBarRTransform.sizeDelta.y);
            InsideLoadingBarRTransform.sizeDelta = size;
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(loaderScene);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }


        yield return new WaitForSeconds(1);
    }

    public enum Scene
    {
        Map,
        Intro,
        Archive,
        Profile
    }
}