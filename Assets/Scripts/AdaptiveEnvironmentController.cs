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
        }
        foreach (var animation in animations)
        {
            yield return StartCoroutine(Animate(animation));
        }
    }

    private IEnumerator Animate(EnvironmentAnimation animation)
    {
        float elapsedTime = 0;

        while (elapsedTime < animation.AnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / animation.AnimationDuration);
            animation.UpdateAnimation(progress);
            yield return null;
        }

        animation.UpdateAnimation(1f);
    }
}
