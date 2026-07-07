using System.Collections;
using UnityEngine;

public class TriviaPanelBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject TimeBar;

    [SerializeField]
    float timeByQuestion;

    float TimeBarFullSize;
    RectTransform TimeBarRTransform;

    TriviaManager _triviaManager;

    public TriviaManager TriviaManager
    {
        get { return _triviaManager; }
        set { _triviaManager = value; }
    }

    void Start()
    {
        TimeBarFullSize = 1187.9f;
        TimeBarRTransform = TimeBar.GetComponent<RectTransform>();
        StartCoroutine(StartTimeBar());
    }

    private IEnumerator StartTimeBar()
    {
        float progress = 0;
        float increment = 1 / timeByQuestion;
        while (progress <= 1f)
        {
            var size = new Vector2(TimeBarFullSize * progress, TimeBarRTransform.sizeDelta.y);
            progress += increment;
            TimeBarRTransform.sizeDelta = size;
            yield return new WaitForSeconds(1);
        }

        TriviaManager.TriviaQuestionEnded(false);
    }
}