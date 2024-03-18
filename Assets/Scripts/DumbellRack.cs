using UnityEngine;
using UnityEngine.UI;

public class DumbbellRack : MonoBehaviour
{
    public Text exercisePromptText;
    public string exercisePromptMessage = "Would you like to do some exercises? Press E to begin.";

    private bool playerNearby = false;

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
            Debug.Log("Starting exercise...");
        }
    }
}
