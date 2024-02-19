using UnityEngine;

public class TreesGrow : EnvironmentAnimation
{
    public GameObject TreeParent; // Assign in the Inspector
    public AnimationCurve growthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float startVariance = 2.0f; // The maximum variance in start times
    public float baseWidthScale = 1.0f; // Base width scale for trees
    public float widthScaleVariance = 0.1f; // Variance in width scale

    private Transform[] treeTransforms;
    private float[] startTimes;
    private float[] widthScales; // Individual width scale for each tree
    private float[] individualDurations; // Individual duration for each tree's growth

    public override void Init()
    {
        if (TreeParent != null)
        {
            int childCount = TreeParent.transform.childCount;
            treeTransforms = new Transform[childCount];
            startTimes = new float[childCount];
            widthScales = new float[childCount];
            individualDurations = new float[childCount]; // Initialize the individualDurations array

            for (int i = 0; i < childCount; i++)
            {
                treeTransforms[i] = TreeParent.transform.GetChild(i);
                treeTransforms[i].gameObject.SetActive(false);

                startTimes[i] = Random.Range(0f, startVariance);
                widthScales[i] = baseWidthScale + Random.Range(-widthScaleVariance, widthScaleVariance);

                // Calculate individual growth durations based on start times
                // Trees that start later will have slightly shorter growth periods to finish at different times
                individualDurations[i] = AnimationDuration - startTimes[i] * (AnimationDuration / startVariance);
            }
        }
        else
        {
            Debug.LogError("TreeParent not assigned in TreesGrow script.");
        }
    }

    public override void UpdateAnimation(float progress)
    {
        if (treeTransforms != null)
        {
            for (int i = 0; i < treeTransforms.Length; i++)
            {
                Transform treeTransform = treeTransforms[i];
                if (treeTransform != null)
                {
                    GameObject tree = treeTransform.gameObject;

                    // Here, we calculate the adjusted progress based on each tree's individual duration
                    float adjustedProgress = Mathf.Clamp01((progress * AnimationDuration - startTimes[i]) / individualDurations[i]);

                    if (!tree.activeSelf && adjustedProgress > 0)
                    {
                        tree.SetActive(true);
                    }

                    if (tree.activeSelf)
                    {
                        float scale = growthCurve.Evaluate(adjustedProgress);
                        treeTransform.localScale = new Vector3(scale * widthScales[i], scale, scale * widthScales[i]);
                    }
                }
            }
        }
    }
}
