using JetBrains.Annotations;
using nickmaltbie.ScrollingShader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TomarIsAHillbilly : MonoBehaviour
{
    // A reference to the other component
    public AsteroidSpawner asteroidspawner;
    public ConveyerBelt ConveyerBelt;

    public float Zackie = 0;
    public GameObject startButton;
    public GameObject endButton;
    public GameObject spawner;
    public TMP_Text timerText;
    public TMP_Text score;
    public TMP_Text Announcer;
    private float countdownTime = 0;
    public float messageInterval = 2f;
    bool isRunning = false;
    bool gamehaps = false;
    private string[] messages = {
        "Welcome.",
        "Hit the rocks.",
        "Deposit gems.",
        "Good Luck!"
        // Add more messages as needed
    };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gamehaps && countdownTime > 0)
        {
            countdownTime -= Time.deltaTime; // Countdown by deltaTime (time since the last frame)
            DisplayTime(countdownTime);
        }
        else
        {
            timerText.text = "Press the blue Button to exit and red to play agian";
            countdownTime = 0; // To prevent displaying negative values
            gameEnd();
            endButton.SetActive(true);
            // Optionally, you can trigger an action when the countdown reaches zero
            // For example: TimerEnded();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a tag or specific characteristics (in this case, Object B)
        if (collision.gameObject.CompareTag("LyleBurt"))
        {
            Zackie += 1;
            score.text = "" + Zackie;
            Debug.Log("collected Gem");
            // Destroy the collided object (Object B)
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Racist"))
        {
            Destroy(collision.gameObject);
        }
    }

    public void startAsteroidgame()
    {
        introduction();
        startButton.SetActive(false);
    }

    void introduction()
    {
        StartCoroutine(ShowMessages());
        StartCoroutine(GameStarter());
    }
   
    void gameStart()
    {
        score.text = Zackie.ToString();
        timerText.text = countdownTime.ToString();
        spawner.SetActive(true);
        countdownTime = 60f;
        gamehaps = true;
        difficultyOne();
        difficultyTwo();
        difficultyThree();

    }
    void gameEnd()
    {
        spawner.SetActive(false);
        startButton.SetActive(true);
    }

    void DisplayTime(float timeToDisplay)
    {
        // Convert timeToDisplay to minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the UI Text element to display the timer in MM:SS format
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator ShowMessages()
    {
        foreach (string message in messages)
        {
            Announcer.text = message;
            yield return new WaitForSeconds(messageInterval);
        }
    }

    IEnumerator GameStarter()
    {
        isRunning = true;

        // Your initial function logic goes here
        Debug.Log("FirstFunction started.");

        yield return new WaitForSeconds(9f); // Wait for 9 seconds

        if (isRunning)
        {
            gameStart(); // Activate the second function
        }
    }

    IEnumerator difficultyOne()
    {
        isRunning = true;

        // Your initial function logic goes here
        Debug.Log("FirstFunction started.");

        yield return new WaitForSeconds(0.1f); // Wait for x seconds

        if (isRunning)
        {
            asteroidspawner.spawnInterval = 2f;
            ConveyerBelt.velocity = 1f;    // Activate the second function
        }
    }

    IEnumerator difficultyTwo()
    {
        isRunning = true;

        // Your initial function logic goes here
        Debug.Log("FirstFunction started.");

        yield return new WaitForSeconds(30f); // Wait for x seconds

        if (isRunning)
        {
            asteroidspawner.spawnInterval = 1.5f;
            ConveyerBelt.velocity = 2f;    // Activate the second function
        }
    }

    IEnumerator difficultyThree()
    {
        isRunning = true;

        // Your initial function logic goes here
        Debug.Log("FirstFunction started.");

        yield return new WaitForSeconds(45f); // Wait for x seconds

        if (isRunning)
        {
            asteroidspawner.spawnInterval = 1f;
            ConveyerBelt.velocity = 3f;    // Activate the second function
        }
    }

}
