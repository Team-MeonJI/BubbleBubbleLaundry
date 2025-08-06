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
        Init();
    }

    public void Init()
    {
        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        bgmSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
    }

    public void SetMasterVolume(float _volume)
    {

    }

    public void SetBGMVolume(float _volume)
    {

    }

    public void SetSFXVolume(float _volume)
    {

    }
}
