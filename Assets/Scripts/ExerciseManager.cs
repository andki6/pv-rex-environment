using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ExerciseManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 spawnLocation;
    public TextMeshProUGUI exercisePromptText;
    public TextMeshProUGUI countdownText;
    public Slider progressBar; 
    public TextMeshProUGUI exerciseText; 
    public TextMeshProUGUI completeText;
    public string exercisePromptMessage = "Would you like to do some exercises? Press E to begin.";

    private bool playerNearby = false;

    private void Start()
    {
        progressBar.gameObject.SetActive(false);
        completeText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false); 
        exercisePromptText.gameObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
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
        exercisePromptText.gameObject.SetActive(false);

        player.transform.position = spawnLocation;

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
        float duration = 20f; 
        while (time < duration)
        {
            time += Time.deltaTime;
            progressBar.value = time / duration;
            exerciseText.text = $"First Exercise: Bicep Curls\nTime Remaining: {(int)(duration - time)}s";
            yield return null;
        }

        progressBar.gameObject.SetActive(false);
        exerciseText.gameObject.SetActive(false);
        completeText.gameObject.SetActive(true);
        completeText.text = "Exercise Complete!";
        yield return new WaitForSeconds(2f); 
        completeText.gameObject.SetActive(false);
        exercisePromptText.gameObject.SetActive(false);
    }
}
