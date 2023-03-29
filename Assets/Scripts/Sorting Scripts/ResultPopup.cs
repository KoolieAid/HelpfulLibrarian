using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI perfectScore;

    [SerializeField] private GameObject resultsPopup;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        resultsPopup.SetActive(false);
    }

    void OnEnable()
    {
        SortingGameManager.OnGameEnd += ShowResults;
    }
    void OnDisable()
    {
        SortingGameManager.OnGameEnd -= ShowResults;
    }

    void ShowResults(int value1, int value2)
    {
        perfectScore.text = value1.ToString();
        score.text = value2.ToString();
        resultsPopup.SetActive(true);

        if (value2 >= (value1 / 2f))
        {
            mainMenuButton.interactable = true;
        }
    }
}
