using UnityEngine;

public class TreesSway : EnvironmentAnimation
{
    public GameObject TreeParent;
    public float StartSwayAngle = 2f;
    public float MaxSwayAngle = 10f;
    public float SwaySpeed = 0.5f;
    public float MaxSwaySpeed = 2f;
    public float TiltVariance = 5f;
    public AnimationCurve tiltProgressionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)); // Tilt progression curve

    private Transform[] treeTransforms;
    private float[] targetTiltAngles;
    private bool animationCompleted = false;
    private float finalSwayAngle;
    private float finalSwaySpeed;
    private float[] finalTiltAngles;

    public override void Init()
    {
        if (TreeParent != null)
        {
            int childCount = TreeParent.transform.childCount;
            treeTransforms = new Transform[childCount];
            targetTiltAngles = new float[childCount];
            finalTiltAngles = new float[childCount];

            for (int i = 0; i < childCount; i++)
            {
                treeTransforms[i] = TreeParent.transform.GetChild(i);
                targetTiltAngles[i] = Random.Range(-TiltVariance, TiltVariance);
                finalTiltAngles[i] = 0; // Initialize with 0; will be updated at the end of the animation
            }
        }
        else
        {
            Debug.LogError("TreeParent not assigned in TreesSway script.");
        }
    }

    public override void UpdateAnimation(float progress)
    {
        // If the animation has completed, do not continue to update
        if (animationCompleted) return;

        finalSwayAngle = Mathf.Lerp(StartSwayAngle, MaxSwayAngle, progress);
        finalSwaySpeed = Mathf.Lerp(SwaySpeed, MaxSwaySpeed, progress);

        for (int i = 0; i < treeTransforms.Length; i++)
        {
            Transform treeTransform = treeTransforms[i];
            if (treeTransform != null)
            {
                float tiltProgress = tiltProgressionCurve.Evaluate(progress);
                finalTiltAngles[i] = Mathf.Lerp(0, targetTiltAngles[i], tiltProgress);

                ApplySway(treeTransform, i, finalSwayAngle, finalSwaySpeed, finalTiltAngles[i]);
            }
        }

        if (progress >= 0.999f)
        {
            animationCompleted = true;
        }
    }

    void Update()
    {
        if (animationCompleted)
        {
            for (int i = 0; i < treeTransforms.Length; i++)
            {
                Transform treeTransform = treeTransforms[i];
                if (treeTransform != null)
                {
                    ApplySway(treeTransform, i, finalSwayAngle, finalSwaySpeed, finalTiltAngles[i]);
                }
            }
        }
    }

    private void ApplySway(Transform treeTransform, int index, float swayAngle, float swaySpeed, float tiltAngle)
    {
        float swayAngleCurrent = Mathf.Sin((Time.time * swaySpeed) + index) * swayAngle;
        Quaternion swayRotation = Quaternion.Euler(tiltAngle, swayAngleCurrent, 0f);
        treeTransform.localRotation = swayRotation;
    }
}
