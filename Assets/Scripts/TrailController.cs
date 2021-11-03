using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject trail;
    public Bird targetBird;

    private List<GameObject> _trails;

    // Start is called before the first frame update
    void Start()
    {
        _trails = new List<GameObject>();
    }

    public void SetBird(Bird bird)
    {
        targetBird = bird;

        for (int i = 0; i < _trails.Count; i++)
        {
            Destroy(_trails[i].gameObject);
        }

        _trails.Clear();
    }

    public IEnumerator SpawnTrail()
    {
        _trails.Add(Instantiate(trail, targetBird.transform.position, Quaternion.identity));
        yield return new WaitForSeconds(0.1f);

        if (targetBird != null && targetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }
}
