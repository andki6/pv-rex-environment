using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeditationAdminController))]
public class MeditationAdminControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeditationAdminController script = (MeditationAdminController)target;

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

        // Button that toggles the isStarted state
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

            if (!script.isStarted)
            {
                GameObject environment = GameObject.Find("Environment");

                if (environment != null)
                {
                    AdaptiveEnvironmentController controller = environment.GetComponent<AdaptiveEnvironmentController>();

                    if (controller != null)
                    {
                        script.StartCoroutine(controller.StartAnimations());
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
            }

            script.isStarted = !script.isStarted;
        }

        DisplayHeartRate();

        // Save changes made by the editor
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }

    private void DisplayHeartRate()
    {
        GameObject shimmerObject = GameObject.Find("Shimmer");
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

                EditorGUILayout.LabelField("Heart Rate: " + heartRateMonitor.HeartRate, heartRateTextStyle, GUILayout.Height(30));

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Resting Heart Rate: " + HeartRateValues.RestingHeartRate, heartRateTextStyle, GUILayout.Height(30));

                GUILayout.Space(10);

                if (GUILayout.Button("Recalibrate resting HR", GUILayout.Height(40)))
                {
                    HeartRateValues.RestingHeartRate = heartRateMonitor.HeartRate;

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
