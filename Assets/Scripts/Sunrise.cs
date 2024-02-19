using UnityEngine;

public class Sunrise : EnvironmentAnimation
{
    public float minExposure = 0.5f;
    public float maxExposure = 1f;
    public Color fogColorAtMinExposure = new Color(72, 91, 97);
    public Color fogColorAtMaxExposure = new Color (183, 239, 255);

    private Light _sunlight;
    private Material _skyboxMaterial;

    public override void Init()
    {
        _sunlight = GetComponentInChildren<Light>();

        // Ensure the sunlight starts off
        _sunlight.intensity = 0;

        // Prepare the skybox material
        _skyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = _skyboxMaterial;

        // Set initial exposure and fog color
        _skyboxMaterial.SetFloat("_Exposure", minExposure);
        RenderSettings.fogColor = fogColorAtMinExposure;
    }

    public override void UpdateAnimation(float progress)
    {
        // Update sunlight intensity
        float newIntensity = progress * 1.5f; // Scale to a max of 1.5
        _sunlight.intensity = newIntensity;

        // Adjust skybox exposure
        float exposure = Mathf.Lerp(minExposure, maxExposure, progress);
        _skyboxMaterial.SetFloat("_Exposure", exposure);

        // Interpolate fog color based on the same progress used for exposure
        RenderSettings.fogColor = Color.Lerp(fogColorAtMinExposure, fogColorAtMaxExposure, progress);
    }
}
