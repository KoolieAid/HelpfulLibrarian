using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public bool tutorialIsDone;
    public Button levelsButton;

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

    // Start is called before the first frame update
    void Start()
    {
        tutorialIsDone = false;
        levelManager = GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialIsDone)
            levelsButton.interactable = true;
        else
            levelsButton.interactable = false;


    }
}
