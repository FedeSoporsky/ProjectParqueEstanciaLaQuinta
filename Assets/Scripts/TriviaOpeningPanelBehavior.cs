using UnityEngine;

public class TriviaOpeningPanelBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject triviaManagerPrefab;

    public GameObject triviaPoint;

    int _triviaId;

    public int TriviaId
    {
        get { return _triviaId; }
        set { _triviaId = value; }
    }

    public void StartTrivia()
    {
        var triviaManager = Instantiate(triviaManagerPrefab);
        var triviaManagerCmp = triviaManager.GetComponent<TriviaManager>();
        triviaManagerCmp.TriviaId = TriviaId;
        triviaManagerCmp.triviaPoint = triviaPoint;
    }
}