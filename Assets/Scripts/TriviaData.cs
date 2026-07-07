
using Assets.Scripts.Model.Interfaces;
using System.Collections.Generic;

public class TriviaData : IDataPackage
{
    public TriviaData()
    {
    }

    public int TriviaId;

    public string Title;

    public HashSet<Question> Questions;
}