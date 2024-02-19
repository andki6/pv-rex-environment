using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveEnvironmentController : MonoBehaviour
{
    public List<EnvironmentAnimation> animations;

    private IEnumerator Start()
    {
        foreach (var animation in animations)
        {
            animation.Init();
            StartCoroutine(AnimateWithDelay(animation));
        }
        yield break;
    }

    private IEnumerator AnimateWithDelay(EnvironmentAnimation animation)
    {
        // Wait for the specified start time before beginning the animation
        yield return new WaitForSeconds(animation.StartTime);

        float elapsedTime = 0f;

        // Start the animation after the delay
        while (elapsedTime < animation.AnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / animation.AnimationDuration);
            animation.UpdateAnimation(progress);
            yield return null;
        }

        // Ensure the animation completes
        animation.UpdateAnimation(1f);
    }
}
