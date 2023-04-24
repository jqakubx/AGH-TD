using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    private static readonly string MUSIC_PREF_KEY = "Music";
    private static readonly string SFX_PREF_KEY = "SFX";
    private static readonly float DEFAULT_VOLUME = 0.5f;
        
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider musicSlider;
    
    [SerializeField]
    private Slider sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();
        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySFX("boom");
        }
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat(MUSIC_PREF_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SFX_PREF_KEY, sfxSlider.value);
    }

    public void LoadVolume()
    {
        musicSource.volume = PlayerPrefs.GetFloat(MUSIC_PREF_KEY, DEFAULT_VOLUME);
        sfxSource.volume = PlayerPrefs.GetFloat(SFX_PREF_KEY, DEFAULT_VOLUME);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}
