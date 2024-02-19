using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 0.4f;

    void Update()
    {
        
        float rotation = Mathf.Repeat(Time.time * rotationSpeed, 360);
        
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
