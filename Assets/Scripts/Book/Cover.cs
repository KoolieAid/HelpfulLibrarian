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
    [SerializeField] private Sprite[] referenceSprites;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Start()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    public void SetCoverSprite(int index)
    {
        img.sprite = referenceSprites[index];
    }

    public void SetDescription(string desc)
    {
        textMesh.text = desc;
    }

    public void CloseCover()
    {
        gameObject.SetActive(false);
    }
}
