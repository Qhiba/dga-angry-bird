using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public TrailController trailController;
    public List<Bird> birds;
    public List<Enemy> enemies;

    private Bird _shotBird;
    public BoxCollider2D tapCollider;

    [Header("UI")]
    public GameObject gameOverPanel;
    public Text title;

    private bool _isGameEnded = false;
    private bool isWon;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);

        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdDestroyed += ChangeBird;
            birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        tapCollider.enabled = false;
        slingShooter.InitiateBird(birds[0]);
        _shotBird = birds[0];
    }

    private void Update()
    {
        if (_isGameEnded == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void ChangeBird()
    {
        tapCollider.enabled = false;

        if (_isGameEnded)
        {
            return;
        }

        birds.RemoveAt(0);

        if (birds.Count > 0)
        {
            slingShooter.InitiateBird(birds[0]);
            _shotBird = birds[0];
        }

        if (birds.Count == 0)
        {
            _isGameEnded = true;
            isWon = false;
            ShowGameOverUI(isWon);
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject == destroyedEnemy)
            {
                enemies.RemoveAt(i);
                break;
            }
        }

        if (enemies.Count == 0)
        {
            _isGameEnded = true;
            isWon = true;
            ShowGameOverUI(isWon);
        }
    }

    public void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
        tapCollider.enabled = true;
    }

    private void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    private void ShowGameOverUI(bool isWon)
    {
        gameOverPanel.SetActive(true);

        if (isWon)
        {
            title.text = "You Win!";
        }
        else
        {
            title.text = "You Lose!";
        }
    }
}
