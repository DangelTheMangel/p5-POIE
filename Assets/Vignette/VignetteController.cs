using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine.UI;
using TMPro;

public class VignetteController : MonoBehaviour
{
    public Button startButton;
    public TMP_Text infoText;
    public TMP_Text readingTXT;
    private int countdownValue = 10;
    private List<float> baselineReadings;
    private bool isBaselineEstablished = false;
    private float baseline = 0;
    float currentReading;
    float nauseaReading = 0;
    bool isNausies = false;
    private float desiredDuration = 10f;
    private float elapsedTime;
    float VignetteStart = 1;
    float VignetteEnd = 0.6f;
    float percentageComplete = 0;
    [SerializeField]
    private float maxNausisProcenticy = 200;

    SerialPort dataStream = new SerialPort("COM3", 9600);
    private string receivedString;

    public MeshRenderer meshRenderer;

    // A public variable to control the vignette size
    [Range(0f, 1f)]
    public float vignetteSize;

    void Start()
    {
        startButton.onClick.AddListener(TaskOnClick);
        OpenSerialPort();
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        percentageComplete = elapsedTime / desiredDuration;
        ContinuousReadings();
        ProcentageCalculator();
        if (baseline > 0)
        {
            VignetteActivator();
        }

    }

    void TaskOnClick()
    {
        if (!isBaselineEstablished)
        {
            baselineReadings = new List<float>();
            StartCoroutine(StartBaselineCalculation());
        }
    }

    IEnumerator StartBaselineCalculation()
    {
        while (countdownValue > 0)
        {
            infoText.text = "Calculating baseline: " + countdownValue;
            float reading = GetReadingFromArduino();
            if (reading != -1) // Check if reading is valid
            {
                baselineReadings.Add(reading);
            }

            yield return new WaitForSeconds(1);
            countdownValue--;
        }

        baseline = CalculateAverage(baselineReadings);
        isBaselineEstablished = true;
        infoText.text = "Baseline established: " + baseline.ToString("F2");
        countdownValue = 10; // Reset the countdown for potential future use

    }

    void ContinuousReadings()
    {


        currentReading = GetReadingFromArduino();
        if (currentReading != -1) // Check if reading is valid
        {
            readingTXT.text = "Current Reading: " + currentReading.ToString("F2") + " | Baseline: " + baseline.ToString("F2");
            //Debug.Log("Current Reading from Arduino: " + currentReading);
        }

    }

    float GetReadingFromArduino()
    {
        if (dataStream.IsOpen)
        {
            try
            {
                receivedString = dataStream.ReadLine();
                float reading = float.Parse(receivedString); // Assuming data is a single float value
                return reading;
            }
            catch (System.Exception e)
            {
                //Debug.LogError("Error reading from serial port: " + e.Message);
                return -1; // Return -1 to indicate an invalid reading
            }
        }
        return -1; // Return -1 if data stream is not open
    }

    float CalculateAverage(List<float> readings)
    {
        float sum = 0;
        foreach (float reading in readings)
        {
            sum += reading;
        }
        return readings.Count > 0 ? sum / readings.Count : 0;
    }

    void OpenSerialPort()
    {
        try
        {
            dataStream.Open();
        }
        catch (System.Exception e)
        {
            //Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (dataStream != null && dataStream.IsOpen)
        {
            dataStream.Close();
        }
    }

    void ProcentageCalculator()
    {
        nauseaReading = (baseline / currentReading);
        //Debug.Log("nauseaReading is " + nauseaReading);
    }

    void VignetteActivator()
    {
        Mathf.Clamp(nauseaReading, 0.3f, 1);

        if (nauseaReading > 0.8f) 
        {
            meshRenderer.material.SetFloat("_ApertureSize", 1);
        }
        else 
        {
            VignetteStart = Mathf.Lerp(VignetteStart, nauseaReading, 0.1f);
            VignetteMat(VignetteStart);
        }
    }

    void VignetteMat(float x)
    {
        meshRenderer.material.SetFloat("_ApertureSize", x);
    }
}
