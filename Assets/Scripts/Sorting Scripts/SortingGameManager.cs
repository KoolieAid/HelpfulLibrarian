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
    [SerializeField] private List<SortingBook> sortingBookList = new List<SortingBook>();
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
        // [In Order]
        // SetSortingBookList();
        SetCartBooksData();// testing only, should be called by what ever starts the game
        GetAllCategories();// testing only, should be called by  what ever starts the game
        StartCoroutine("StartTimer");// testing only, should be called by  what ever starts the game

    }

    public void SetSortingBookList(List<BookInfo> bookList)
    {
        foreach (BookInfo b in bookList)
        {
            SortingBook sortingBook = new SortingBook();
            sortingBook.book = b;
            sortingBook.category = b.GetBookCategory();

            sortingBookList.Add(sortingBook);
        }
    }

    public void SetCartBooksData()
    {
        for (int i = 0; i < cartBooks.Length; i++)
        {
            if (i < sortingBookList.Count)
            {
                cartBooks[i].SetBooksInStack(sortingBookList[i].book, sortingBookList[i].category);
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
        foreach (SortingBook sB in sortingBookList)
        {
            categoryList.Add(sB.category);
        }
        categoryList.Add("Nothing Here / Dud");// make a list or array of possible DECOY coategory names

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
