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
    [SerializeField] private bookCovers[] bookCoversInStack = new bookCovers[2];

    public bool isOpen;
    [SerializeField] private GameObject covers;

    private void Start()
    {
        instance = this;
        isOpen = false;
        CloseCovers();
    }

    public void SetCoverData(BookSet bookSet1, BookSet bookSet2)
    {
        bookCoversInStack[0].img.sprite = bookSet1.bookCover.bookBack;
        SetDescription(0, bookSet1.bookInfo.title, bookSet1.bookInfo.description, bookSet1.bookInfo.keyword.image);

        bookCoversInStack[1].img.sprite = bookSet2.bookCover.bookBack;
        SetDescription(1, bookSet2.bookInfo.title, bookSet2.bookInfo.description, bookSet2.bookInfo.keyword.image);
    }

    void SetDescription(int index, string title, string description, Sprite image)
    {
        bookCoversInStack[index].titleText.text = title;
        bookCoversInStack[index].bodyText.text = description;
        bookCoversInStack[index].subjImage.sprite = image;
    }

    public void OpenCovers()
    {
        covers.SetActive(true);
        //covers.transform.position = new Vector3(0, 0, 0);
        isOpen = true;
    }

    public void CloseCovers()
    {
        isOpen = false;
        covers.SetActive(false);
        //covers.transform.position = new Vector3(0, 2000, 0);
    }
}
