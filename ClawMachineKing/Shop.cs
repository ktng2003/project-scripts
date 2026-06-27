using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class Shop : MonoBehaviour
{
    public Button btAd;
    public Button btPlaycount100;
    public Button btPlaycount220;
    public Button btPlaycount600;

    private static Shop instance;
    public static Shop Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<Shop>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<Shop>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<Shop>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        ButButtonState();
    }

    public void ButButtonState()
    {
        if (GameManager.Instance.countPlay >= 5)
            btAd.interactable = false;
        else
            btAd.interactable = true;

        if (GameManager.Instance.countPlay > 9899)
            btPlaycount100.interactable = false;
        else
            btPlaycount100.interactable = true;

        if (GameManager.Instance.countPlay > 9779)
            btPlaycount220.interactable = false;
        else
            btPlaycount220.interactable = true;

        if (GameManager.Instance.countPlay > 9399)
            btPlaycount600.interactable = false;
        else
            btPlaycount600.interactable = true;
    }
}