using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AYellowpaper.SerializedCollections;

public class LevelManager : MonoBehaviour
{
    public List<Level> levelDataList; // Store Level Data for all levels
    public int selectedLevel = 0; // #0 is the tutorial level
    
    public SerializedDictionary<int,Memory_Game.Level>minigameLevelDataDictionary = new SerializedDictionary<int, Memory_Game.Level>();
    
    public int selectedMinigame = 0;

    public List<int> levelsUnlocked = new List<int>();


    // On level selected
    public void LoadLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
        SceneManager.LoadScene("Main Level");
    }

    public void LoadMinigame(int levelNum)
    {
        selectedMinigame  = levelNum;
        SceneManager.LoadSceneAsync("SortingMiniGame").completed += _ =>
        {
            SceneManager.LoadSceneAsync("Memory Game", LoadSceneMode.Additive).completed += _ =>
            {
                SortingGameManager.Instance.canvas.SetActive(false);
            };
        };
    }

    public Memory_Game.Level GetMinigameData()
    {
        return minigameLevelDataDictionary[selectedMinigame];
    }

    // level scene retrieves assigned level data for the level
    public Level GetLevelData()
    {
        return levelDataList[selectedLevel];
    }
    
}
