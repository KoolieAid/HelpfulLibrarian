using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Book : MonoBehaviour
{
    private bool _isShowingDescription = false;
    [SerializeField] private TextMeshProUGUI textMeshTitle;
    [SerializeField] private BookCover[] referenceSprites;
    [SerializeField] private Image img;
    [SerializeField] private string title;
    [FormerlySerializedAs("word")] [SerializeField] private string coverTitle;
    [SerializeField] private string wordTranslation;
    [SerializeField] private Sprite wordImage;
    [Multiline(5)]
    [SerializeField] private string description;
    private int spriteIndex;
    private Sprite bookBackSprite;

    private enum Location
    {
        OnCounter,
        OffCounter,
    };

    private Location locationOnScreen;
    [SerializeField] private Collider2D bookCollider;
    [SerializeField] private int speed = 300;
    [SerializeField] private Vector2 originalPos;

    private void Start()
    {
        originalPos = transform.position;
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex].bookFront;
        bookBackSprite = referenceSprites[spriteIndex].bookBack;
        textMeshTitle.text = title;

        /*GetComponent<DoubleDetector>().onDoubleTap.AddListener(() =>
        {
            // Show confirmation
            Confirmattion.Instance.gameObject.transform.parent.gameObject.SetActive(true);
            
            Confirmattion.Instance.onConfirm.AddListener(() =>
            {
                if (!ReaderManager.Instance) return; // Tutorial
                ReaderManager.Instance.Compare(this);
            });
        });
*/
        
    }

    public void BookClicked()
    {
        if (locationOnScreen == Location.OnCounter)
        {
            ShowDescription();
        }

        StartCoroutine("ReturnToStartPos");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TableCounter") locationOnScreen = Location.OnCounter;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TableCounter") locationOnScreen = Location.OffCounter;
    }

    IEnumerator ReturnToStartPos()
    {
        bookCollider.enabled = false;
        while (Vector2.Distance(transform.position, originalPos) > 1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        bookCollider.enabled = true;
    }
    
    public void ShowDescription()
    {
        // Cover.instance.transform.parent.gameObject.SetActive(true);
        Cover.instance.OpenCover();
        Cover.instance.SetCoverSprite(bookBackSprite);
        _isShowingDescription = true;
        Cover.instance.SetDescription(coverTitle, wordTranslation, description, wordImage);
    }

    public void HideDescription()
    {
        // Cover.instance.transform.parent.gameObject.SetActive(false);
        Cover.instance.CloseCover();
        _isShowingDescription = false;
        Cover.instance.SetDescription("", "", "", null);
    }

    public void ToggleDescription()
    {
        if (_isShowingDescription)
        {
            HideDescription();
            return;
        }
        ShowDescription();
    }

    public void ShowConfirmation()
    {
        
    }

    public string GetTitle()
    {
        return title;
    }

    public void SetBookInfo(BookInfo info)
    {
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex].bookFront;
        
        title = info.title;
        textMeshTitle.text = title;
        description = info.description;

        if (!info.keyword)
        {
            Debug.LogWarning($"Keyword is null for {info.title}");
            return;
        }

        coverTitle = info.title;
        wordImage = info.keyword.image;
    }

}
