using ShimmeringUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MeditationAdminController))]
public class MeditationAdminControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeditationAdminController script = (MeditationAdminController)target;

        DisplayStatusText(script);

        StartStopButton(script);

        DisplayHeartRate();

        LoadExerciseSceneButton();

        EditorUtility.SetDirty(script);
    }

    private void DisplayStatusText(MeditationAdminController script)
    {
        // Style for the status text
        GUIStyle statusTextStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter
        };

        statusTextStyle.normal.textColor = script.isStarted ? Color.green : Color.red;
        string statusText = script.isStarted ? "Started" : "Stopped";

        GUILayout.Space(10);

        EditorGUILayout.LabelField(statusText, statusTextStyle, GUILayout.Height(30));
    }

    private void LoadExerciseSceneButton()
    {
        if (GUILayout.Button("Switch to exercise scene", GUILayout.Height(40)))
        {
            // Ensure this operation is only available during Play mode
            if (Application.isPlaying)
            {
                // Load the exercise scene
                SceneManager.LoadScene("Assets/Scenes/exercise-scene.unity");
            }
            else
            {
                Debug.LogWarning("Scene switching is only available in Play Mode.");
            }
        }
    }

    private void StartStopButton(MeditationAdminController script)
    {
        if (GUILayout.Button("Start/Stop", GUILayout.Height(40)) && Application.isPlaying) // Ensure this operation is only available during Play mode
        {
            GameObject firstPersonController = GameObject.Find("FirstPersonController");

            if (firstPersonController != null)
            {
                GuidedBreathing guidedBreathing = firstPersonController.GetComponent<GuidedBreathing>();

                if (guidedBreathing != null)
                {
                    if (script.isStarted)
                    {
                        guidedBreathing.EndNarration();
                    }
                    else
                    {
                        guidedBreathing.StartNarration();
                    }
                }
                else
                {
                    Debug.LogError("GuidedBreathing component not found on FirstPersonController.");
                }
            }
            else
            {
                Debug.LogError("FirstPersonController GameObject not found.");
            }


            GameObject environment = GameObject.Find("Environment");

            if (environment != null)
            {
                AdaptiveEnvironmentController controller = environment.GetComponent<AdaptiveEnvironmentController>();

                if (controller != null)
                {
                    if (!script.isStarted)
                    {
                        script.StartCoroutine(controller.StartAnimations());
                    }
                    else
                    {
                        controller.StopAllCoroutines();
                    }
                }
                else
                {
                    Debug.LogError("AdaptiveEnvironmentController component not found on Environment.");
                }
            }
            else
            {
                Debug.LogError("Environment GameObject not found.");
            }


            script.isStarted = !script.isStarted;
        }
    }

    private void DisplayHeartRate()
    {
        GameObject shimmerObject = GameObject.Find("ShimmerDevice");
        if (shimmerObject != null)
        {
            ShimmerHeartRateMonitor heartRateMonitor = shimmerObject.GetComponent<ShimmerHeartRateMonitor>();
            if (heartRateMonitor != null)
            {
                GUIStyle heartRateTextStyle = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 24,
                    alignment = TextAnchor.MiddleCenter
                };

                heartRateTextStyle.normal.textColor = Color.cyan;

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Heart Rate: " + heartRateMonitor.heartRate, heartRateTextStyle, GUILayout.Height(30));

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Resting Heart Rate: " + HeartRateValues.RestingHeartRate, heartRateTextStyle, GUILayout.Height(30));

                GUILayout.Space(10);

                if (GUILayout.Button("Recalibrate resting HR", GUILayout.Height(40)))
                {
                    HeartRateValues.RestingHeartRate = heartRateMonitor.heartRate;

                    EditorUtility.SetDirty(heartRateMonitor);
                }
            }
            else
            {
                EditorGUILayout.LabelField("ShimmerHeartRateMonitor component not found.", GUILayout.Height(20));
            }
        }
        else
        {
            EditorGUILayout.LabelField("\"Shimmer\" GameObject not found.", GUILayout.Height(20));
        }
    }

}
