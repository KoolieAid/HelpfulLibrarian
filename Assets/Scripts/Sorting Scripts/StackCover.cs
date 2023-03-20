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

    private void Start()
    {
        instance = this;
        isOpen = false;
        gameObject.SetActive(false);
    }

    public void SetCoverSprites(Sprite renderSprite1, Sprite renderSprite2)
    {
        bookCoversInStack[0].img.sprite = renderSprite1;
        bookCoversInStack[1].img.sprite = renderSprite2;
    }

    public void SetDescriptions(string title1, string title2, string desc1, string desc2, Sprite image1, Sprite image2)
    {
        bookCoversInStack[0].titleText.text = title1;
        bookCoversInStack[0].bodyText.text = desc1;
        bookCoversInStack[0].subjImage.sprite = image1;

        bookCoversInStack[1].titleText.text = title2;
        bookCoversInStack[1].bodyText.text = desc2;
        bookCoversInStack[1].subjImage.sprite = image2;
    }

    public void OpenCovers()
    {
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseCovers()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
