using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
    public struct BookSet
    {
        public BookInfo bookInfo;
        public BookCover bookCover;
    }
public class BookStack : MonoBehaviour
{
    
    [SerializeField] private BookSet bookSet;
    private string category;

    private Vector2 originalPos;
    public int speed = 300;

    private Collider2D bookCollider;

    private int numOfChances = 3;
    private int numOfTries;

    public Image bookSprite;
    public bool interactable = true;
    private bool isDragging = false;
    private bool onBookshelf = false;
    private Bookshelf bookshelf;

    public static Action<bool, string> OnFailBook;

    void OnEnable()
    {
        Bookshelf.OnSort += SortCheck;
    }
    void OnDisable()
    {
        Bookshelf.OnSort -= SortCheck;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        bookCollider = GetComponent<Collider2D>();


        var dragComp = GetComponent<Draggable>();

        dragComp.onDrag = data =>
        {
            gameObject.transform.position = data.position;
        };

        dragComp.onBeginDrag.AddListener(() =>
        {
            isDragging = true;
            transform.SetAsLastSibling();
        });
        
        dragComp.onEndDrag.AddListener(() =>
        {
            isDragging = false;
            if (onBookshelf)
            {
                bookshelf.CompareCaterory(this);
            }

            if (gameObject.activeInHierarchy)
                StartCoroutine("ReturnToStartPos");
        });
    }

    void SortCheck(bool status, string name)
    {
        if (name != gameObject.name)
        {
            return;
        }
        
        if (status) // Sorted Correctly
        {
            StopCoroutine("ReturnToStartPos");
            gameObject.SetActive(false);
        }
        else if (!status)  // Sorted Incorrectly
        {
            TryCounter();
            StartCoroutine("ReturnToStartPos");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bookshelf b = collision.gameObject.GetComponent<Bookshelf>();
        if (b) 
        {
            onBookshelf = true;
            bookshelf = b;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bookshelf>()) 
        {
            onBookshelf = false;
            bookshelf = null;
        }
    }

    public void ShowDescription()
    {
        if (isDragging) return;
        
        StackCover.instance.SetCoverData(bookSet);
        StackCover.instance.OpenCovers();
    }

    void TryCounter()
    {
        numOfTries += 1;

        if (numOfTries > numOfChances)
        {
            SetColliderStatus(false); // No longer able to interact with this stack of books
            interactable = false;
            bookSprite.color = Color.grey;
            if (OnFailBook != null)
                OnFailBook(false, "failed");
        }
    }
    // Called at the start of this part of the game
    public void SetBooksInStack(BookInfo book, string topic)
    {
        bookSet.bookInfo = book;
        category = topic;
    }

    public void SetColliderStatus(bool isActive)
    {
        bookCollider.enabled = isActive;
    }

    IEnumerator ReturnToStartPos()
    {
        while (Vector2.Distance(transform.position, originalPos) > 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetBookCategory(string categoryName)
    {
        category = categoryName;
    }

    public string GetBookCategory()
    {
        return category;
    }
}
