using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bookshelf : MonoBehaviour
{

    [SerializeField] private string category;
    [SerializeField] private TextMeshProUGUI categoryText;
    public static Action<bool, string> OnSort;
    public string topicCategory;

    public void CompareCaterory(BookStack bookStack)
    {
        if (bookStack.GetBookCategory() == category)
        {
            if (OnSort != null)
                OnSort(true, bookStack.name);
        }
        else
        {
            if (OnSort != null)
                OnSort(false, bookStack.name);
        }
    }

    public void SetCategory(string categoryName)
    {
        category = categoryName;
        categoryText.text = category;
    }
}
