using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject help;
    public GameObject setting;

    public Button changePlanetButton;

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

    public void OnSetting()
    {
        setting.SetActive(true);
    }

    public void OffSetting()
    {
        setting.SetActive(false);
    }

    public void OnHelp()
    {
        help.SetActive(true);
    }

    public void OffHelp()
    {
        help.SetActive(false);
    }      
}