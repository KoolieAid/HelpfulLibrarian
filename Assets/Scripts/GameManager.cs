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
        
        StartCoroutine(SetLevelManager());
    }

    IEnumerator SetLevelManager()
    {
        for (;;)
        {
            if (levelManager == null)
            {
                levelManager = gameObject.GetComponent<LevelManager>();
                Debug.Log("level manager not null now");
            }
            yield return null;
        }
    }
}
