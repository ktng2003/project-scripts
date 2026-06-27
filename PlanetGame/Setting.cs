using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Setting : MonoBehaviour
{
    public Text tVersion;

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