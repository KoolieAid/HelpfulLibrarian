using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject statusPanel;

    private void Start()
    {
        statusPanel.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ShowLevelStatus()
    {
        statusPanel.SetActive(true);
    }
}
