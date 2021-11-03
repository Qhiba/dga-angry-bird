using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D collider;
    public LineRenderer trajectory;

    private Vector2 _startPos;

    [SerializeField] private float _radius = 0.75f;
    [SerializeField] private float _throwSpeed = 30f;

    private Bird _bird;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    private void OnMouseUp()
    {
        collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        //Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;

        trajectory.enabled = false;
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        collider.enabled = true;
    }

    private void OnMouseDrag()
    {
        //Mengubah posisi mouse ke world position
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = pos - _startPos;
        if (dir.sqrMagnitude > _radius)
        {
            dir = dir.normalized * _radius;

        }
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);
        if (!trajectory.enabled)
        {
            trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    private void DisplayTrajectory(float distance)
    {
        if (_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        //Posisi awal trajectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        //Velocity awal
        Vector2 segVelocity = velocity * distance * _throwSpeed;

        for (int i = 0; i < segmentCount; i++)
        {
            float elapseTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapseTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapseTime, 2);
        }

        trajectory.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            trajectory.SetPosition(i, segments[i]);
        }
    }
}
