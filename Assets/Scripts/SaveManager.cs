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
        if (!PlayerPrefs.HasKey("save-data")) return;

        var d = PlayerPrefs.GetString("save-data");
        var obj = JsonUtility.FromJson<SaveData>(d);

        var lvlM = GameManager.instance.levelManager;
        var gM = GameManager.instance;

        gM.tutorialIsDone = obj.isTutorialDone;
        lvlM.levelsUnlocked = new List<int>(obj.unlockedLevels);

        var dict = new Dictionary<int, int>();
        obj.lvlStars.ForEach(p => { dict.Add(p.key, p.value); });

        gM.SetNewRecord(dict);
    }

    public void SaveFromCurrent()
    {
        SaveToDrive(FromCurrent());
    }

    private string FromCurrent()
    {
        var gmMng = GameManager.instance;
        var lvlMng = GameManager.instance.levelManager;

        var origDict = gmMng.GetRecordCopy();

        var tempList = new List<Pair<int, int>>();

        foreach (var keyValuePair in origDict)
        {
            var t = new Pair<int, int>();
            t.key = keyValuePair.Key;
            t.value = keyValuePair.Value;
            tempList.Add(t);
        }

        var data = new SaveData()
        {
            isTutorialDone = gmMng.tutorialIsDone,
            unlockedLevels = new List<int>(lvlMng.levelsUnlocked),
            lvlStars = tempList,
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

    public List<Pair<int, int>> lvlStars;
}

[Serializable]
public struct Pair<K, V>
{
    public K key;
    public V value;
}