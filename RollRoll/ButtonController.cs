using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject main;
    public GameObject play;
    public GameObject gameResult;
    public GameObject continueAds;
    public GameObject menu;
    public GameObject alarm;
    public GameObject help;
    public GameObject mission;
    public GameObject shop;
    public GameObject setting;
    public GameObject coin;
    public GameObject pause;
    public GameObject rollSpawner;

    private static ButtonController instance;
    public static ButtonController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<ButtonController>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<ButtonController>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<ButtonController>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnMain()
    {
        main.SetActive(true);
    }

    public void OffMain()
    {
        main.SetActive(false);
    }

    public void OnMenu()
    {
        menu.SetActive(true);
    }

    public void OffMenu()
    {
        menu.SetActive(false);
    }

    public void OnHelp()
    {
        help.SetActive(true);
    }

    public void OffHelp()
    {
        help.SetActive(false);
    }

    public void OnMission()
    {
        mission.SetActive(true);
    }

    public void OffMission()
    {
        mission.SetActive(false);
    }

    public void OnShop()
    {
        shop.SetActive(true);
    }

    public void OffShop()
    {
        Shop.Instance.scrollSize.GetComponent<Transform>().localPosition = Vector2.zero;
        shop.SetActive(false);
    }

    public void OnSetting()
    {
        setting.SetActive(true);
    }

    public void OffSetting()
    {
        setting.SetActive(false);
    }

    public void OnPause()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    public void OffPause()
    {
        Time.timeScale = 1.0f;
        pause.SetActive(false);
    }

    public void OnMyApps()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=4659462324973421495");
    }

    public void GameStart()
    {
        main.SetActive(false);
        menu.SetActive(false);
        play.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Play;
    }

    public void GameContinue()
    {
        GameReset();

        gameResult.SetActive(false);
        continueAds.SetActive(false);
        menu.SetActive(false);

        GameManager.Instance.gameState = GameManager.GameState.Play;
    }

    public void GameResult()
    {
        Time.timeScale = 0f;
        gameResult.SetActive(true);
        menu.SetActive(true);

        if (GameManager.Instance.score > GameManager.Instance.highScore)
        {
            GameManager.Instance.highScore = GameManager.Instance.score;
            Mission.Instance.CheckHighScore();
        }

        GameManager.Instance.totalScore += GameManager.Instance.score;
        if (GameManager.Instance.totalScore > 999999)
            GameManager.Instance.totalScore = 999999;
        if (GameManager.Instance.totalScore >= GameManager.Instance.level_totalScore[GameManager.Instance.totalScoreLevel])
            Mission.Instance.CheckTotalScore();
 
        if (PlayGamesPlatform.Instance.localUser.authenticated && Application.internetReachability != NetworkReachability.NotReachable)
            GPGSManager.Instance.AddScore();

        SoundManager.Instance.PlayRollFailSound();

        GameManager.Instance.gameState = GameManager.GameState.GameResult;
    }

    public void ReturnToMain()
    {
        GameReset();
   
        RollSpawner.Instance.spwanTimer = 4.0f;
        RollSpawner.Instance.checkTimer = 0.6f;
        RollSpawner.Instance.animationSpeed = 1.0f;

        gameResult.SetActive(false);
        continueAds.SetActive(true);
        play.SetActive(false);
        main.SetActive(true);

        GameManager.Instance.score = 0;
        GameManager.Instance.tScore.text = "";

        GameManager.Instance.gameState = GameManager.GameState.Main;
    }

    public void GameReset()
    {
        Time.timeScale = 1.0f;

        RollSpawner.Instance.timer = 2.0f;
        GameManager.Instance.isRoll = false;
        GameManager.Instance.isDragged = false;

        int len = rollSpawner.transform.childCount;
        for (int i = 0; i < len; i++)
        {
            if (rollSpawner.transform.GetChild(i).gameObject.activeSelf == true)
                rollSpawner.transform.GetChild(i).GetComponent<Roll>().Pool.Release(rollSpawner.transform.GetChild(i).gameObject);
        }
        RollSpawner.Instance.StopAllCoroutines();

        RollSpawner.Instance.pool_Queue.Clear();
    }
}