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
            SaveManager.Instance.OverwriteDiskFromData();
        };
    }

    public void OnReplayButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
