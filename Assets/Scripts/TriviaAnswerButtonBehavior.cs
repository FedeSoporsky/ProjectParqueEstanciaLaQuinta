using UnityEngine;

public class TriviaAnswerButtonBehavior : MonoBehaviour
{
    Answer _answer;

    public Answer Answer
    {
        get { return _answer; }
        set { _answer = value; }
    }

    TriviaManager _triviaManager;

    public TriviaManager TriviaManager
    {
        get { return _triviaManager; }
        set { _triviaManager = value; }
    }

    public void ResolveAnswerSelection()
    {
        TriviaManager.TriviaQuestionEnded(Answer.IsCorrect);
    }
}