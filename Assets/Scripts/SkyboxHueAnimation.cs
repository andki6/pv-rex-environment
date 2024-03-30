using UnityEngine;

public class SkyboxHueAnimation : EnvironmentAnimation
{
    public GameObject CameraGameObject; // Assign this in the Inspector
    public Color StartColor = Color.white; // Initial hue color
    public Color EndColor = Color.blue; // Target hue color

    private Material skyboxMaterial;

    public override void Init()
    {
        // Verify if the Camera GameObject is assigned
        if (CameraGameObject == null)
        {
            Debug.LogError("SkyboxHueAnimation: CameraGameObject is not assigned.");
            return;
        }

        // Attempt to access the skybox material
        skyboxMaterial = RenderSettings.skybox;
        if (skyboxMaterial == null)
        {
            Debug.LogError("SkyboxHueAnimation: Failed to access skybox material.");
        }
    }

    public override void UpdateAnimation(float progress)
    {
        if (skyboxMaterial != null)
        {
            // Interpolate the skybox color based on the animation progress
            Color currentColor = Color.Lerp(StartColor, EndColor, progress);
            skyboxMaterial.SetColor("_Tint", currentColor); // _Color is a common color property name, adjust as needed

            // Update the skybox material in render settings
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment(); // Update global illumination to reflect changes in skybox
        }
    }
}
