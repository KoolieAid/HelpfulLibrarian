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

    private bool isOverRequester = false;

    private void Start()
    {
        originalPos = transform.position;
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex].bookFront;
        bookBackSprite = referenceSprites[spriteIndex].bookBack;
        textMeshTitle.text = title;     
    }

    public void SetToOriginalPosition()
    {
        transform.position = originalPos;
    }

    public void BookClicked()
    {
        if (locationOnScreen == Location.OnCounter)
        {
            ShowDescription();
            StartCoroutine("ReturnToStartPos");
        }

        if (locationOnScreen == Location.OffCounter && isOverRequester)
        {
            if (!ReaderManager.Instance) return; // Tutorial

            ReaderManager.Instance.Compare(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        if (name == "TableCounter") locationOnScreen = Location.OnCounter;
        if (name.Contains("ReaderPrefab")) isOverRequester = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        if (name == "TableCounter") locationOnScreen = Location.OffCounter;
        if (name.Contains("ReaderPrefab")) isOverRequester = false;
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
