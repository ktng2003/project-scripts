using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotDoll : MonoBehaviour
{
    public Button btSlot;

    public Image dollIcon;

    private void OnEnable()
    {
        if (gameObject.tag == "CatchCatch")
        {
            if (GameManager.Instance.is_CatchCatch[int.Parse(this.gameObject.name)] == true)
            {
                btSlot.interactable = true;
                dollIcon.color = new Color(255f, 255f, 255f);
            }
        }
        else if (gameObject.tag == "PushPush")
        {
            if (GameManager.Instance.is_PushPush[int.Parse(this.gameObject.name)] == true)
            {
                btSlot.interactable = true;
                dollIcon.color = new Color(255f, 255f, 255f);
            }
        }
        else if (gameObject.tag == "SpinSpin")
        {
            if (GameManager.Instance.is_SpinSpin[int.Parse(gameObject.name)] == true)
            {
                btSlot.interactable = true;
                dollIcon.color = new Color(255f, 255f, 255f);
            }
        }
    }

    public void OnDoll()
    {
        if (gameObject.tag == "CatchCatch")
        {
            DollInformation.Instance.dollInfo_CatchCatch[int.Parse(gameObject.name)].SetActive(true);
            GameManager.Instance.dollCatchCatch = int.Parse(gameObject.name);
        }
        else if (gameObject.tag == "PushPush")
        {
            DollInformation.Instance.dollInfo_PushPush[int.Parse(gameObject.name)].SetActive(true);
            GameManager.Instance.dollPushPush = int.Parse(gameObject.name);
        }
        else if (gameObject.tag == "SpinSpin")
        {
            DollInformation.Instance.dollInfo_SpinSpin[int.Parse(gameObject.name)].SetActive(true);
            GameManager.Instance.dollSprinSpin = int.Parse(gameObject.name);
        }
    }
}