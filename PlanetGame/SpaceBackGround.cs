using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBackGround : MonoBehaviour
{
    public GameObject preStar;
    public GameObject preShootingStar;

    public Transform stars;
    public Transform shootingStars;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject star = Instantiate(preStar, stars);
        }

        StartCoroutine(SpawnShootingStars());
    }

    IEnumerator SpawnShootingStars()
    {
        yield return new WaitForSeconds(10.0f);
        GameObject shootingStar = Instantiate(preShootingStar, shootingStars);

        yield return new WaitForSeconds(20.0f);
        shootingStar = Instantiate(preShootingStar, shootingStars);
    }
}