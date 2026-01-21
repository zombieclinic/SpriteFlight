using UnityEngine;

public class bullet : MonoBehaviour
{

public int damage = 1;

 private void OnTriggerEnter2D(Collider2D other)
    {
        Obstacle asteroid = other.GetComponent<Obstacle>();
        if(asteroid == null) return;

        asteroid.TakeHit(damage);
        Destroy(gameObject);
         Debug.Log("Bullet trigger hit: " + other.name);
    }
}
