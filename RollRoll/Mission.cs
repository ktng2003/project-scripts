using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public GameObject dailyLoginBlock;
    public GameObject highScoreBlock;
    public GameObject totalScoreBlock;

    public Text tHighScore;
    public Text tTotalScore;

    private static Mission instance;
    public static Mission Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = Resources.FindObjectsOfTypeAll<Mission>().FirstOrDefault();

                if (obj != null)
                    instance = obj;
                else
                    Debug.LogError("instance not found");
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = Resources.FindObjectsOfTypeAll<Mission>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.CheckDailyLogin();
        CheckHighScore();
        CheckTotalScore();

        tHighScore.text = GameManager.Instance.highScore + " / " + GameManager.Instance.level_highScore[GameManager.Instance.highScoreLevel];
        tTotalScore.text = GameManager.Instance.totalScore + " / " + GameManager.Instance.level_totalScore[GameManager.Instance.totalScoreLevel];
    }

    public void CheckHighScore()
    {
        if (GameManager.Instance.highScore >= GameManager.Instance.level_highScore[GameManager.Instance.highScoreLevel])
        {
            GameManager.Instance.isGetHighScoreCoin = false;
            highScoreBlock.SetActive(false);
        }
        else
        {
            GameManager.Instance.isGetHighScoreCoin = true;
            highScoreBlock.SetActive(true);
        }

        GameManager.Instance.CheckAlarm();
    }

    public void CheckTotalScore()
    {
        if (GameManager.Instance.totalScore >= GameManager.Instance.level_totalScore[GameManager.Instance.totalScoreLevel])
        {
            GameManager.Instance.isGetTotalScoreCoin = false;
            totalScoreBlock.SetActive(false);
        }
        else
        {
            GameManager.Instance.isGetTotalScoreCoin = true;
            totalScoreBlock.SetActive(true);
        }

        GameManager.Instance.CheckAlarm();
    }

    public void GetDailyCoin()
    {
        if (GameManager.Instance.coin >= 999989)
            GameManager.Instance.coin = 999999;
        else
            GameManager.Instance.coin += 10;

        GameManager.Instance.tCoin.text = GameManager.Instance.coin.ToString();
        GameManager.Instance.getCoin.transform.GetChild(1).GetComponent<Text>().text = "+ 10";
        GameManager.Instance.StartGetCoinSequence();

        GameManager.Instance.isGetLoginCoin = true;
        dailyLoginBlock.SetActive(true);

        GameManager.Instance.CheckAlarm();

        GameManager.Instance.Save();
    }

    public void GetHighScoreCoin()
    {
        if (GameManager.Instance.coin >= 999979)
            GameManager.Instance.coin = 999999;
        else
            GameManager.Instance.coin += 20;
       
        GameManager.Instance.tCoin.text = GameManager.Instance.coin.ToString();
        GameManager.Instance.getCoin.transform.GetChild(1).GetComponent<Text>().text = "+ 20";
        GameManager.Instance.StartGetCoinSequence();        

        if (GameManager.Instance.highScoreLevel < GameManager.Instance.level_highScore.Count - 1)
            GameManager.Instance.highScoreLevel += 1;

        CheckHighScore();

        tHighScore.text = GameManager.Instance.highScore + " / " + GameManager.Instance.level_highScore[GameManager.Instance.highScoreLevel];
        GameManager.Instance.CheckAlarm();

        GameManager.Instance.Save();
    }

    public void GetTotalScoreCoin()
    {
        if (GameManager.Instance.coin >= 999979)
            GameManager.Instance.coin = 999999;
        else
            GameManager.Instance.coin += 20;

        GameManager.Instance.tCoin.text = GameManager.Instance.coin.ToString();
        GameManager.Instance.getCoin.transform.GetChild(1).GetComponent<Text>().text = "+ 20";
        GameManager.Instance.StartGetCoinSequence();

        if (GameManager.Instance.totalScoreLevel < GameManager.Instance.level_totalScore.Count - 1)
            GameManager.Instance.totalScoreLevel += 1;

        CheckTotalScore();

        tTotalScore.text = GameManager.Instance.totalScore + " / " + GameManager.Instance.level_totalScore[GameManager.Instance.totalScoreLevel];
        GameManager.Instance.CheckAlarm();

        GameManager.Instance.Save();
    }
}