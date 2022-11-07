using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject creditPanel;
    public GameObject gamemodePanel;
    public GameObject levelSelectPanel;


    public void PlayButtonClicked()
    {
        ActivatePanel(gamemodePanel.name);
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
    }
    public void TutorialButtonClicked()
    {
        // Go to Tutorial Scene
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
}
