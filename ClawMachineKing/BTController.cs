using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using DG.Tweening;

public class BTController : MonoBehaviour
{
    public List<GameObject> bt_Main = new List<GameObject>(); 
    public List<GameObject> bt_GameMode = new List<GameObject>();
    public List<GameObject> bt_Global = new List<GameObject>();   

    private static BTController instance;
    public static BTController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<BTController>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<BTController>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<BTController>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    } 

    public void OnCollection()
    {
        CameraManager.Instance.transform.position = new Vector3(0f, 0f, -1000.0f);
        bt_Main[1].SetActive(true);
    }

    public void OffCollection()
    {
        int len = Collection.Instance.list_Collection.Count;

        for (int i = 0; i < len; i++)
        {
            Collection.Instance.bt_CollectionType[i].GetComponent<Image>().color = new Color(153 / 255f, 153 / 255f, 153 / 255f);
            Collection.Instance.bt_CollectionType[i].GetComponent<Button>().interactable = true;
            Collection.Instance.t_CollectionType[i].color = new Color(153 / 255f, 153 / 255f, 153 / 255f);
            Collection.Instance.scroll_Pos[i].transform.localPosition = new Vector3(Collection.Instance.scroll_Pos[0].transform.localPosition.x, 0f, 0f);
            Collection.Instance.list_Collection[i].SetActive(false);
        }
        CameraManager.Instance.transform.position = new Vector3(275.0f, 235.0f, -230.0f);
        bt_Main[1].SetActive(false);
    }

    public void OnDollInformation()
    {
        OffGlobalUI();
        bt_Main[2].SetActive(true);
    }

    public void OffDollInformation()
    {
        bt_Main[2].SetActive(false);
        OnGlobalUI();
    }  
    
    public void OnGameMode()
    {
        OffGlobalUI();
        bt_Main[3].SetActive(true);
        bt_Main[4].transform.localScale = new Vector3(1.0f, 0f, 1.0f);
        bt_Main[4].transform.DOScaleY(1.0f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OffGameMode()
    {
        bt_Main[3].SetActive(false);
        OnGlobalUI();
    }  
  
    public void OnShop()
    {
        bt_Global[0].SetActive(true);
    }

    public void OffShop()
    {
        bt_Global[0].SetActive(false);

        if (bt_Main[1].activeSelf == true)
        {
            CameraManager.Instance.transform.position = new Vector3(275.0f, 235.0f, -230.0f);
            bt_Main[1].SetActive(false);
        }
    }

    public void OnGlobalUI()
    {
        bt_Global[1].SetActive(true);
        bt_Global[2].SetActive(true);
    }

    public void OffGlobalUI()
    {
        bt_Global[1].SetActive(false);
        bt_Global[2].SetActive(false);
    }

    public void OnSetting()
    {
        bt_Global[3].SetActive(true);
    }

    public void OffSetting()
    {
        bt_Global[3].SetActive(false);
    } 
    
    public void OnDollGet()
    {
        bt_GameMode[4].SetActive(true);
    }

    public void OffDollGet()
    {
        bt_GameMode[4].SetActive(false);
    }
    
    public void OnCatchCatch()
    {
        OffGameMode();
        bt_Main[0].SetActive(false);

        CatchCatchDollDrop.Instance.StartCoroutine("DollDropCatchCatch");

        CameraManager.Instance.transform.position = new Vector3(1000.0f, 290.0f, -200.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.CatchCatchMiddle;

        bt_GameMode[0].SetActive(true);
        bt_GameMode[1].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Stop();
        AudioManager.Instance.audio_BGM[1].Play();
    }

    public void OffCatchCatch()
    {
        CatchCatch.Instance.countCatchCatch = 12;
        CatchCatch.Instance.respawnTime = 0f;

        var len = CatchCatchDollDrop.Instance.list_Doll.Count;

        for (int i = 0; i < len; i++)
        {
            Destroy(CatchCatchDollDrop.Instance.list_Doll[i]);
            CatchCatchDollDrop.Instance.list_Doll[i] = null;
        }

        CameraManager.Instance.transform.position = new Vector3(275.0f, 235.0f, -230.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(0.0f, -30.0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.Main;

        bt_GameMode[0].SetActive(false);
        bt_GameMode[1].SetActive(false);
        bt_Main[0].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Play();
        AudioManager.Instance.audio_BGM[1].Stop();
    }

    public void OnPushPush()
    {
        OffGameMode();
        bt_Main[0].SetActive(false);

        PushPushDollDrop.Instance.DollDropPushPush();

        CameraManager.Instance.transform.position = new Vector3(2000.0f, 290.0f, -288.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.PushPushMiddle;

        bt_GameMode[0].SetActive(true);
        bt_GameMode[2].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Stop();
        AudioManager.Instance.audio_BGM[1].Play();
    }

    public void OffPushPush()
    {
        PushPush.Instance.countPushPush = 6;
        PushPush.Instance.respawnTime = 0f;

        var len = PushPushDollDrop.Instance.list_Doll.Count;

        for (int i = 0; i < len; i++)
        {
            Destroy(PushPushDollDrop.Instance.list_Doll[i]);
            PushPushDollDrop.Instance.list_Doll[i] = null;
        }

        CameraManager.Instance.transform.position = new Vector3(275.0f, 235.0f, -230.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(0.0f, -30.0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.Main;

        bt_GameMode[0].SetActive(false);
        bt_GameMode[2].SetActive(false);
        bt_Main[0].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Play();
        AudioManager.Instance.audio_BGM[1].Stop();
    }

    public void OnSpinSpin()
    {
        OffGameMode();
        bt_Main[0].SetActive(false);

        SpinSpinDollDrop.Instance.DollDropSpinSpin();

        CameraManager.Instance.transform.position = new Vector3(3000.0f, 290.0f, -288.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.SpinSpinMiddle;

        bt_GameMode[0].SetActive(true);
        bt_GameMode[3].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Stop();
        AudioManager.Instance.audio_BGM[1].Play();
    }

    public void OffSpinSpin()
    {
        SpinSpin.Instance.countSpinSpin = 10;
        SpinSpin.Instance.respawnTime = 0f;

        var lenSpin = SpinSpinDollDrop.Instance.list_Doll.Count;

        for (int i = 0; i < lenSpin; i++)
        {
            Destroy(SpinSpinDollDrop.Instance.list_Doll[i]);
            SpinSpinDollDrop.Instance.list_Doll[i] = null;
        }

        var lenClaw = SpinSpinDollDrop.Instance.list_Claw.Count;

        for (int j = 0; j < lenClaw; j++)
        {
            var clawPos = SpinSpinDollDrop.Instance.list_Claw[j].transform;

            clawPos.localPosition = new Vector3(clawPos.localPosition.x, clawPos.localPosition.y, -1.0f);
            clawPos.Rotate(new Vector3(12.0f, clawPos.rotation.y, clawPos.rotation.z));
        }

        CameraManager.Instance.transform.position = new Vector3(275.0f, 235.0f, -230.0f);
        CameraManager.Instance.transform.rotation = Quaternion.Euler(0.0f, -30.0f, 0f);
        CameraManager.Instance.cameraState = CameraManager.CameraState.Main;

        bt_GameMode[0].SetActive(false);
        bt_GameMode[3].SetActive(false);
        bt_Main[0].SetActive(true);

        AudioManager.Instance.audio_BGM[0].Play();
        AudioManager.Instance.audio_BGM[1].Stop();
    }
}