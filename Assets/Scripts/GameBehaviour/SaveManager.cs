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
    public class PlayerData //player data
    {
        public int currentLevel; 
        public int currentScore;
        public int currentHealth;
        public float currentDamage;
        public float currentAttackSpeed;
        public float currentMovementSpeed;
        public int currentMaxShield;
    }
    [System.Serializable]
    public class SaveData //egyesített struktúra
    {
        public string enteredName;
        public PlayerData playerData;
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
            CreateDefaultSaveFile(); //ha nincs save data file akkor létrehoz egyet
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
        Debug.Log("Saved Data To File");
    }
    //betöltés
    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        string pureText = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(pureText);
        loaded = true;
        Debug.Log("Loaded Data From File");
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
    public void SaveEnteredName(string name)
    {
        saveData.enteredName = name;
        Save();
    }

    public void SavePlayerData(int health, int score, int level, float damage, float attackSpeed, float movSpeed, int maxShield)
    {
        saveData.playerData.currentHealth = health;
        saveData.playerData.currentScore = score;
        saveData.playerData.currentLevel = level;
        saveData.playerData.currentDamage = damage;
        saveData.playerData.currentAttackSpeed = attackSpeed;
        saveData.playerData.currentMovementSpeed = movSpeed;
        saveData.playerData.currentMaxShield = maxShield;
        Save();
    }

    public void AddScoreToLeaderboard(int playerScore) //pontszám hozzáadása leaderboardhoz
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
                if (newUserData.score > saveData.leaderboardData[i].score) //csak akkor menti el a pontszámot ha megdönti a beírt névhez tartozót
                {
                    saveData.leaderboardData[i].score = newUserData.score;
                    Debug.Log("updated leaderboard data");
                }
            }
        }
        if (newName) // ha olyan név van beírva ami még nincs a leaderboardban akkor azt elmenti
        {
            saveData.leaderboardData.Add(newUserData);
            Debug.Log("added new leaderboard data entry");
        }
        Save();
        
    }

}
