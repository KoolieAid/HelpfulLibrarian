using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject creditPanel;
    public GameObject gamemodePanel;
    public GameObject levelSelectPanel;

    public Button levelSelectButton;

    [Header("Level Buttons")]
    public Button[] levelButtons;


    [Header("Level Stars")]
    public StarManager[] starManagers = new StarManager[9];
    private Dictionary<int, int> recordOfStars = new Dictionary<int, int>()
    {
        {1, 0},
        {2, 0},
        {3, 0},
        {4, 0},
        {5, 0},
        {6, 0},
        {7, 0},
        {8, 0},
        {9, 0}
    };
    private void Start()
    {
        if (GameManager.instance.tutorialIsDone)
        {
            levelSelectButton.interactable = true;
        }
    }

    public void PlayButtonClicked()
    {
        ActivatePanel(gamemodePanel.name);
        
        UnlockLevelButtons();
    }
    public void CreditButtonClicked()
    {
        ActivatePanel(creditPanel.name);
    }
    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void LevelSelectButtonClicked()
    {
        ActivatePanel(levelSelectPanel.name);
        ShowStars();
    }

    void ShowStars()
    {
        for (int i = 0; i < starManagers.Length; i++)
        {
            int value;
            recordOfStars.TryGetValue(i + 1, out value);
            starManagers[i].SetStarsToActive(value);
        }
    }

    public void SetLevelStarScore(int levelNum, int starScore)
    {
        recordOfStars[levelNum] = starScore;
    }
    public int GetLevelStarScore(int levelNum)
    {
        int i;
        recordOfStars.TryGetValue(levelNum, out i);
        return i;
    }

    public void TutorialButtonClicked()
    {
        // Go to Tutorial Scene
        SceneManager.LoadScene("Tutorial");
    }

    public void BackButtonClicked(string previousPanelName)
    {
        ActivatePanel(previousPanelName);
    }

    void ActivatePanel(string panelToActivate)
    {
        mainMenuPanel.SetActive(mainMenuPanel.name.Equals(panelToActivate));
        creditPanel.SetActive(creditPanel.name.Equals(panelToActivate));
        gamemodePanel.SetActive(gamemodePanel.name.Equals(panelToActivate));
        levelSelectPanel.SetActive(levelSelectPanel.name.Equals(panelToActivate));
    }

    public void SelectLevel(int levelNumber)
    {
        GameManager.instance.levelManager.LoadLevel(levelNumber);
    }

    private void UnlockLevelButtons()
    {
        // unlock each levels
        for (int i = 0; i < GameManager.instance.levelManager.levelsUnlocked.Count + 1; i++)
        {
            if (!levelButtons[i].interactable)
            {
                levelButtons[i].interactable = true;
                
                Debug.Log($"Unlocked button {levelButtons[i]}");
            }
        }
    }
}
