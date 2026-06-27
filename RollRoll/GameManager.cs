using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.Purchasing;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Main = 0,
        Play = 1,
        GameResult = 2,
    }  
    
    public List<int> level_highScore = new List<int>();
    public List<int> level_totalScore = new List<int>();
   
    public List<bool> is_Skin = new List<bool>();

    public GameState gameState;

    public GameObject getCoin;
    public GameObject block;

    public Text tVersion;
    public Text tCoin;
    public Text tScore;

    public Image stateVibration;

    public Sprite onVibration;
    public Sprite offVibration;

    public Animator animator;

    public int coin = 0;
    public int score = 0;
    public int highScore = 0;
    public int totalScore = 0;
    public int highScoreLevel = 0;
    public int totalScoreLevel = 0;
    public int selectSkin = 0;
    public int vibration = 1;

    public bool isRemoveAllAds = false;
    public bool isGetLoginCoin = false;
    public bool isGetHighScoreCoin = true;
    public bool isGetTotalScoreCoin = true;
    public bool isDragged = false;
    public bool isRoll = false;

    private DateTime nowData;

    private Coroutine getCoinCoroutine;

    private int nowYear;
    private int nowMonth;
    private int nowDay;

    string filePath;

    Vector2 startPos, deltaPos, nowPos;
    const float dragAccuracy = 50.0f;

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

        CheckGetSkin();
        Load();
        nowData = DateTime.Now;

        StartCoroutine(FadeInGameStart());  

        CheckVibration();
        NowVersion();
        tCoin.text = tCoin.text = coin.ToString();
        CheckDailyLogin();
        CheckAlarm();

        gameState = GameState.Main;
    }

    void Update()
    {
        TouchDetection();
    }

    [System.Serializable]
    public class JsonClass
    {
        public int jsonCoin = GameManager.Instance.coin;
        public int jsonHighScore = GameManager.Instance.highScore;
        public int jsonTotalScore = GameManager.Instance.totalScore;
        public int jsonHighScoreLevel = GameManager.Instance.highScoreLevel;
        public int jsonTotalScoreLevel = GameManager.Instance.totalScoreLevel;
        public int jsonSelectSkin = GameManager.Instance.selectSkin;

        public int jsonNowYear = GameManager.Instance.nowYear;
        public int jsonNowMonth = GameManager.Instance.nowMonth;
        public int jsonNowDay = GameManager.Instance.nowDay;

        public bool jsonIsRemoveAllAds = GameManager.Instance.isRemoveAllAds;

        public bool jsonIsGetLoginCoin = GameManager.Instance.isGetLoginCoin;
        public bool jsonIsGetHighScoreCoin = GameManager.Instance.isGetHighScoreCoin;
        public bool jsonIsGetTotalScoreCoin = GameManager.Instance.isGetTotalScoreCoin;
      
        public List<bool> jsonIsSkin = GameManager.Instance.is_Skin;
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

        coin = data.jsonCoin;
        highScore = data.jsonHighScore;
        totalScore = data.jsonTotalScore;
        highScoreLevel = data.jsonHighScoreLevel;
        totalScoreLevel = data.jsonTotalScoreLevel;
        selectSkin = data.jsonSelectSkin;

        nowYear = data.jsonNowYear;
        nowMonth = data.jsonNowMonth;
        nowDay = data.jsonNowDay;

        isRemoveAllAds = data.jsonIsRemoveAllAds;

        isGetLoginCoin = data.jsonIsGetLoginCoin;
        isGetHighScoreCoin = data.jsonIsGetHighScoreCoin;
        isGetTotalScoreCoin = data.jsonIsGetTotalScoreCoin;

        is_Skin = data.jsonIsSkin;
    }

    public void ResetInformation()
    {
        Save();
        Load();
    }

    public void CheckAlarm()
    {
        if (isGetLoginCoin == false || isGetHighScoreCoin == false || isGetTotalScoreCoin == false)
            ButtonController.Instance.alarm.SetActive(true);
        else
            ButtonController.Instance.alarm.SetActive(false);
    }

    public void CheckDailyLogin()
    {
        nowData = DateTime.Now;

        if (nowYear != nowData.Year || nowMonth != nowData.Month || nowDay != nowData.Day && isGetLoginCoin == true)
        {
            nowYear = nowData.Year;
            nowMonth = nowData.Month;
            nowDay = nowData.Day;

            Mission.Instance.dailyLoginBlock.SetActive(false);
            isGetLoginCoin = false;
            Save();
        }
        else if (nowYear == nowData.Year && nowMonth == nowData.Month && nowDay == nowData.Day && isGetLoginCoin == true)
            Mission.Instance.dailyLoginBlock.SetActive(true);
    }

    public void CheckGetSkin()
    {
        var len = Shop.Instance.list_Skin.Count - 1;
        for (int i = 0; i < len; i++)
            is_Skin.Add(false);
    }

    public void CheckVibration()
    {
        if (PlayerPrefs.HasKey("Vibration") == true)
            vibration = PlayerPrefs.GetInt("Vibration");
        else
            PlayerPrefs.SetInt("Vibration", vibration);

        if (vibration == 1)
            stateVibration.sprite = onVibration;
        else
            stateVibration.sprite = offVibration;
    }

    public void OnOffVibration()
    {
        if (vibration == 1)
        {
            stateVibration.sprite = offVibration;
            vibration = 0;
            PlayerPrefs.SetInt("Vibration", vibration);
        }
        else
        {
            stateVibration.sprite = onVibration;
            vibration = 1;
            PlayerPrefs.SetInt("Vibration", vibration);
        }

        if (vibration == 1)
            Handheld.Vibrate();
    }

    public void NowVersion()
    {
        tVersion.text = "v" + Application.version;
    }

    public void TouchDetection()
    {
        if (gameState == GameState.Play && ButtonController.Instance.pause.activeSelf == false)
        {
            if (Input.GetMouseButton(0))
            {
                nowPos = (Input.touchCount == 0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;

                if (Input.GetMouseButtonDown(0))
                {
                    startPos = nowPos;
                    deltaPos = Vector2.zero;
                }

                deltaPos = nowPos - startPos;

                if (deltaPos.sqrMagnitude > dragAccuracy * dragAccuracy)
                    isDragged = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDragged)
                {
                    if (RollSpawner.Instance.pool_Queue.Count == 0)
                        ButtonController.Instance.GameResult();
                    else
                        DetectDirection(deltaPos);
                }

                isDragged = false;
            }
        }
    }

    public void DetectDirection(Vector2 deltaPos)
    {
        float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;

        if (angle >= -45.0f && angle < 45.0f) // 오른쪽
            ProcessDirection(2);
        else if (angle >= 45.0f && angle < 135.0f) // 위쪽
            ProcessDirection(1);
        else if (angle >= -135.0f && angle < -45.0f) // 아래쪽
            ProcessDirection(3);
        else // 왼쪽
            ProcessDirection(0);
    }

    private void ProcessDirection(int expectedDirection)
    {
        var roll = RollSpawner.Instance.pool_Queue[0].GetComponent<Roll>();
        if (roll.timer >= RollSpawner.Instance.despawnTime / RollSpawner.Instance.animationSpeed - RollSpawner.Instance.checkTimer
            && roll.direction == expectedDirection)
        {
            ChangeScore();
            SoundManager.Instance.PlayRollSuccessSound();
        }
        else
            ButtonController.Instance.GameResult();
    }

    public void ChangeScore()
    {
        RollSpawner.Instance.pool_Queue[0].GetComponent<Roll>().isSwipeSuccessful = true;
        if (score < 999999)
        {
            score += 1;
            tScore.text = score.ToString();
        }
        else
            score = 999999;

        if (score != 0 && score % 10 == 0)
        {
            coin += 1;
            tCoin.text = coin.ToString();
            getCoin.transform.GetChild(1).GetComponent<Text>().text = "+ 1";
            StartGetCoinSequence();

            Save();
        }
    }

    public void StartGetCoinSequence()
    {
        if (getCoinCoroutine != null)
            StopCoroutine(getCoinCoroutine);

        StopCoroutine(FadeGetCoin());
        StartCoroutine(GetCoin());
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

    IEnumerator FadeInGameStart()
    {
        block.SetActive(true);       

        yield return new WaitForSeconds(1.5f);
        SoundManager.Instance.PlayTitleSound();

        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.PlayTitleSound();

        yield return new WaitForSeconds(1.0f);

        animator.enabled = false;
        block.SetActive(false);
    }

    IEnumerator GetCoin()
    {
        yield return null;

        getCoinCoroutine = StartCoroutine(FadeGetCoin());
    }

    IEnumerator FadeGetCoin()
    {
        getCoin.GetComponent<CanvasGroup>().alpha = 1.0f;

        yield return new WaitForSeconds(1.0f);

        float elapsedTime = 0f;
        float fadedTime = 0.5f;

        while (elapsedTime <= fadedTime)
        {
            getCoin.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1.0f, 0f, elapsedTime / fadedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        getCoin.GetComponent<CanvasGroup>().alpha = 0f;
        getCoinCoroutine = null;
    }
}