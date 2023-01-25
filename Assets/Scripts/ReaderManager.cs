using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ReaderManager : MonoBehaviour
{
    public static ReaderManager Instance;
    
    public Reader currentReader;
    [Min(1)] [SerializeField]
    private int stars = 3;

    public List<BookInfo> possibleBooks = new();
    [SerializeField] private Vector3 spawnCoords;
    [SerializeField] private GameObject ReaderPrefab;
    [SerializeField] private Transform CanvasParent;

    [Tooltip("Fires when the player wins.")]
    [SerializeField] private UnityEvent onPlayerWin;
    [Tooltip("Fires when the player loses")]
    [SerializeField] private UnityEvent onPlayerLose;
    private void Awake()
    {
        Instance = this;
        if (possibleBooks.Count < 1) Debug.LogWarning($"No possible books detected. Please resolve this.");
        if (currentReader != null) return; // For tutorial sequence
        NextReader();
    }

    public bool Compare(Book book)
    {
        // Debug.Log($"Request: {currentReader.requestedTitle}. Book title: {book.GetTitle()}. Evaluation: {currentReader.requestedTitle == book.GetTitle()}");
        // If title does not match
        if (currentReader.requestedTitle != book.GetTitle())
        {
            stars--;
            // Update the UI here.
            
            if (stars <= 0)
            {
                Debug.Log($"Player lost. Request: {currentReader.requestedTitle}. Book Selected: {book.GetTitle()}");
                onPlayerLose.Invoke();
                // Restart Scene?? idk
            }

            return false;
        }

        // Correct?? Next reader pls
        Debug.Log("CORRECT, going to next reader");
        NextReader();
        return true;
    }
    
    private void NextReader()
    {
        // If theres no possible reader next
        if (possibleBooks.Count < 1)
        {
            Debug.Log("Player won the level");
            onPlayerWin.Invoke();
            //SceneManager.LoadScene(gameObject.scene.name);
            return;
        }
        
        ReplaceReader();
    }

    /// <remarks>
    /// This is simply a helper function, used for reusing, and for shortening code. NOT for methods.
    /// </remarks>
    private void ReplaceReader()
    {
        var bookinfo = possibleBooks[Random.Range(0, possibleBooks.Count)];
        possibleBooks.Remove(bookinfo);

        GenerateBookGrid(bookinfo);
            
        // Instantiate a new reader with the book info
        var reader = GenerateReader();
        reader.requestedTitle = bookinfo.title;
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
        return reader;
    }

    /// <summary>
    /// Regenerates the <see cref="Book"/> grid, includes the new book info
    /// </summary>
    /// <param name="correctBookInfo">Info that the supposedly correct book needs</param>
    /// <returns>The targeted <see cref="Book"/></returns>
    private Book GenerateBookGrid(BookInfo correctBookInfo)
    {
        // Would be possible to have this become a member instead of asking for components
        var books = GetComponentsInChildren<Book>(true);

        var correctBook = books[Random.Range(0, books.Length)];
        correctBook.SetBookInfo(correctBookInfo);
        
        foreach (var book in books)
        {
            if (book.Equals(correctBook)) continue;
            
            // TODO: Mask the wrong choices instead of making them obvious
            book.SetBookInfo(new BookInfo()
            {
                title = "bad",
                description = "also bad lol"
            });
        }

        return correctBook;
    }

    [Serializable]
    public struct BookInfo
    {
        public string title;
        [Multiline(5)]
        public string description;
    }
    
}
