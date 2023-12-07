using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
public class ThrowingMinigame : MonoBehaviour
{

    [SerializeField]
    GameObject timerScreen, EndScreen;
    [SerializeField]
    TMP_Text points;
    [SerializeField]
    TMP_Text timer, EndTime;
    [SerializeField]
    bool gameRunning = false;
    [SerializeField]
    Transform[] TargetPoints;
    [SerializeField]
    Transform[] spawnPostions;
    [SerializeField]
    Transform bagContainers;
    public float pointsValue = 0;
    public float maxPoints = 10;
    public float time = 0;

    [SerializeField]
    GameObject throwingObj;
    public bool isdone = false;

    // Update is called once per frame
    void Update()
    {
        if (gameRunning) {
            time += Time.deltaTime;
            display(time);
        }
    }

    public void startGame() {
        gameRunning = true;
    }

    void display(float timeToDisplay)
    {
        showTime(timer, timeToDisplay);
        points.text = pointsValue + " / " + maxPoints;
    }

    void showTime(TMP_Text t, float ttp) {
        float minutes = Mathf.FloorToInt(ttp / 60);
        float seconds = Mathf.FloorToInt(ttp % 60);
        t.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public Vector3 getRandimPost() {
        return TargetPoints[Random.Range(0, TargetPoints.Length)].position;
    }

    public void addAPoint() {
        if (!isdone)
        {
            pointsValue++;
            if (pointsValue >= maxPoints)
            {
                gameRunning = false;
                timerScreen.SetActive(false);
                EndScreen.SetActive(true);
                showTime(EndTime, time);
                isdone = true;
                bagContainers.gameObject.SetActive(false);
            }
            else
            {
                var obj = Instantiate(throwingObj, spawnPostions[(int)Random.Range(0, spawnPostions.Length)].position, Quaternion.identity);
                obj.GetComponent<ballThrowing>().throwingMinigame = this;
                obj.GetComponent<ballThrowing>().spawnpoint = spawnPostions[(int)Random.Range(0, spawnPostions.Length)];
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.transform.parent = bagContainers;
            }
        }
    }
}
