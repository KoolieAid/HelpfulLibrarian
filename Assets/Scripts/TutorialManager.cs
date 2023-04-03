using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadSceneAsync("MainMenu").completed += _ =>
        {
            SaveManager.Instance.SaveFromCurrent();
        };
    }

    public void OnReplayButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
