using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{  
    public TextMeshProUGUI tVersion;

    public Transform setting;

    private void OnEnable()
    {
        AudioManager.Instance.CheckAudioBGM();
        AudioManager.Instance.CheckAudioSE();

        setting.DOLocalMoveY(0f, 0.2f).From(-1080.0f, true, true).SetEase(Ease.OutQuad);
    }
 
    void Start()
    {
        NowVersion();
    }

    public void NowVersion()
    {
        tVersion.text = "v" + Application.version;
    }

    public void OnMyApps()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=4659462324973421495");
    }
}