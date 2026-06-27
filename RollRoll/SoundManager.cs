using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{ 
    public List<AudioSource> list_Sound = new List<AudioSource>();

    public Image stateSound;

    public Sprite onSound;
    public Sprite offSound;

    private int sounds = 1;

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
        CheckAudio();
    }

    public void CheckAudio()
    {
        if (PlayerPrefs.HasKey("Sounds") == true)
            sounds = PlayerPrefs.GetInt("Sounds");
        else
            PlayerPrefs.SetInt("Sounds", sounds);

        if (sounds == 1)
            stateSound.sprite = onSound;
        else
            stateSound.sprite = offSound;
    }

    public void OnOffAudio()
    {
        if (sounds == 1)
        {
            stateSound.sprite = offSound;
            sounds = 0;
            PlayerPrefs.SetInt("Sounds", sounds);
        }
        else
        {
            stateSound.sprite = onSound;
            sounds = 1;
            PlayerPrefs.SetInt("Sounds", sounds);
        }
    }

    public void PlayTitleSound()
    {
        if (sounds == 1)
            list_Sound[0].Play();
    }

    public void PlayClickSound()
    {
        if (sounds == 1)
            list_Sound[1].Play();
    }
    public void PlayMissionCompleteSound()
    {
        if (sounds == 1)
            list_Sound[2].Play();
    }

    public void PlayRollSuccessSound()
    {
        if (sounds == 1)
            list_Sound[3].Play();
    }
    public void PlayRollFailSound()
    {
        if (sounds == 1)
            list_Sound[4].Play();
    }
}