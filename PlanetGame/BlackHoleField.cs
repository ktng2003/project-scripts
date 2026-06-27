using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlackHoleField : MonoBehaviour
{
    public float force = 3000.0f;

    private float x = 0f;
    private float y = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void FixedUpdate()
    {
       BlackHoleGravity();
    }

    public void BlackHoleGravity()
    {
        int len = transform.childCount;

        if (len > 1)
        {
            for (int i = 1; i < len; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf == true)
                {
                    float dx = GameManager.Instance.blackHole.position.x - transform.GetChild(i).position.x;
                    float dy = GameManager.Instance.blackHole.position.y - transform.GetChild(i).position.y;
                    float dz = GameManager.Instance.blackHole.position.z - transform.GetChild(i).position.z;

                    var distance = dx * dx + dy * dy + dz * dz;
                    var gravity = force * transform.GetChild(i).GetComponent<Rigidbody>().mass / distance;

                    transform.GetChild(i).GetComponent<Rigidbody>().AddForce(gravity * dx, gravity * dy, gravity * dz);
                }
            }
        }
    }
}