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
            Confirmattion.Instance.gameObject.SetActive(true);
            
            Confirmattion.Instance.onConfirm.AddListener(() =>
            {
                ReaderManager.Instance.Compare(this);
            });
        });

        
    }
    
    public void ShowDescription()
    {
        Cover.instance.gameObject.SetActive(true);
        Cover.instance.SetCoverSprite(bookBackSprite);
        _isShowingDescription = true;
        Cover.instance.SetDescription(word, wordTranslation, description);
    }

    public void HideDescription()
    {
        Cover.instance.gameObject.SetActive(false);
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
    }

}
