using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    private int levelNum;

    [Tooltip("Theres a chance that one of these are going into the wrong answer section")]
    [SerializeField]
    private List<BookInfo> correctAnswers = new();

    [Tooltip("This will NEVER become a correct answer")]
    [SerializeField]
    private List<BookInfo> wrongAnswers = new();

    public ReadOnlyCollection<BookInfo> GetCorrectAnswers()
    {
        return new ReadOnlyCollection<BookInfo>(correctAnswers);
    }

    public ReadOnlyCollection<BookInfo> GetWrongAnswers()
    {
        return new ReadOnlyCollection<BookInfo>(wrongAnswers);
    }

    public int GetLevelNumber()
    {
        return levelNum;
    }
}