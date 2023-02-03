using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReplayButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void SetTutorialToDone()
    {
        GameManager.instance.tutorialIsDone = true;
    }
}
