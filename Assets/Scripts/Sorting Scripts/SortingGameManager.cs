using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

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

    [Header("Timer Variables")]
    [SerializeField] private Image timerBarFill;
    [SerializeField] private float initialTime;
    private float currentTime;
    private bool timerIsPaused = false;

    [Header("Timer Bar Aesthetics")]
    [SerializeField] private Color blueFill;
    [SerializeField] private Color yellowFill;
    [SerializeField] private Color redFill;
    [SerializeField] [Range(0, 1)] private float blueThreshold;
    [SerializeField] [Range(0, 1)] private float yellowThreshold;
    [SerializeField] [Range(0, 1)] private float redThreshold;

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
        GetPairedBooks();// testing only, should be called by main Game Mgr
        //StartTimer(); // testing only, should be called by main Game Mgr
        StartCoroutine("StartTimer");
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
    IEnumerator StartTimer()
    {
        Debug.Log("StartTimer()");
        currentTime = initialTime;

        while (true)
        {
            var greenFill = timerBarFill.color;
            currentTime -= 1.0f;
            

            timerBarFill.fillAmount = currentTime / initialTime;

            var bg = redFill;

            bg = Color.Lerp(bg, yellowFill, System.Convert.ToSingle(timerBarFill.fillAmount >= redThreshold));
            bg = Color.Lerp(bg, blueFill, System.Convert.ToSingle(timerBarFill.fillAmount >= yellowThreshold));
            bg = Color.Lerp(bg, greenFill, System.Convert.ToSingle(timerBarFill.fillAmount >= blueThreshold));

            timerBarFill.color = bg;

            if (currentTime <= 0 || timerIsPaused)
            {
                if (OnSortAll != null)
                    OnSortAll(perfectScore, score);
                yield break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        
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

            timerIsPaused = true;
        }
    }

    
}
