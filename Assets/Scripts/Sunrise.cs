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

        _sunlight.intensity = 0;

        _skyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = _skyboxMaterial;

        _skyboxMaterial.SetFloat("_Exposure", minExposure);
        RenderSettings.fogColor = fogColorAtMinExposure;
    }

    public override void UpdateAnimation(float progress)
    {
        float newIntensity = progress * 1.5f; // Scale to a max of 1.5
        _sunlight.intensity = newIntensity;

        float exposure = Mathf.Lerp(minExposure, maxExposure, progress);
        _skyboxMaterial.SetFloat("_Exposure", exposure);

        RenderSettings.fogColor = Color.Lerp(fogColorAtMinExposure, fogColorAtMaxExposure, progress);
    }
}
