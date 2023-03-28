using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool tutorialIsDone;

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

    void Start()
    {
        tutorialIsDone = false;
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
        else if (scene.name == "SortingMiniGame")
        {

        }
    }

    public void SetScores(int levelNum, int numOfStars)
    {
        levelScore[levelNum] = numOfStars;
    }


}
