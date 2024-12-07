using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    //fájlba mentés adatststruktúrája
    [System.Serializable]
    public class SettingsData //hangerõ
    {
        public float musicVolume;
        public float sfxVolume;
    }
    [System.Serializable]
    public class UserData //az egyes leaderboard elemek
    {
        public string name;
        public int score;
    }
    [System.Serializable]
    public class SaveData //egyesített struktúra
    {
        public string enteredName;
        public int currentLevel; //jelenlegi szint elmentése
        public SettingsData settings;
        public List<UserData> leaderboardData = new List<UserData>(); //leaderboard lista
    }

    public SaveData saveData;
    public bool loaded = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(File.Exists(Path.Combine(Application.persistentDataPath, "saveData.json")))
        {
            Load();
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new save file.");
            CreateDefaultSaveFile();
            Load();
        }
        
    }

    void CreateDefaultSaveFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(filePath, json);
    }

    //kimentés fájlba
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData,true);
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        File.WriteAllText(path, json);
        Debug.Log(Application.persistentDataPath);
    }
    //betöltés
    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        string pureText = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(pureText);
        loaded = true;
    }

    public void SaveMusicData(float volume)
    {
        saveData.settings.musicVolume = volume;
        Save();
    }

    public void SaveSFXData(float volume)
    {
        saveData.settings.sfxVolume = volume;
        Save();
    }

    public void SaveLevel(int level)
    {
        saveData.currentLevel = level;
        Save();
    }

    public void SaveEnteredName(string name)
    {
        saveData.enteredName = name;
        Save();
    }

    
}
