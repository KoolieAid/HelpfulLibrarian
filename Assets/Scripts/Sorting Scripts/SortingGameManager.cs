using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BookPairs
{
    public BookInfo book1;
    public BookInfo book2;
    public Topics pairCategory;
}
public class SortingGameManager : MonoBehaviour
{
    [SerializeField] private BookPairs[] pairedBooks;
    [SerializeField] private BookStack[] stackedBooks;

    private int numOfBooksToSort;
    private int perfectScore;
    private int score;

    public delegate void BooksSortAction(int perfectScore, int score);
    public static event BooksSortAction OnSortAll;

    void OnEnable()
    {
        Bookshelf.OnSort += BooksToSortTracker;
        BookStack.OnFailBookSort += BooksToSortTracker;
    }
    void OnDisable()
    {
        Bookshelf.OnSort -= BooksToSortTracker;
        BookStack.OnFailBookSort -= BooksToSortTracker;
    }


    private void Start()
    {
        GetPairedBooks();// testing only
    }

    public void SetLastPairedBooks(BookInfo book1, BookInfo book2, Topics topic)
    {
        pairedBooks[pairedBooks.Length].book1 = book1;
        pairedBooks[pairedBooks.Length].book2 = book2;
        pairedBooks[pairedBooks.Length].pairCategory = topic;
    }

    public void GetPairedBooks()
    {
        for (int i = 0; i < stackedBooks.Length; i++)
        {
            if (i < pairedBooks.Length)
            {
                stackedBooks[i].SetBooksInStack(pairedBooks[i].book1, pairedBooks[i].book2, pairedBooks[i].pairCategory);
                numOfBooksToSort += 1;
            }
            else
            {
                stackedBooks[i].gameObject.SetActive(false);
            }
        }

        perfectScore = numOfBooksToSort;
    }

    void BooksToSortTracker(bool isCorrect, string name)
    {
        if (isCorrect)
        {
            numOfBooksToSort -= 1;
            score += 1;
        }
        if (!isCorrect && name == "failed")
        {
            numOfBooksToSort -= 1;
        }

        if (numOfBooksToSort <= 0)
        {
            if (OnSortAll != null)
                OnSortAll(perfectScore, score); // Done Sorting All Books Event
        }
    }

    
}
