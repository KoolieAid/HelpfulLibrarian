using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void TryGetSave()
    {
        if (PlayerPrefs.HasKey("save-data")){
        {
            var d = PlayerPrefs.GetString("save-data");
            var obj = JsonUtility.FromJson<SaveData>(d);

            var lvlM = GameManager.instance.levelManager;
            var gM = GameManager.instance;

            gM.tutorialIsDone = obj.isTutorialDone;
            lvlM.levelsUnlocked = new List<int>(obj.unlockedLevels);
        }}
        
        SaveToDrive(Encode(GameManager.instance.levelManager, GameManager.instance));
    }

    private string Encode(LevelManager lvlMng, GameManager gmMng)
    {
        var data = new SaveData()
        {
            isTutorialDone = gmMng.tutorialIsDone,
            unlockedLevels = new List<int>(lvlMng.levelsUnlocked),
        };

        return JsonUtility.ToJson(data);
    }

    private void SaveToDrive(string json)
    {
        PlayerPrefs.SetString("save-data", json);
        PlayerPrefs.Save();
    }
}

[Serializable]
public struct SaveData
{
    public bool isTutorialDone;

    public List<int> unlockedLevels;
}
