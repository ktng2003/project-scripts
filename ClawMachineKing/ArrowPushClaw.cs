using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPushClaw : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.transform.parent.GetChild(1).gameObject.SetActive(false);
        other.transform.parent.GetChild(2).gameObject.SetActive(false);
    }
}