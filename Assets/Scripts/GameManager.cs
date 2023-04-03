using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool tutorialIsDone = false;

    public LevelManager levelManager;

    private Dictionary<int, int> levelScore = new Dictionary<int, int>()
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

    [SerializeField] private int debugCount;
    
    public Dictionary<int, int> GetRecordCopy()
    {
        return new Dictionary<int, int>(levelScore);
    }

    public void SetNewRecord(Dictionary<int, int> newDict)
    {
        levelScore = newDict;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            MainMenuScript menuScript = GameObject.Find("~Main Menu Mgr").GetComponent<MainMenuScript>();

            foreach (var s in levelScore)
            {
                int v;
                levelScore.TryGetValue(s.Key, out v);
                if (v > menuScript.GetLevelStarScore(s.Key))
                    menuScript.SetLevelStarScore(s.Key, v);
            }
        }
    }

    public void SetScores(int levelNum, int numOfStars)
    {
        if (!levelScore.ContainsKey(levelNum))
        {
            levelScore.TryAdd(levelNum, numOfStars);
            return;
        }
        levelScore[levelNum] = numOfStars;
    }

    public void TutorialFinished()
    {
        tutorialIsDone = true;
    }

    public void SecretDebugging()
    {
        if (debugCount != 5)
        {
            debugCount++; 
            return;
        }

        if (debugCount == 5)
        {
            tutorialIsDone = true;

            // good luck future me to explain this shit that i came up in 3 mins, no docs  
        
            var total = levelManager.levelDataList.Count;

            var b4 = levelManager.levelsUnlocked.Count + 1;

            for (var i = b4; i < total; i++)
            {
                levelManager.levelsUnlocked.Add(i + 1);
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}
