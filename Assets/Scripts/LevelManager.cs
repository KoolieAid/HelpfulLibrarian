using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<Level> levelDataList; // Store Level Data for all levels
    int selectedLevel = 0; // #0 is the turorial level

    // On level selected
    public void LoadLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
        SceneManager.LoadScene("Main Level");
    }

    // level scene retrives assigned level data for the level
    public Level GetLevelData()
    {
        return levelDataList[selectedLevel];
    }
}
