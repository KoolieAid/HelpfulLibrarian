using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SortingBook
{
    public BookInfo book;
    public string category;
}
public class SortingGameManager : MonoBehaviour
{
    public static SortingGameManager Instance;
    
    [SerializeField] private List<SortingBook> sortingBookList = new List<SortingBook>();
    [SerializeField] private BookStack[] cartBooks;
    [SerializeField] private Bookshelf[] bookShelves = new Bookshelf[7];
    private List<string> categoryList = new List<string>();
    [SerializeField] private List<string> decoyCategories = new List<string>();

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

    public GameObject canvas;

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

    private void Awake()
    {
        Instance = this;
        if (GameManager.instance != null)
        {
            Debug.Log("Thorough debug mode");
            canvas.SetActive(false);
        }
    }


    public void ManualStart(List<BookInfo> books)
    {
        SetSortingBookList(books);
        SetCartBooksData();
        GetAllCategories();
        StartCoroutine(nameof(StartTimer));
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
        
        for (int i = 0;categoryList.Count <= cartBooks.Length; i++)
        {
            int n = UnityEngine.Random.Range(0, decoyCategories.Count);
            categoryList.Add(decoyCategories[n]);
            decoyCategories.RemoveAt(n);
        }
        
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
        
        l.levelsUnlocked.Add(l.selectedLevel + 1);
    }

    public void RestartGame()
    {
        
        SceneManager.LoadSceneAsync("SortingMiniGame").completed += _ =>
        {
            var temp = sortingBookList;
            Instance.canvas.SetActive(true);
            Instance.sortingBookList = temp;
            Instance.SetCartBooksData();
            Instance.GetAllCategories();
            Instance.StartCoroutine(nameof(StartTimer));
        };
    }
}
