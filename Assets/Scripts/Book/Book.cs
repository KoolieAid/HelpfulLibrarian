using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    
    private bool isDragging = false;
    private Vector3 originalPosition;
    private bool onReader = false;

    public UnityEvent onBookSelected;
    public UnityEvent onBookPressed;

    private void Start()
    {
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex].bookFront;
        bookBackSprite = referenceSprites[spriteIndex].bookBack;
        textMeshTitle.text = title;

        originalPosition = transform.position;

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
            gameObject.transform.position = originalPosition;

            if (onReader)
            {
                onBookSelected.Invoke();
                Confirmattion.Instance.gameObject.transform.parent.gameObject.SetActive(true);
                
                Confirmattion.Instance.onConfirm.AddListener(() =>
                {
                    if (!ReaderManager.Instance) return; // Tutorial
                    ReaderManager.Instance.Compare(this);
                });
            }
        });
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<Reader>()) return;
        onReader = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<Reader>()) return;
        onReader = false;
    }

    public void ShowDescription()
    {
        if (isDragging) return;
        onBookPressed.Invoke();
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
