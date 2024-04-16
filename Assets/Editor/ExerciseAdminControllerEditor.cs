using System;
using ShimmeringUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ExerciseAdminController))]
public class ExerciseAdminControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ExerciseAdminController script = (ExerciseAdminController)target;

        DisplayStatusText(script);

        StartStopButton(script);

        DisplayHeartRate();

        EditorUtility.SetDirty(script);
    }

    private void DisplayStatusText(ExerciseAdminController script)
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

    private void LoadMeditationSceneButton()
    {
        //This function is broken... the lighting doesn't load correctly so this button was removed.
        if (GUILayout.Button("Switch to meditation scene", GUILayout.Height(40)))
        {
            // Ensure this operation is only available during Play mode
            if (Application.isPlaying)
            {
                // Load the meditation scene
                SceneManager.LoadScene("Assets/Scenes/meditation-scene.unity");
            }
            else
            {
                Debug.LogWarning("Scene switching is only available in Play Mode.");
            }
        }
    }

    private void StartStopButton(ExerciseAdminController script)
    {
        if (GUILayout.Button("Start/Stop", GUILayout.Height(40)) && Application.isPlaying) // Ensure this operation is only available during Play mode
        {
            GameObject environment = GameObject.Find("Terrain");

            if (environment != null)
            {
                AdaptiveEnvironmentController controller = environment.GetComponent<AdaptiveEnvironmentController>();

                if (controller != null)
                {
                    if (!script.isStarted)
                    {
                        script.StartCoroutine(controller.StartAnimations());
                        GameObject dumbbellRack = GameObject.Find("DumbbellRack");
                        if (dumbbellRack != null)
                        {
                            ExerciseManager exerciseManager = dumbbellRack.GetComponent<ExerciseManager>();
                            if (exerciseManager != null && Application.isPlaying)
                            {
                                script.StartCoroutine(exerciseManager.StartExerciseRoutine());
                            }
                            else
                            {
                                Debug.LogError("ExerciseManager component not found on DumbbellRack.");
                            }
                        }
                        else
                        {
                            Debug.LogError("DumbbellRack GameObject not found.");
                        }
                    }
                    else
                    {
                        GameObject dumbbellRack = GameObject.Find("DumbbellRack");
                        ExerciseManager exerciseManager = dumbbellRack.GetComponent<ExerciseManager>();
                        if (exerciseManager != null && Application.isPlaying)
                        {
                            exerciseManager.StopExerciseRoutine();
                        }
                        else
                        {
                            Debug.LogError("ExerciseManager component not found on DumbbellRack.");
                        }

                        controller.StopAllCoroutines();
                        script.StartCoroutine(controller.InitializeAnimations());
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
