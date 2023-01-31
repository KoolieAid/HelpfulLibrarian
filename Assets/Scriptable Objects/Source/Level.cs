using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level")]
public class Level : ScriptableObject
{
    [Tooltip("Theres a chance that one of these are going into the wrong answer section")]
    [SerializeField]
    private List<ReaderManager.BookInfo> correctAnswers = new();

    [Tooltip("This will NEVER become a correct answer")]
    [SerializeField]
    private List<ReaderManager.BookInfo> wrongAnswers = new();

    public ReadOnlyCollection<ReaderManager.BookInfo> GetCorrectAnswers()
    {
        return new ReadOnlyCollection<ReaderManager.BookInfo>(correctAnswers);
    }

    public ReadOnlyCollection<ReaderManager.BookInfo> GetWrongAnswers()
    {
        return new ReadOnlyCollection<ReaderManager.BookInfo>(wrongAnswers);
    }
}