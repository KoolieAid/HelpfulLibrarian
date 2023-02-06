using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameBackButton : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
