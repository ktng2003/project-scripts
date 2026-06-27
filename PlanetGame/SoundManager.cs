using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> sound_BGM = new List<AudioSource>();
    public List<AudioSource> sound_SE = new List<AudioSource>();

    public Slider sliderBGM;
    public Slider sliderSE;

    private float soundBGM = 1.0f;
    private float soundSE = 1.0f;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SoundManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<SoundManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<SoundManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        CheckSoundBGM();
        CheckSoundSE();
    }

    public void CheckSoundBGM()
    {
        if (PlayerPrefs.HasKey("SoundBGM") == true)
        {
            sound_BGM[0].volume = PlayerPrefs.GetFloat("SoundBGM");

            soundBGM = PlayerPrefs.GetFloat("SoundBGM");
        }
        else
            PlayerPrefs.SetFloat("SoundBGM", soundBGM);

        sliderBGM.value = soundBGM;
    }

    public void CheckSoundSE()
    {
        if (PlayerPrefs.HasKey("SoundSE") == true)
        {
            var len = sound_SE.Count;

            for (int i = 0; i < len; i++)
                sound_SE[i].volume = PlayerPrefs.GetFloat("SoundSE");

            soundSE = PlayerPrefs.GetFloat("SoundSE");
        }
        else
            PlayerPrefs.SetFloat("SoundSE", soundSE);

        sliderSE.value = soundSE;
    }

    public void SetSoundBGM(float _sound)
    {
        PlayerPrefs.SetFloat("SoundBGM", _sound);

        var len = sound_BGM.Count;

        for (int i = 0; i < len; i++)
            sound_BGM[i].volume = _sound;
    }

    public void SetSoundSE(float _sound)
    {
        PlayerPrefs.SetFloat("SoundSE", _sound);

        var len = sound_SE.Count;

        for (int i = 0; i < len; i++)
            sound_SE[i].volume = _sound;
    }

    public void PlayClickSound()
    {
        sound_SE[0].PlayOneShot(sound_SE[0].clip);
    }

    public void PlayFireSound()
    {
        sound_SE[1].PlayOneShot(sound_SE[1].clip);
    }

    public void PlayMergeSound()
    {
        sound_SE[2].PlayOneShot(sound_SE[2].clip);
    }
}