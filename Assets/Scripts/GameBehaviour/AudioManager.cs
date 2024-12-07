using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //hangokért felelõs script
    public static AudioManager Instance {  get; private set; }

    //hang típusok
    public enum SFX_enum
    {
        PLAYER_SHOOT,
        ENEMY_HIT,
        ENEMY_DEATH,
        PLAYER_HIT,
        PICKUP_LOCATOR,
        ENTER_BLACKHOLE,
        DEFEAT_BOSS
    }
    public enum Music_enum
    {
        MENU_THEME,
        GAME_THEME
    }

    //hangokhoz kapcsolódó adatstruktúra
    [Serializable]
    public class Data
    {
        public AudioClip clip;
        public float volume;
        public bool loop;
    }
    [Serializable]
    public class SFXData : Data
    {
        public SFX_enum type;
    }
    [Serializable]
    public class MusicData : Data
    {
        public Music_enum type;
    }
    public List<SFXData> sfxData = new List<SFXData>();
    public List<MusicData> musicData = new List<MusicData>();

    public AudioSource musicSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    private const string SFXExposedParam = "SFXVolumeExposedParam";
    private const string MusicExposedParam = "MusicVolumeExposedParam";



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

    //mixer hangerejének beállításához
    public void SetVolume(float value, string param, Slider slider)
    {
        audioMixer.SetFloat(param, value: Mathf.Log10(value) * 40);
        slider.value = value;
        Debug.Log(value + param);
    }
}
