using System.Collections;
using UnityEngine;
using TMPro;
using ShimmeringUnity;

public class GuidedBreathing : MonoBehaviour
{
    public GameObject statsPopup;
    private TMP_Text meditationStats;
    private TMP_Text closeMessage;

    private AudioSource audioSource;

    private bool narrationStarted = false; // Flag to track if narration has started
    private float countdown = 60f; // Countdown timer from 60 seconds

    private float narrationStartTime; // To keep track of when the narration starts
    private float narrationEndTime; // To calculate total time elapsed

    private ShimmerHeartRateMonitor heartRateMonitor;

    private void Start()
    {
        meditationStats = statsPopup.transform.Find("MeditationStats").GetComponent<TMP_Text>();
        closeMessage = statsPopup.transform.Find("CloseMessage").GetComponent<TMP_Text>();

        // Stats popup is initally hidden
        statsPopup.SetActive(false);

        // Get an AudioSource component to play the narration
        audioSource = gameObject.GetComponent<AudioSource>();

        // Configure the AudioSource component
        audioSource.playOnAwake = false; // Don't play the audio immediately when the game starts

        heartRateMonitor = GameObject.Find("ShimmerDevice").GetComponent<ShimmerHeartRateMonitor>();
    }

    private void Update()
    {
        // Check if narration has finished
        if (narrationStarted && !audioSource.isPlaying)
        {
            EndNarration();
            narrationStarted = false; // Reset the flag
        }

        if (countdown <= 0)
        {
            statsPopup.SetActive(false);
        }
        else if (statsPopup.activeSelf)
        {
            countdown -= Time.deltaTime;
            closeMessage.text = $"This will automatically close in {Mathf.CeilToInt(countdown)} seconds.";
        }
    }

    public void StartNarration()
    {
        audioSource.Play(); // Play the breathing narration
        narrationStartTime = Time.time; // Record the start time of the narration
        narrationStarted = true; // Indicates that the narration has begun

        HeartRateValues.InitialHeartRate = heartRateMonitor.heartRate;

        statsPopup.SetActive(false);
    }

    public void EndNarration()
    {   
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        narrationEndTime = Time.time; // Mark the end time of the narration

        float totalTimeElapsed = narrationEndTime - narrationStartTime;
        int minutes = Mathf.FloorToInt(totalTimeElapsed / 60);
        int seconds = Mathf.FloorToInt(totalTimeElapsed % 60);

        HeartRateValues.FinalHeartRate = heartRateMonitor.heartRate;

        HeartRateValues.AverageHeartRate = (HeartRateValues.FinalHeartRate + HeartRateValues.InitialHeartRate) / 2 + 1;

        // Show stats popup.
        meditationStats.text = $"Total time elapsed: {minutes}m {seconds}s\n" +
                                $"Calibrated resting heart rate: {HeartRateValues.RestingHeartRate} bpm\n" +
                                $"Initial heart rate: {HeartRateValues.InitialHeartRate} bpm\n" +
                                $"Final heart rate: {HeartRateValues.FinalHeartRate} bpm\n" +
                                $"Average heart rate: {HeartRateValues.AverageHeartRate} bpm";

        statsPopup.SetActive(true);
        countdown = 60; // Reset countdown timer
    }
}