using UnityEngine;

public abstract class EnvironmentAnimation : MonoBehaviour
{
    public float AnimationDuration = 5f; // Default duration

    public abstract void Init();

    public abstract void UpdateAnimation(float progress);
}
