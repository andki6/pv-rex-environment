using System.Collections;
using UnityEngine;
using TMPro;

public class GBNarration : MonoBehaviour
{
    public GameObject statsPopup;
    private TMP_Text meditationStats;
    private TMP_Text closeMessage;

    public AudioClip breathingNarration;
    private AudioSource audioSource;

    private bool narrationOngoing = false;
    private bool narrationStarted = false; // Flag to track if narration has started
    private float countdown = 60f; // Countdown timer from 60 seconds

    private float narrationStartTime; // To keep track of when the narration starts
    private float narrationEndTime; // To calculate total time elapsed

    private void Start()
    {
        meditationStats = statsPopup.transform.Find("meditationStats").GetComponent<TMP_Text>();
        closeMessage = statsPopup.transform.Find("closeMessage").GetComponent<TMP_Text>();

        // Stats popup is initally hidden
        statsPopup.SetActive(false);

        // Get or Add an AudioSource component to play the narration
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource component
        audioSource.clip = breathingNarration;
        audioSource.playOnAwake = false; // Don't play the audio immediately when the game starts
    }

    private void Update()
    {
        // When the user is in the zone and presses "E", start the narration if it's not already playing
        if (narrationOngoing && Input.GetKeyDown(KeyCode.E) && !audioSource.isPlaying)
        {
            StartNarration();
        }

        // If narration is ongoing and the user presses "X", end the narration early if it's currently playing
        if (narrationOngoing && Input.GetKeyDown(KeyCode.X) && audioSource.isPlaying)
        {
            EndNarration();
        }

        // Check if narration has finished
        if (narrationStarted && !audioSource.isPlaying)
        {
            EndNarration();
            narrationStarted = false; // Reset the flag
        }

        // Close popup if "C" is pressed or automatically after 60 seconds
        if (statsPopup.activeSelf && (Input.GetKeyDown(KeyCode.C) || countdown <= 0))
        {
            statsPopup.SetActive(false);
        }
        else if (statsPopup.activeSelf)
        {
            countdown -= Time.deltaTime;
            closeMessage.text = $"This will automatically close in {Mathf.CeilToInt(countdown)} seconds. Press \"C\" to close now.";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the user has entered the zone
        if (other.gameObject.CompareTag("Player")) {
            narrationOngoing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the user has exited the zone
        if (other.gameObject.CompareTag("Player")) {
        narrationOngoing = false;

        // End the narration if it was started but hasn't finished yet
        if (narrationStarted && audioSource.isPlaying) {
            EndNarration();
            narrationStarted = false; // Reset the flag to indicate narration is no longer in progress
        }
    }
    }

    private void StartNarration()
    {
        audioSource.Play(); // Play the breathing narration
        narrationStartTime = Time.time; // Record the start time of the narration
        narrationStarted = true; // Indicates that the narration has begun
    }

    private void EndNarration()
    {   
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        narrationEndTime = Time.time; // Mark the end time of the narration

        float totalTimeElapsed = narrationEndTime - narrationStartTime;
        int minutes = Mathf.FloorToInt(totalTimeElapsed / 60);
        int seconds = Mathf.FloorToInt(totalTimeElapsed % 60);

        // Show stats popup.
         meditationStats.text = $"Total time elapsed: {minutes}m {seconds}s\n" +
                                "Calibrated resting heart rate: 66 bpm\n" +
                                "Initial heart rate: 117 bpm\n" +
                                "Final heart rate: 62 bpm\n" +
                                "Average heart rate: 74 bpm";

        statsPopup.SetActive(true);
        countdown = 60; // Reset countdown timer
    }
}