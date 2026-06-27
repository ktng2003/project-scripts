using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public float minStartDistance = 50.0f;
    public float maxStartDistance = 125.0f;
    public float minTargetDistance = 100.0f;
    public float maxTargetDistance = 250.0f;
    public float moveSpeed = 50.0f;
    public float minWaitTime = 15.0f;
    public float maxWaitTime = 30.0f;

    private TrailRenderer trail;

    private float maxScale = 0.05f;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        StartCoroutine(ShootingStarMove());
    }

    private IEnumerator ShootingStarMove()
    {
        while (true)
        {
            Vector3 startDirection = Random.onUnitSphere;
            float startDistance = Random.Range(minStartDistance, maxStartDistance);
            Vector3 spawnPosition = GameManager.Instance.blackHole.position + startDirection * startDistance;
            transform.position = spawnPosition;

            Vector3 startScale = Vector3.one * maxScale;
            Vector3 endScale = Vector3.zero;
            transform.localScale = startScale;

            trail.Clear();

            Vector3 targetDirection = Random.onUnitSphere;
            float targetDistance = Random.Range(minTargetDistance, maxTargetDistance);
            Vector3 targetPosition = spawnPosition + targetDirection * targetDistance;

            float travelDistance = Vector3.Distance(spawnPosition, targetPosition);
            float currentDistance = travelDistance;

            while (currentDistance > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                currentDistance = Vector3.Distance(transform.position, targetPosition);

                float t = 1 - (currentDistance / travelDistance);
                transform.localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }

            transform.position = targetPosition;
            transform.localScale = Vector3.zero;

            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}