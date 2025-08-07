using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioManager instance;
    public AudioManager Instance { get { return instance; } }

    public AudioMixer audioMixer;
    private Slider masterSlider;
    private Slider bgmSlider;
    private Slider sfxSlider;

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
        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float _volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(_volume) * 20);
    }

    public void SetBGMVolume(float _volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(_volume) * 20);
    }

    public void SetSFXVolume(float _volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(_volume) * 20);
    }
}
