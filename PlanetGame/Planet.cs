using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using UnityEditor;

public class Planet : MonoBehaviour
{
    public IObjectPool<GameObject> pool { get; set; }

    public Rigidbody rb;

    public bool isSelected = true;
    public bool isMerged = false;  

    void OnCollisionEnter(Collision collision)
    {
        rb.angularDrag = 2.0f;

        if (gameObject.tag != collision.gameObject.tag)
            return;

        Planet collidedPlanet = collision.gameObject.GetComponent<Planet>();

        if (isMerged || collidedPlanet.isMerged)
            return;

        if (GetInstanceID() > collidedPlanet.GetInstanceID())
            return;

        Vector3 collisionPoint = collision.contacts[0].point;

        isMerged = true;
        collidedPlanet.isMerged = true;

        if (tag == "Moon")
        {
            PlanetManager.Instance.MergePlanet(1, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 1;
        }
        else if (tag == "Mercury")
        {
            PlanetManager.Instance.MergePlanet(2, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 3;
        }
        else if (tag == "Mars")
        {
            PlanetManager.Instance.MergePlanet(3, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 6;
        }
        else if (tag == "Venus")
        {
            PlanetManager.Instance.MergePlanet(4, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 10;
        }
        else if (tag == "Earth")
        {
            PlanetManager.Instance.MergePlanet(5, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 15;
        }
        else if (tag == "Neptune")
        {
            PlanetManager.Instance.MergePlanet(6, collisionPoint, gameObject, collision.gameObject);

            GameManager.Instance.score += 21;
        }
        else if (tag == "Uranus")
        {
            PlanetManager.Instance.MergePlanet(7, collisionPoint, gameObject, collision.gameObject);

            GameManager.Instance.score += 28;
        }
        else if (tag == "Saturn")
        {
            PlanetManager.Instance.MergePlanet(8, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 36;
        }
        else if (tag == "Jupiter")
        {
            PlanetManager.Instance.MergePlanet(9, collisionPoint, gameObject, collision.gameObject);
            GameManager.Instance.score += 45;
        }

        SoundManager.Instance.PlayMergeSound();
        GameManager.Instance.tScore.text = GameManager.Instance.score.ToString();
    }
}