using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Countdown : MonoBehaviour
{
    public Button startButton;
    public TMP_Text infoText;
    private int countdownValue = 10;
    private List<float> baselineReadings;
    private bool isBaselineEstablished = false;
    private float baseline = 0;

    SerialPort dataStream = new SerialPort("COM3", 9600);
    private string receivedString;

    void Start()
    {
        startButton.onClick.AddListener(TaskOnClick);
        OpenSerialPort();
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
        StartCoroutine(ContinuousReadings());
    }

    IEnumerator ContinuousReadings()
    {
        while (true)
        {
            float currentReading = GetReadingFromArduino();
            if (currentReading != -1) // Check if reading is valid
            {
                infoText.text = "Current Reading: " + currentReading.ToString("F2") + " | Baseline: " + baseline.ToString("F2");
            }
            yield return new WaitForSeconds(1);
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
                Debug.LogError("Error reading from serial port: " + e.Message);
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
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (dataStream != null && dataStream.IsOpen)
        {
            dataStream.Close();
        }
    }
}
	