using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowClaw : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (CatchCatch.Instance.isPlayState == true && CatchCatch.Instance.isCatch == true)
        {
            other.transform.parent.transform.SetParent(transform);
            other.transform.parent.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}