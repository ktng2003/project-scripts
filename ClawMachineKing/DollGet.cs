using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DollGet : MonoBehaviour
{
    public Image dollIcon;

    void OnTriggerEnter(Collider other)
    {
        GameObject doll = other.transform.parent.gameObject;

        if (doll.TryGetComponent<AlreadyCollected>(out _)) return;
        doll.AddComponent<AlreadyCollected>();

        string tag = doll.tag;
        int index = int.Parse(doll.transform.GetChild(0).name) - 1;

        BTController.Instance.OnDollGet();

        if (tag == "CatchCatch")
        {
            dollIcon.sprite = Doll.Instance.atlasCatchCatch.GetSprite("img_Doll_CatchCatch" + (index + 1));
            if(GameManager.Instance.is_CatchCatch[index] == false)
                GameManager.Instance.acquiredCatchCatchCount++;
            GameManager.Instance.is_CatchCatch[index] = true;
            CatchCatch.Instance.countCatchCatch -= 1;
        }
        else if (tag == "PushPush")
        {
            dollIcon.sprite = Doll.Instance.atlasPushPush.GetSprite("img_Doll_PushPush" + (index + 1));
            if (GameManager.Instance.is_PushPush[index] == false)
                GameManager.Instance.acquiredPushPushCount++;
            GameManager.Instance.is_PushPush[index] = true;
            PushPush.Instance.countPushPush -= 1;
        }
        else if (tag == "SpinSpin")
        {
            dollIcon.sprite = Doll.Instance.atlasSpinSpin.GetSprite("img_Doll_SpinSpin" + (index + 1));
            if (GameManager.Instance.is_SpinSpin[index] == false)
                GameManager.Instance.acquiredSpinSpinCount++;
            GameManager.Instance.is_SpinSpin[index] = true;
            SpinSpin.Instance.countSpinSpin -= 1;
        }

        GameManager.Instance.Save();
        AudioManager.Instance.AudioDollGet();

        Destroy(doll);

        StartCoroutine(DollCountCheckNextFrame(tag));
    }

    private IEnumerator DollCountCheckNextFrame(string tag)
    {
        yield return null;

        if (tag == "CatchCatch")
            CatchCatch.Instance.DollCountCheck();
        else if (tag == "PushPush")
            PushPush.Instance.DollCountCheck();
        else if (tag == "SpinSpin")
            SpinSpin.Instance.DollCountCheck();
    }
}

public class AlreadyCollected : MonoBehaviour { }