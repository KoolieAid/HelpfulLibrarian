using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{

    public TextMeshProUGUI score;
    public TextMeshProUGUI perfectScore;

    public GameObject resultsPopup;

    private void Start()
    {
        resultsPopup.SetActive(false);
    }

    void OnEnable()
    {
        SortingGameManager.OnSortAll += ShowResults;
    }
    void OnDisable()
    {
        SortingGameManager.OnSortAll -= ShowResults;
    }

    void ShowResults(int value1, int value2)
    {
        perfectScore.text = value1.ToString();
        score.text = value2.ToString();
        resultsPopup.SetActive(true);
    }
}
