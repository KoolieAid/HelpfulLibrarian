using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackCover : MonoBehaviour
{
    public static StackCover instance;

    [System.Serializable]
    private struct bookCovers
    {
        public Image img;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI bodyText;
        public Image subjImage;
    }
    [SerializeField] private bookCovers bookCoversInStack;

    public bool isOpen;
    [SerializeField] private GameObject covers;

    private void Start()
    {
        instance = this;
        isOpen = false;
        CloseCovers();
    }

    public void SetCoverData(BookSet bookSet)
    {
        bookCoversInStack.img.sprite = bookSet.bookCover.bookBack;
        SetDescription(bookSet.bookInfo.title, bookSet.bookInfo.description, bookSet.bookInfo.keyword.image);
    }

    void SetDescription(string title, string description, Sprite image)
    {
        bookCoversInStack.titleText.text = title;
        bookCoversInStack.bodyText.text = description;
        bookCoversInStack.subjImage.sprite = image;
    }

    public void OpenCovers()
    {
        covers.SetActive(true);
        isOpen = true;
    }

    public void CloseCovers()
    {
        isOpen = false;
        covers.SetActive(false);
    }
}
