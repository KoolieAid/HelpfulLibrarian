using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<Level> levelDataList; // Store Level Data for all levels
    public int selectedLevel = 0; // #0 is the tutorial level
    
    public Dictionary<int,Memory_Game.Level> minigameLevelDataDictionary = new Dictionary<int, Memory_Game.Level>(); // Store Level Data for all Minigame levels
    public List<int> minigameLevels = new List<int>();
    public List<Memory_Game.Level>minigameLevelData = new List<Memory_Game.Level>();
    public int selectedMinigame = 0;

    public List<int> levelsUnlocked = new List<int>();

    private void Start()
    {
        for (int i = 0; i < minigameLevels.Count; i++)
        {
            minigameLevelDataDictionary.Add(minigameLevels[i], minigameLevelData[i]);
        }
    }

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
        Memory_Game.Level data;
        minigameLevelDataDictionary.TryGetValue(selectedMinigame, out data);
        return data;
    }

    // level scene retrieves assigned level data for the level
    public Level GetLevelData()
    {
        return levelDataList[selectedLevel];
    }
    
}
