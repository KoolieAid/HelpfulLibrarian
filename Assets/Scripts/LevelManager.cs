using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<Level> levelDataList; // Store Level Data for all levels
    public int selectedLevel = 0; // #0 is the tutorial level

    public List<int> levelsUnlocked = new List<int>();

    // On level selected
    public void LoadLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
        SceneManager.LoadScene("Main Level");

    }

    // level scene retrieves assigned level data for the level
    public Level GetLevelData()
    {
        return levelDataList[selectedLevel];
    }
    
}
