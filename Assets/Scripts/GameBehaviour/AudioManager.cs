using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //hangok�rt felel�s script
    public static AudioManager Instance {  get; private set; }

    //hang t�pusok
    public enum SFX_enum
    {
        PLAYER_SHOOT,
        ENEMY_SHOOT,
        BOSS_SHOOT,
        ENEMY_HIT,
        EXPLOSION,
        BOSS_EXPLOSION,
        ASTEROID_DESTROY,
        PLAYER_HIT,
        PICKUP_LOCATOR,
        POWERUP,
        ENTER_BLACKHOLE,
        BOSS_SPAWN,
        BUTTON_CLICK,
        BUTTON_HOVER,
        DIALOGUE_POPUP,
        NEXT_TEXT,
        TYPING,
        ENGINE_SOUND
    }
    public enum Music_enum
    {
        MENU_THEME,
        GAME_THEME,
        GAMEOVER_THEME,
        ENDING_THEME,
        INTRO_THEME
    }

    //hangokhoz kapcsol�d� adatstrukt�ra
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
    public AudioSource sfxSource;

    private List<AudioSource> sfxList = new List<AudioSource>();

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

    //mixer hangerej�nek be�ll�t�s�hoz
    public void SetVolume(float value, string param, Slider slider)
    {
        audioMixer.SetFloat(param, value: Mathf.Log10(value) * 40);
        if(value == 0f) audioMixer.SetFloat(param, -80f);
        slider.value = value;
        Debug.Log(value + param);
    }

    //zene lej�tsz�sa
    public void PlayMusic(Music_enum type)
    {
        var auSource = musicSource;

        for (int i = 0; i < musicData.Count; i++)
        {
            if (musicData[i].type == type) //megkeresi a k�rt t�pus� zen�t �s azt j�tsza le
            {
                auSource.clip = musicData[i].clip;
                auSource.volume = musicData[i].volume;
                auSource.loop = musicData[i].loop;
                auSource.Play();
                break;
            }
        }
    }
    
    //szinte ugyanaz mint a PlayMusic
    public void PlaySFX(SFX_enum type)
    {
        var auSource = GetAudioSource(sfxList, sfxSource); //ez elt�r

        for (int i = 0; i < sfxData.Count; i++)
        {
            if (sfxData[i].type == type)
            {
                auSource.clip = sfxData[i].clip;
                auSource.volume = sfxData[i].volume;
                auSource.loop = sfxData[i].loop;
                if (type == SFX_enum.ASTEROID_DESTROY) { auSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f); }
                else auSource.pitch = 1f;
                auSource.Play();
                break;
            }
        }
    }

    public AudioSource GetAudioSource(List<AudioSource> _list, AudioSource source) //�j audio source-t hoz l�tre a hangoknak ha minden source �pp haszn�latban van
    {
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (!sfxList[i].isPlaying)
            {
                return sfxList[i]; //ha tal�lt �res audio source akkor azt �jra felhaszn�lja
            }
        }

        var newSource = Instantiate(source, source.transform.parent);
        newSource.gameObject.SetActive(true);
        sfxList.Add(newSource);
        return newSource;
    }
}
