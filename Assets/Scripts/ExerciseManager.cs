using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ExerciseManager : MonoBehaviour
{
    public GameObject statsPopup;
    private TMP_Text meditationStats;

    public GameObject player;
    public Vector3 spawnLocation;
    public TextMeshProUGUI exercisePromptText;
    public TextMeshProUGUI countdownText;
    public Slider progressBar; 
    public TextMeshProUGUI exerciseText; 
    public string exercisePromptMessage = ""; //disabled because exercises are started from the admincontrols inspector menu

    private ShimmerHeartRateMonitor heartRateMonitor;

    private bool playerNearby = false;

    private void Start()
    {
        meditationStats = statsPopup.transform.Find("MeditationStats").GetComponent<TMP_Text>();

        // Stats popup is initally hidden
        statsPopup.SetActive(false);

        progressBar.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false); 
        exercisePromptText.gameObject.SetActive(false);

        heartRateMonitor = GameObject.Find("ShimmerDevice").GetComponent<ShimmerHeartRateMonitor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false; //disabled
            exercisePromptText.text = exercisePromptMessage;
            exercisePromptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            exercisePromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartExerciseRoutine());
        }
    }

    public IEnumerator StartExerciseRoutine()
    {
        statsPopup.SetActive(false);
        exercisePromptText.gameObject.SetActive(false);

        HeartRateValues.InitialHeartRate = heartRateMonitor.HeartRate;

        //player.transform.position = spawnLocation;

        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.gameObject.SetActive(false);
        exerciseText.gameObject.SetActive(true);
        exerciseText.text = "First Exercise: Bicep Curls";
        progressBar.gameObject.SetActive(true);
        float time = 0;
        float duration = 45f; 
        while (time < duration)
        {
            time += Time.deltaTime;
            progressBar.value = time / duration;
            exerciseText.text = $"First Exercise: Bicep Curls\nTime Remaining: {(int)(duration - time)}s";
            yield return null;
        }

        HeartRateValues.FinalHeartRate = heartRateMonitor.HeartRate;

        HeartRateValues.AverageHeartRate = (HeartRateValues.FinalHeartRate + HeartRateValues.InitialHeartRate) / 2;

        progressBar.gameObject.SetActive(false);
        exerciseText.gameObject.SetActive(false);

        meditationStats.text = $"Total time elapsed: {duration}s\n" +
                                $"Calibrated resting heart rate: {HeartRateValues.RestingHeartRate} bpm\n" +
                                $"Initial heart rate: {HeartRateValues.InitialHeartRate} bpm\n" +
                                $"Final heart rate: {HeartRateValues.FinalHeartRate} bpm\n" +
                                $"Average heart rate: {HeartRateValues.AverageHeartRate} bpm";

        statsPopup.SetActive(true);

        exercisePromptText.gameObject.SetActive(false);
    }

    public void StopExerciseRoutine()
    {
        exercisePromptText.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        exerciseText.gameObject.SetActive(false);
        statsPopup.SetActive(false);
    }
}
