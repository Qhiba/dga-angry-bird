using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    private void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (collision.gameObject.tag == "Bird")
        {
            _isHit = true;
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Obstacle")
        {
            //Hitung damage yang diperoleh
            float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            health -= damage;

            if (health <= 0)
            {
                _isHit = true;
                Destroy(gameObject);
            }
        }
    }
}
