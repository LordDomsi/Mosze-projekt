using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    //f�jlba ment�s adatststrukt�r�ja
    [System.Serializable]
    public class SettingsData //hanger�
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
    public class SaveData //egyes�tett strukt�ra
    {
        public string enteredName;
        public int currentLevel; //jelenlegi szint elment�se
        public int currentScore;
        public int currentHealth;
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

    //kiment�s f�jlba
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData,true);
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        File.WriteAllText(path, json);
        Debug.Log("Saved");
    }
    //bet�lt�s
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

    public void SaveScore(int score)
    {
        saveData.currentScore = score;
        Save();
    }

    public void SaveHealth(int health)
    {
        saveData.currentHealth = health;
        Save();
    }

    public void AddScoreToLeaderboard(int playerScore)
    {
        UserData newUserData = new UserData();
        newUserData.name = saveData.enteredName;
        newUserData.score = playerScore;
        bool newName = true;
        for (int i = 0; i < saveData.leaderboardData.Count; i++)
        {
            if (newUserData.name == saveData.leaderboardData[i].name)
            {
                newName = false;
                if (newUserData.score > saveData.leaderboardData[i].score)
                {
                    saveData.leaderboardData[i].score = newUserData.score;
                    Debug.Log("updated leaderboard data");
                }
            }
        }
        if (newName)
        {
            saveData.leaderboardData.Add(newUserData);
            Debug.Log("added new leaderboard data entry");
        }
        Save();
        
    }

}
