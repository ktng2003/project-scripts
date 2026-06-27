using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<bool> is_CatchCatch = new List<bool>();
    public List<bool> is_PushPush = new List<bool>();
    public List<bool> is_SpinSpin = new List<bool>(); 

    public Canvas mainCanvas;

    public TextMeshProUGUI tMainPlayCount;
    public TextMeshProUGUI tGlobalPlayCount;

    public int countPlay = 5;
    public int acquiredCatchCatchCount = 0;
    public int acquiredPushPushCount = 0;
    public int acquiredSpinSpinCount = 0;
    public int dollCatchCatch;
    public int dollPushPush;
    public int dollSprinSpin;

    private DateTime nowData;

    private int nowYear;
    private int nowMonth;
    private int nowDay;

    string filePath;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        filePath = Application.persistentDataPath + "/PlayerInfo.txt";
        Debug.Log(filePath);

        DollGetCheck();
        Load();

        nowData = DateTime.Now;

        ChangeDay();
        GameCountPlay();
        DollAcquiredCountCheck();

        AudioManager.Instance.audio_BGM[0].Play();
    }

    [System.Serializable]
    public class JsonClass
    {
        public int jsonPlayCount = GameManager.Instance.countPlay;

        public int jsonNowYear = GameManager.Instance.nowYear;
        public int jsonNowMonth = GameManager.Instance.nowMonth;
        public int jsonNowDay = GameManager.Instance.nowDay;

        public List<bool> jsonIsCatchCatch = GameManager.Instance.is_CatchCatch;
        public List<bool> jsonIsPushPush = GameManager.Instance.is_PushPush;
        public List<bool> jsonIsSpinSpin = GameManager.Instance.is_SpinSpin;
    }

    public void Save()
    {
        JsonClass savefile = new JsonClass();

        string jdata = JsonUtility.ToJson(savefile);
        File.WriteAllText(filePath, jdata);
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            ResetInformation();
            return;
        }

        string jdata = File.ReadAllText(filePath);
        JsonClass data = JsonUtility.FromJson<JsonClass>(jdata);

        countPlay = data.jsonPlayCount;

        nowYear = data.jsonNowYear;
        nowMonth = data.jsonNowMonth;
        nowDay = data.jsonNowDay;

        is_CatchCatch = data.jsonIsCatchCatch;
        is_PushPush = data.jsonIsPushPush;
        is_SpinSpin = data.jsonIsSpinSpin;

        ExtendList(is_CatchCatch, Doll.Instance.doll_CatchCatch.Count);
        ExtendList(is_PushPush, Doll.Instance.doll_PushPush.Count);
        ExtendList(is_SpinSpin, Doll.Instance.doll_SpinSpin.Count);
    }

    public void ResetInformation()
    {
        Save();
        Load();
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    void OnApplicationQuit()
    {
        nowYear = nowData.Year;
        nowMonth = nowData.Month;
        nowDay = nowData.Day;

        Save();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            nowYear = nowData.Year;
            nowMonth = nowData.Month;
            nowDay = nowData.Day;

            Save();
        }
    }

    public void DollGetCheck()
    {
        var lenCatchCatch = Doll.Instance.doll_CatchCatch.Count;
        for (int i = 0; i < lenCatchCatch; i++)
            is_CatchCatch.Add(false);

        var lenPushPush = Doll.Instance.doll_PushPush.Count;
        for (int j = 0; j < lenPushPush; j++)
            is_PushPush.Add(false);

        var lenSpinSpin = Doll.Instance.doll_SpinSpin.Count;
        for (int k = 0; k < lenSpinSpin; k++)
            is_SpinSpin.Add(false);
    }

    public void DollAcquiredCountCheck()
    {
        var lenCatchCatch = Doll.Instance.doll_CatchCatch.Count;
        for (int i = 0; i < lenCatchCatch; i++)
        {
            if (is_CatchCatch[i] == true)
                acquiredCatchCatchCount += 1;
        }

        var lenPushPush = Doll.Instance.doll_PushPush.Count;
        for (int j = 0; j < lenPushPush; j++)
        {
            if (is_PushPush[j] == true)
                acquiredPushPushCount += 1;
        }

        var lenSpinSpin = Doll.Instance.doll_SpinSpin.Count;
        for (int k = 0; k < lenSpinSpin; k++)
        {
            if (is_SpinSpin[k] == true)
                acquiredSpinSpinCount += 1;
        }
    }

    public void ChangeDay()
    {
        if (nowYear != nowData.Year || nowMonth != nowData.Month || nowDay != nowData.Day)
        {
            if (countPlay < 5)
            {
                countPlay = 5;
                Save();
            }
        }
    }

    public void GameCountPlay()
    {
        tMainPlayCount.text = countPlay.ToString() + "/5";
        tGlobalPlayCount.text = countPlay.ToString() + "/5";
    }

    private void ExtendList(List<bool> list, int targetLength)
    {
        while (list.Count < targetLength)
            list.Add(false);
    }
}