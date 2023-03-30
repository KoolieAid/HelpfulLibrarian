using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Cover : MonoBehaviour
{
    public static Cover instance;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI titleTranslationText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Image subjImage;

    public bool isOpen = false;

    private void Start()
    {
        instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    public void SetCoverSprite(Sprite renderSprite)
    {
        img.sprite = renderSprite;
    }

    public void SetDescription(string word, string wordTrans, string desc, Sprite image)
    {
        titleText.text = word;
        titleTranslationText.text = "(" + wordTrans + ")";
        bodyText.text = desc;
        subjImage.sprite = image;
    }

    public void OpenCover()
    {
        transform.parent.gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseCover()
    {
        isOpen = false;
        transform.parent.gameObject.SetActive(false);
    }
}
