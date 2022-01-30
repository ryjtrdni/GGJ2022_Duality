using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
    public GameObject WinLabel;
    public GameObject LoseLabel;
    private GameplayController gameController;
    private PlayerHealth playerHealth;

    void Start()
    {
        var controllerObj = GameObject.FindWithTag("GameController");
        if (!controllerObj)
        {
            throw new ArgumentNullException("No GameplayController in scene!");
        }

        gameController = controllerObj.GetComponent<GameplayController>();

        var playerObj = GameObject.FindWithTag("Player");
        if (!controllerObj)
        {
            throw new ArgumentNullException("No Player in scene!");
        }
        playerHealth = playerObj.GetComponent<PlayerHealth>();
    }

    private static string LAST_LEVEL = "Level2";

    void Update()
    {
        // WIN
        // If you reach the last level display a score
        if (gameController.CurrentScore >= gameController.ScoreTarget)
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name == LAST_LEVEL)
            {
                WinLabel.SetActive(true);
                StartCoroutine(FinishScreen());
            }
            else
            {
                StartCoroutine(NextLevel());
            }
        }
        else
        {
            // LOSE
            // If player runs out of health or time
            if (playerHealth.currentHealth <= 0 || gameController.CurrentTime <= 0f)
            {
                LoseLabel.SetActive(true);
                gameController.CurrentTime = 0.0f;
                StartCoroutine(RestartLevel());
            }
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2);
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2);
        var scene = SceneManager.GetActiveScene();
        var levelNumberString = Regex.Match(scene.name, @"\d+$").Value;
        var levelNumber = int.Parse(levelNumberString);
        var nextLevel = $"Level{levelNumber + 1}";
        SceneManager.LoadScene(nextLevel);
    }

    private IEnumerator FinishScreen()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Frontend");
    }
}