using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    private bool _isShowingDescription = false;
    [SerializeField] private TextMeshProUGUI textMeshTitle;
    [SerializeField] private Sprite[] referenceSprites;
    [SerializeField] private Image img;
    [SerializeField] private string title;
    [SerializeField] private string description;
    private int spriteIndex;

    private void Start()
    {
        spriteIndex = Random.Range(0, referenceSprites.Length);
        img.sprite = referenceSprites[spriteIndex];
        textMeshTitle.text = title;
        
        GetComponent<DoubleDetector>().onDoubleTap.AddListener(() =>
        {
            // Show confirmation
            Confirmattion.Instance.gameObject.SetActive(true);
        });
        
    }
    
    public void ShowDescription()
    {
        Cover.instance.gameObject.SetActive(true);
        Cover.instance.SetCoverSprite(spriteIndex);
        _isShowingDescription = true;
        Cover.instance.SetDescription(description);
    }

    public void HideDescription()
    {
        Cover.instance.gameObject.SetActive(false);
        _isShowingDescription = false;
        Cover.instance.SetDescription("");
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
}
