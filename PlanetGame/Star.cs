using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Star : MonoBehaviour
{
    public float minDistance = 50.0f;
    public float MaxDistance = 125.0f;
    public float referenceTime = 5.0f;
    public float maxWaitTime = 3.0f;

    private float initialScale;
    private float maxScale = 0.1f;
    private float elapsedTimer = 0f;

    void Start()
    {
        initialScale = Random.Range(0f, maxScale);
        transform.localScale = Vector3.one * initialScale;
        elapsedTimer = (initialScale / maxScale) * referenceTime;

        StartCoroutine(TwinkleStar());
    }

    private IEnumerator TwinkleStar()
    {
        while (true)
        {
            float spawnRange = Random.Range(minDistance, MaxDistance);
            Vector3 randomDirection = Random.onUnitSphere;
            Vector3 spawnPosition = GameManager.Instance.blackHole.position + randomDirection * spawnRange;
            transform.position = spawnPosition;

            while (elapsedTimer < referenceTime)
            {
                float scaleUp = Mathf.Lerp(initialScale, maxScale, elapsedTimer / referenceTime);
                transform.localScale = Vector3.one * scaleUp;

                elapsedTimer += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one * maxScale;

            elapsedTimer = 0f;
            while (elapsedTimer < referenceTime)
            {
                float scaleDown = Mathf.Lerp(maxScale, 0f, elapsedTimer / referenceTime);
                transform.localScale = Vector3.one * scaleDown;

                elapsedTimer += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.zero;
            elapsedTimer = 0f;

            float waitTime = Random.Range(0f, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}