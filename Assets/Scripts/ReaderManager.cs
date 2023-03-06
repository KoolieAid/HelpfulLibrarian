using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ReaderManager : MonoBehaviour
{
    public static ReaderManager Instance;

    [Header("TESTING")] [SerializeField] private Level testLevel;
    
    [Header("Active Info")]
    public Reader currentReader;
    [Min(1)] [SerializeField]
    private int stars = 3;
    private List<BookInfo> currentCorrectAnswers;

    [Header("Book Animation")]
    [SerializeField] private float speed;

    [Header("Spawning")]
    [SerializeField] private Vector3 spawnCoords;
    [SerializeField] private GameObject ReaderPrefab;
    [SerializeField] private Transform CanvasParent;

    [Header("Events")]
    [Tooltip("Fires when the player wins.")]
    [SerializeField] private UnityEvent onPlayerWin;
    [Tooltip("Fires when the player loses")]
    [SerializeField] private UnityEvent onPlayerLose;

    [Header("Stars UI")] 
    [SerializeField] private GameObject[] starsUI;

    [Header("Data")]
    [SerializeField] private Level levelData; 
    [Tooltip("Chance of getting an answer book with the wrongs ones")]
    [Range(0, 100)]
    [SerializeField] private float chanceOfCorrect;
    
    [SerializeField] private Sprite[] readerSprites;
    private void Awake()
    {
        Instance = this;
        
        // try catch for accessible testing 
        try
        {
            levelData = GameManager.instance.levelManager.GetLevelData();
        }
        catch
        {
            // If GameManager is unavailable, go to this. Means that the scene was not loaded from the main menu
            levelData = testLevel;
        }
        
        if (levelData.GetCorrectAnswers().Count < 1) Debug.LogWarning($"No possible books detected. Please resolve this.");
        currentCorrectAnswers = new List<BookInfo>(levelData.GetCorrectAnswers());
        
        

        if (currentReader != null) return; // For tutorial sequence
        NextReader();
    }

    public bool Compare(Book book)
    {
        // Debug.Log($"Request: {currentReader.requestedTitle}. Book title: {book.GetTitle()}. Evaluation: {currentReader.requestedTitle == book.GetTitle()}");
        // If title does not match
        if (currentReader.requestedTitle != book.GetTitle())
        {
            //DeductStars();
            // Deduct Timer
            currentReader.DeductPatience();

            currentReader.TriggerWrongBookAnimation();

            ParticleManager.Instance.PlayParticle("X");
            ParticleManager.Instance.PlayParticle("Smoke");

            return false;
        }

        // Correct?? Next reader pls
        Debug.Log("CORRECT, going to next reader");
        ParticleManager.Instance.PlayParticle("Heart");
        ParticleManager.Instance.PlayParticle("Star");

        // add book animation
        StartCoroutine(GiveBookAnimation(book));
        
        //NextReader();
        return true;
    }

    private void DeductStars()
    {
        stars--;
        // Update the UI here.
        starsUI[stars].gameObject.SetActive(false);
        
        if (stars <= 0)
        {
            onPlayerLose.Invoke();
            if (currentReader.gameObject) Destroy(currentReader.gameObject);
        }
    }
    
    private void NextReader()
    {
        // If theres no possible reader next
        if (currentCorrectAnswers.Count < 1 )
        {
            Debug.Log("Player won the level");
            onPlayerWin.Invoke();
            if (currentReader.gameObject) Destroy(currentReader.gameObject);
            return;
        }
        
        if (stars <= 0) return;
        
        ReplaceReader();
    }

    /// <remarks>
    /// This is simply a helper function, used for reusing, and for shortening code. NOT for methods.
    /// </remarks>
    private void ReplaceReader()
    {
        var bookinfo = currentCorrectAnswers[Random.Range(0, currentCorrectAnswers.Count)];
        currentCorrectAnswers.Remove(bookinfo);
        
        GenerateBookGrid(bookinfo);
        
        // Instantiate a new reader with the book info
        var reader = GenerateReader();
        reader.requestedTitle = bookinfo.title;
        reader.SetRequestImage(bookinfo.keyword.image);
        
        // TODO: Add the faces. Problem is: idk how to do animations, there are 3 sprites
        // reader.face.sprite = readerSprites[Random.Range(0, readerSprites.Length)];
        reader.gameObject.SetActive(true);
    }

    /// <summary>
    /// Instantiates a new <see cref="Reader"/>, and performs some checks in the process
    /// </summary>
    /// <returns>The newly instantiated reader</returns>
    private Reader GenerateReader()
    {
        if (currentReader != null) Destroy(currentReader.gameObject);
        currentReader = null;
        var reader = Instantiate(ReaderPrefab, spawnCoords, Quaternion.identity, CanvasParent).GetComponent<Reader>();
        reader.GetComponent<RectTransform>().anchoredPosition = spawnCoords;
        reader.gameObject.transform.SetSiblingIndex(1); // 0 = Background; 1 = Object after background
        currentReader = reader;
        
        reader.onPatienceGone.AddListener(DeductStars);
        reader.onPatienceGone.AddListener(NextReader);

        return reader;
    }

    /// <summary>
    /// Regenerates the <see cref="Book"/> grid, includes the new book info
    /// </summary>
    /// <param name="correctBookInfo">Info that the supposedly correct book needs</param>
    /// <returns>The correct <see cref="Book"/></returns>
    private Book GenerateBookGrid(BookInfo correctBookInfo)
    {
        // Would be possible to have this become a member instead of asking for components
        var books = GetComponentsInChildren<Book>(true);

        var correctBook = books[Random.Range(0, books.Length)];
        correctBook.SetBookInfo(correctBookInfo);

        var copyOfWrongAns = new List<BookInfo>(levelData.GetWrongAnswers());
        // For the chances
        var copyOfCorrectAns = new List<BookInfo>(levelData.GetCorrectAnswers());
        foreach (var book in books)
        {
            if (book.Equals(correctBook)) continue;
            
            if (Random.Range(0.0f, 100f) <= chanceOfCorrect)
            {
                BookInfo inf;
                do
                {
                    inf = copyOfCorrectAns[Random.Range(0, copyOfCorrectAns.Count)];
                // } while (inf.title.Equals(correctBookInfo.title));
                } while (inf == correctBookInfo);

                copyOfCorrectAns.Remove(inf);
                book.SetBookInfo(inf);
                continue;
            }
            
            var newInfo = copyOfWrongAns[Random.Range(0, copyOfWrongAns.Count)];
            copyOfWrongAns.Remove(newInfo);
            book.SetBookInfo(newInfo);
        }

        return correctBook;
    }
    
    IEnumerator GiveBookAnimation(Book _book)
    {
        var bookPos = _book.GetComponent<RectTransform>();
        var origBookPos = bookPos.anchoredPosition;
        var readerPos = currentReader.GetComponent<RectTransform>();
        var final = new Vector2(-250, 170);
        
        Reader.Instance.canDeduct = false;
        
        while (Vector2.Distance(bookPos.anchoredPosition, final) > 1f)
        {
            bookPos.anchoredPosition = Vector3.MoveTowards(bookPos.anchoredPosition,
                final, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        bookPos.anchoredPosition = origBookPos;
        
        Reader.Instance.canDeduct = true;
        NextReader();
        Debug.Log("Book Given");
    }

    [Serializable]
    public struct BookInfo
    {
        public string title;
        public Keyword keyword;
    
        [TextArea(3, 4)]
        public string description;

        public static bool operator==(BookInfo a, BookInfo b)
        {
            if (Equals(a, b)) return true;

            if (a.title.Equals(b.title) && a.keyword.Equals(b.keyword)) return true;

            return false;
        }

        public static bool operator !=(BookInfo a, BookInfo b)
        {
            if (Equals(a, b)) return false;

            if (a.title.Equals(b.title) && a.keyword.Equals(b.keyword)) return false;

            return true;
        }
    }
    
}
