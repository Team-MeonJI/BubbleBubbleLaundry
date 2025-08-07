using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public AudioMixer audioMixer;
    public AudioSource audioSource;

    private Slider masterSlider;
    private Slider bgmSlider;
    private Slider sfxSlider;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float _volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }

    public void SetBGMVolume(float _volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }

    public void SetSFXVolume(float _volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }
}
