using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{  
    public List<AudioSource> audio_BGM = new List<AudioSource>();
    public List<AudioSource> audio_SE = new List<AudioSource>();

    public Slider sliderBGM;
    public Slider sliderSE;  

    private float audioBGM = 1.0f;
    private float audioSE = 1.0f;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<AudioManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<AudioManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<AudioManager>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        CheckAudioBGM();
        CheckAudioSE();
    }

    public void CheckAudioBGM()
    {
        if (PlayerPrefs.HasKey("AudioBGM") == true)
        {
            var len = audio_BGM.Count;

            for (int i = 0; i < len; i++)
                audio_BGM[i].volume = PlayerPrefs.GetFloat("AudioBGM");

            audioBGM = PlayerPrefs.GetFloat("AudioBGM");
        }
        else
            PlayerPrefs.SetFloat("AudioBGM", audioBGM);
      
        sliderBGM.value = audioBGM;
    }

    public void CheckAudioSE()
    {
        if (PlayerPrefs.HasKey("AudioSE") == true)
        {
            var len = audio_SE.Count;

            for (int i = 0; i < len; i++)
                audio_SE[i].volume = PlayerPrefs.GetFloat("AudioSE");

            audioSE = PlayerPrefs.GetFloat("AudioSE");
        }
        else
            PlayerPrefs.SetFloat("AudioSE", audioSE);

        sliderSE.value = audioSE;
    }

    public void SetAudioBGM(float _audio)
    {
        PlayerPrefs.SetFloat("AudioBGM", _audio);

        var len = audio_BGM.Count;

        for (int i = 0; i < len; i++)
            audio_BGM[i].volume = _audio;
    }

    public void SetAudioSE(float _audio)
    {
        PlayerPrefs.SetFloat("AudioSE", _audio);

        var len = audio_SE.Count;

        for (int i = 0; i < len; i++)
            audio_SE[i].volume = _audio;
    }

    public void AudioClick()
    {
        audio_SE[0].Play();
    }

    public void AudioRail()
    {
        if (audio_SE[1].isPlaying == false)
            audio_SE[1].Play();
    }

    public void AudioPush()
    {
        audio_SE[2].Play();
    }

    public void AudioDollGet()
    {
        audio_SE[3].Play();
    }
}