using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;

[System.Serializable]
public struct BookPairs
{
    public BookInfo book1;
    public BookInfo book2;
    public Topics pairCategory;
    public Topics pairCategory2;
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

    public static Action<int, int> OnGameEnd;

    void OnEnable()
    {
        Bookshelf.OnSort += BooksToSortTracker;
        BookStack.OnFailBook += BooksToSortTracker;
    }
    void OnDisable()
    {
        Bookshelf.OnSort -= BooksToSortTracker;
        BookStack.OnFailBook -= BooksToSortTracker;
    }

    private void Start()
    {
        // SetLastBooks() // this needs to be callsed first
        GetPairedBooks();// testing only, should be called by main Game Mgr
        StartCoroutine("StartTimer");// testing only, should be called by main Game Mgr

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
        currentTime = initialTime;

        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (currentTime <= 0 || timerIsPaused)
            {
                if (OnGameEnd != null)
                    OnGameEnd(perfectScore, score);
                yield break;
            }
            var greenFill = timerBarFill.color;
            /// Actual computation
            /// (distance / time) * deltaTime
            /// Think of 1 second as 1 meter
            /// so we get
            /// (initialTime / initialTime) * delta time
            /// To simplify we get this:
            currentTime -= Time.deltaTime;

            timerBarFill.fillAmount = currentTime / initialTime;

            var bg = redFill;

            bg = Color.Lerp(bg, yellowFill, System.Convert.ToSingle(timerBarFill.fillAmount >= redThreshold));
            bg = Color.Lerp(bg, blueFill, System.Convert.ToSingle(timerBarFill.fillAmount >= yellowThreshold));
            bg = Color.Lerp(bg, greenFill, System.Convert.ToSingle(timerBarFill.fillAmount >= blueThreshold));

            timerBarFill.color = bg;

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
            if (OnGameEnd != null)
                OnGameEnd(perfectScore, score);
                UnlockNextLevel();

            timerIsPaused = true;
        }
    }

        private void UnlockNextLevel()
    {
        if (GameManager.instance == null)
        {
            Debug.LogWarning("Game Manager is null");
            return;
        }
        
        var l = GameManager.instance.levelManager;
        if(l.levelsUnlocked.Contains(l.selectedLevel + 1))
           return;
        
        l.levelsUnlocked.Add(/*l.selectedLevel + 1*/2);
        l.levelsUnlocked.Add(/*l.selectedLevel + 1*/3);
        l.levelsUnlocked.Add(/*l.selectedLevel + 1*/4);
        
    }
}
