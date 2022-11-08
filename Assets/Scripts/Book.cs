using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Book : MonoBehaviour
{
    private bool _isShowingDescription = false;
    private GameObject cover;
    [SerializeField] private TextMeshProUGUI textMesh;

    public void ShowDescription()
    {
        cover.SetActive(true);
        _isShowingDescription = true;
    }

    public void HideDescription()
    {
        cover.SetActive(false);
        _isShowingDescription = false;
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

    public void SetDescription(string desc)
    {
        textMesh.text = desc;
    }
}
