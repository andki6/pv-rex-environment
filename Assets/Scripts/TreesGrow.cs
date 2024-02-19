using UnityEngine;

public class TreesGrow : EnvironmentAnimation
{
    public GameObject TreeParent; // Assign the parent GameObject containing all the trees as children in the Inspector
    public AnimationCurve growthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private Transform[] treeTransforms;

    public override void Init()
    {
        // Check if TreeParent is assigned
        if (TreeParent != null)
        {
            // Initialize the treeTransforms array with the transforms of all children
            treeTransforms = new Transform[TreeParent.transform.childCount];
            for (int i = 0; i < TreeParent.transform.childCount; i++)
            {
                treeTransforms[i] = TreeParent.transform.GetChild(i);
                treeTransforms[i].gameObject.SetActive(false); // Disable all trees at the start
            }
        }
        else
        {
            Debug.LogError("TreeParent not assigned in TreesGrow script.");
        }
    }

    public override void UpdateAnimation(float progress)
    {
        // Enable and grow trees based on progress
        if (treeTransforms != null)
        {
            foreach (var treeTransform in treeTransforms)
            {
                if (treeTransform != null)
                {
                    GameObject tree = treeTransform.gameObject;

                    // Enable tree if progress is greater than 0
                    if (!tree.activeSelf && progress > 0)
                    {
                        tree.SetActive(true);
                    }

                    // Calculate the scale of the tree based on the growth curve and progress
                    float scale = growthCurve.Evaluate(progress);
                    treeTransform.localScale = Vector3.one * scale;
                }
            }
        }
    }
}
