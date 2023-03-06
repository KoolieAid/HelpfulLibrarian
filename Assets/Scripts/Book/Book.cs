using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Book : MonoBehaviour
{
    private bool _isShowingDescription = false;
    [SerializeField] private TextMeshProUGUI textMeshTitle;
    [SerializeField] private BookCover[] referenceSprites;
    [SerializeField] private Image img;
    [SerializeField] private string title;
    [SerializeField] private string word;
    [SerializeField] private string wordTranslation;
    [Multiline(5)]
    [SerializeField] private string description;
    private int spriteIndex;
    private Sprite bookBackSprite;

    private void Start()
    {
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex].bookFront;
        bookBackSprite = referenceSprites[spriteIndex].bookBack;
        textMeshTitle.text = title;
        
        GetComponent<DoubleDetector>().onDoubleTap.AddListener(() =>
        {
            // Show confirmation
            Confirmattion.Instance.gameObject.transform.parent.gameObject.SetActive(true);
            
            Confirmattion.Instance.onConfirm.AddListener(() =>
            {
                if (!ReaderManager.Instance) return; // Tutorial
                ReaderManager.Instance.Compare(this);
            });
        });

        
    }
    
    public void ShowDescription()
    {
        // Cover.instance.transform.parent.gameObject.SetActive(true);
        Cover.instance.OpenCover();
        Cover.instance.SetCoverSprite(bookBackSprite);
        _isShowingDescription = true;
        Cover.instance.SetDescription(word, wordTranslation, description);
    }

    public void HideDescription()
    {
        // Cover.instance.transform.parent.gameObject.SetActive(false);
        Cover.instance.CloseCover();
        _isShowingDescription = false;
        Cover.instance.SetDescription("", "", "");
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

    public void SetBookInfo(ReaderManager.BookInfo info)
    {
        title = info.title;
        textMeshTitle.text = title;
        description = info.description;

        if (!info.keyword)
        {
            Debug.LogWarning($"Keyword is null for {info.title}");
            return;
        }
        word = info.keyword.wordVersion;
        wordTranslation = info.keyword.wordTranslation;
    }

}
