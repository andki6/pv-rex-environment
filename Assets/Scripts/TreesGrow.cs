using UnityEngine;

public class TreesGrow : EnvironmentAnimation
{
    public GameObject TreeParent; // Assign in the Inspector
    public AnimationCurve growthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float startVariance = 2.0f; // The maximum variance in start times
    // New approach: Define a base width scale and a variance around it
    public float baseWidthScale = 1.0f; // Base width scale for trees
    public float widthScaleVariance = 0.1f; // Variance in width scale

    private Transform[] treeTransforms;
    private float[] startTimes;
    private float[] widthScales; // Individual width scale for each tree

    public override void Init()
    {
        if (TreeParent != null)
        {
            int childCount = TreeParent.transform.childCount;
            treeTransforms = new Transform[childCount];
            startTimes = new float[childCount];
            widthScales = new float[childCount]; // Initialize the widthScales array

            for (int i = 0; i < childCount; i++)
            {
                treeTransforms[i] = TreeParent.transform.GetChild(i);
                treeTransforms[i].gameObject.SetActive(false);

                // Assign a random start time within the specified variance for each tree
                startTimes[i] = Random.Range(0f, startVariance);

                // Determine a width scale for each tree within the specified variance
                widthScales[i] = baseWidthScale + Random.Range(-widthScaleVariance, widthScaleVariance);
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

                    // Adjust progress based on the tree's start time
                    float adjustedProgress = Mathf.Clamp01((progress * AnimationDuration - startTimes[i]) / (AnimationDuration - startTimes[i]));

                    if (!tree.activeSelf && adjustedProgress > 0)
                    {
                        tree.SetActive(true);
                    }

                    if (tree.activeSelf)
                    {
                        // Calculate the scale of the tree based on the growth curve and adjusted progress
                        float scale = growthCurve.Evaluate(adjustedProgress);
                        // Apply the individual width scale to the X and Z axes
                        treeTransform.localScale = new Vector3(scale * widthScales[i], scale, scale * widthScales[i]);
                    }
                }
            }
        }
    }
}
