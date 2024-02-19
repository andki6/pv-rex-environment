using UnityEngine;

public abstract class EnvironmentAnimation : MonoBehaviour
{
    public float AnimationDuration = 5f; // Default duration
    public float StartTime = 0f; // Time to wait before starting the animation

    public abstract void Init();
    public abstract void UpdateAnimation(float progress);
}
