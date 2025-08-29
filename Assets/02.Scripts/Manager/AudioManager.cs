using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public AudioMixer audioMixer;
    public AudioSource audioSource;

    private Slider bgmSlider;
    private Slider sfxSlider;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerPrefs.SetFloat("BGM", 0.2f);
        PlayerPrefs.SetFloat("SFX", 0.2f);

        SetBGMVolume(PlayerPrefs.GetFloat("BGM"));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX"));
    }

    public void Init()
    {
        bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float _volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }

    public void SetBGMVolume(float _volume)
    {
        PlayerPrefs.SetFloat("BGM", _volume);
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }

    public void SetSFXVolume(float _volume)
    {
        PlayerPrefs.SetFloat("SFX", _volume);
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(_volume, 0.0001f, 1.0f)) * 20);
    }
}
