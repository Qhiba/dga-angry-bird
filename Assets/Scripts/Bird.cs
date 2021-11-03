using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething};
    public Rigidbody2D rigidbody;
    public CircleCollider2D collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return _state; } }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
        _state = BirdState.Idle;
    }

    void FixedUpdate()
    {
        if (_state == BirdState.Idle && rigidbody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) && rigidbody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            //Hancurkan gameObject setelah 2 detik;
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        collider.enabled = true;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.velocity = velocity * distance * speed;
        OnBirdShot(this);
    }

    private void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();

        Debug.Log("On Bird: " + OnBirdDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
    }

    public virtual void OnTap()
    {
        //Do nothing
    }
}
