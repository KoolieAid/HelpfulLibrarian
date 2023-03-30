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

    private Vector3 originalPos;
    public int speed = 300;

    private Collider2D bookCollider;

    private int numOfChances = 3;
    private int numOfTries;

    public enum Location
    {
        OnCart,
        OffCart,
    };
    public Location bookLocation;

    private enum SortStatus
    {
        Unsorted,
        Sorted,
    };
    private SortStatus bookStatus;

    public Image bookSprite;
    public bool interactable = true;
    private bool isOverBookshelf = false;
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
        bookStatus = SortStatus.Unsorted;
        bookLocation = Location.OnCart;
    }

    void SortCheck(bool status, string name)
    {
        if (name == gameObject.name)
        {
            if (status) // Sorted Correctly
            {
                bookStatus = SortStatus.Sorted;
                gameObject.SetActive(false);
                StopCoroutine("ReturnToStartPos");
            }
            else if (!status)  // Sorted Incorrectly
            {
                bookStatus = SortStatus.Unsorted;
                TryCounter();
                StartCoroutine("ReturnToStartPos");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name  = collision.gameObject.name;
        if (name == "Cart") 
        {
            bookLocation = Location.OnCart;
        }
        if (name.Contains("Shelf"))
        {
            isOverBookshelf = true;
            bookshelf = collision.gameObject.GetComponent<Bookshelf>();
        } 
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        string name  = collision.gameObject.name;
        if (name == "Cart")
        {
            bookLocation = Location.OffCart;
        }
        if (name.Contains("Shelf")) 
        {
            isOverBookshelf = false;
            bookshelf = null;
        } 
    }

    public void BookClicked()
    {

        if (bookLocation == Location.OffCart && isOverBookshelf)
        {
            bookshelf.CompareCaterory(this);
        }
        else if (bookLocation == Location.OnCart)
        {
            StackCover.instance.SetCoverData(bookSet);
            StackCover.instance.OpenCovers();
        }


        if (bookStatus == SortStatus.Unsorted)
        {
            StartCoroutine("ReturnToStartPos");
        }

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
        SetColliderStatus(false);
        while (Vector2.Distance(transform.position, originalPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        SetColliderStatus(true);
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
