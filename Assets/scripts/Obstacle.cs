using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Rigidbody2D rb;

    [Header("draft Correction")]
    public float impulseMin = 1.5f;
    public float impulseMax = 3.5f;
    public float impulseStrength = 15f;

    public GameObject bounceEffectPrefab;
    public float maxSpinSpeed = 10f;
    public float minSize = 0.5f;

    public float maxSize = 2.0f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    private void Start()
    {
       astroid();

       draftImpulse();
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);

        //Destiry he effect after 1 second
        Destroy(bounceEffect, 1f);
    }

    private void astroid()
        {
            float randomSize = Random.Range(minSize, maxSize);
                transform.localScale = new Vector3 (randomSize, randomSize, 1);

                rb = GetComponent <Rigidbody2D>();
                rb.AddForce(Vector2.right * 100);

                

                Vector2 randomDirection = Random.insideUnitCircle;

                float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
                rb.AddForce(randomDirection * randomSpeed);

                float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
                rb.AddTorque(randomTorque);
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

