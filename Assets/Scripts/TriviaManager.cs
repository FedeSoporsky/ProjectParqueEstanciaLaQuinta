using System.Linq;
using UnityEngine;

public class TriviaManager : MonoBehaviour
{
    TriviaData triviaData;
    public GameObject triviaPoint;

    [SerializeField]
    DataProvider dataProvider;
    [SerializeField]
    PanelManager panelManager;
    [SerializeField]
    MapManager mapManager;

    int _triviaId;
    int numberOfQuestions;
    int questionNumber;
    int correctAnswers;

    public int TriviaId
    {
        get { return _triviaId; }
        set { _triviaId = value; }
    }

    readonly string PanelManager = "PanelManager";
    readonly string DataProvider = "DataProvider";
    readonly string GameManager = "GameManager";

    async void Start()
    {
        dataProvider = GameObject.FindGameObjectWithTag(DataProvider).GetComponent<DataProvider>();
        if (dataProvider == null)
        {
            Debug.Log("Failed to load dataProvider at TriviaManger' script.");
        }
        panelManager = GameObject.FindWithTag(PanelManager).GetComponent<PanelManager>();
        mapManager = GameObject.FindWithTag(GameManager).GetComponent<MapManager>();
        triviaData = (TriviaData)await dataProvider.GetTriviaData(TriviaId);

        questionNumber = 0;
        numberOfQuestions = triviaData.Questions.Count();
        correctAnswers = 0;
        StartTrivia();
    }

    private void StartTrivia()
    {
        panelManager.LoadTriviaPanel(triviaData.Questions.Where(x => x.Id == questionNumber).FirstOrDefault(), this);
        questionNumber++;
    }

    public void TriviaQuestionEnded(bool IsCorrectAnswer)
    {
        if (IsCorrectAnswer)
        {
            correctAnswers++;
        }

        if (questionNumber < numberOfQuestions)
        {
            panelManager.LoadTriviaPanel(triviaData.Questions.Where(x => x.Id == questionNumber).FirstOrDefault(), this);
            questionNumber++;
        }
        else
        {
            bool isVictory = correctAnswers > numberOfQuestions / 2;
            panelManager.LoadResultTriviaPanel(isVictory);

            triviaPoint.GetComponent<CompletableItemPointBehavior>().Completed = true;
            mapManager.UpdateVisualsOfCompletedItemPoint(triviaPoint);
        }
    }
}