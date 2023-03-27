using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;

[System.Serializable]
public struct SortingBook
{
    public BookInfo book;
    public string category;
}
public class SortingGameManager : MonoBehaviour
{
    [SerializeField] private SortingBook[] books;
    [SerializeField] private BookStack[] cartBooks;
    [SerializeField] private Bookshelf[] bookShelves = new Bookshelf[7];
    private List<string> categoryList = new List<string>();

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
        GetPairedBooks();// testing only, should be called by main Game Mgr
        GetAllCategories();// testing only, should be called by main Game Mgr
        StartCoroutine("StartTimer");// testing only, should be called by main Game Mgr

    }

    public void SetLastBook(BookInfo book, string topic)
    {
        books[books.Length].book = book;
        books[books.Length].category = topic;
    }

    public void GetPairedBooks()
    {
        for (int i = 0; i < cartBooks.Length; i++)
        {
            if (i < books.Length)
            {
                cartBooks[i].SetBooksInStack(books[i].book, books[i].category);
                numOfBooksToSort += 1;
            }
            else
            {
                cartBooks[i].gameObject.SetActive(false);
            }
        }

        perfectScore = numOfBooksToSort;
    }
    void GetAllCategories()
    {
        foreach (SortingBook sB in books)
        {
            categoryList.Add(sB.category);
        }
        categoryList.Add("Nothing Here / Dud");

        SetBookShelfCategoies();
    }
    void SetBookShelfCategoies()
    {
        for(int i = 0; i < bookShelves.Length; i++)
        {
            int n = UnityEngine.Random.Range(0, categoryList.Count);
            string name = categoryList[n];
            bookShelves[i].SetCategory(name);
            categoryList.RemoveAt(n);
        }
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

            timerIsPaused = true;
        }
    }
}
