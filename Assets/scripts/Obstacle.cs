using System.Collections;

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Rigidbody2D rb;

    [Header("size")]
    public float minSize = 0.5f;
    public float maxSize = 2.0f;

    [Header("Hit Points by size")]
    public int minHits = 1;
    public int maxHits = 1;
    private int hitsRemaining;

    [Header("Movement")]
    public float maxSpinSpeed = 10f;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    [Header("draft Correction")]
    public float impulseMin = 1.5f;
    public float impulseMax = 3.5f;
    public float impulseStrength = 15f;

    [Header("FX")]
    public GameObject bounceEffectPrefab;

    private void Start()
    {
       SetupAsteroid();

       StartCoroutine(draftImpulse());
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

   
    private void SetupAsteroid()
        {
            float randomSize = Random.Range(minSize, maxSize);
            transform.localScale = new Vector3 (randomSize, randomSize, 1f);

            hitsRemaining = HitsFromSize(randomSize);
                

            Vector2 randomDirection = Random.insideUnitCircle;
            float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
            rb.AddForce(randomDirection * randomSpeed);

            float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
            rb.AddTorque(randomTorque);
        }

    private int HitsFromSize(float size)
    {
        float t = Mathf.InverseLerp(minSize, maxSize, size);

        int hits = Mathf.RoundToInt(Mathf.Lerp(minHits, maxHits, t));
        return Mathf.Clamp(hits, minHits, maxHits);
    }

    public void TakeHit(int damage = 1)
    {
        hitsRemaining -= damage;
        Debug.Log("Hits remaining: " + hitsRemaining);

        if (hitsRemaining <= 0)
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(10);
            }
            Destroy(gameObject);
        }
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bounceEffectPrefab) return;

        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
        Destroy(bounceEffect, 1f);
    }

    

    private IEnumerator draftImpulse()
    {
        while(true)
        {
            float waitTime = Random.Range(impulseMin, impulseMax);
            yield return new WaitForSeconds(waitTime);

            Vector2 drift = Random.insideUnitCircle.normalized * impulseStrength;
            rb.AddForce(drift, ForceMode2D.Impulse);
        }
    }
}

