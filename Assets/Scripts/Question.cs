using System.Collections.Generic;

public class Question
{
    public int Id;

    public string QuestionText;

    public HashSet<Answer> Answers;
}