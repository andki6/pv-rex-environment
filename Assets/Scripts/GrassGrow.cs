using UnityEngine;

public class GrassGrow : EnvironmentAnimation
{
    private Terrain _terrain;
    private int maxGrassDensity = 75;
    private Material terrainMaterial;

    // Public variables to specify the patch of terrain to update during animation
    public int updateStartX = 0; // Starting X coordinate of the area to update
    public int updateStartY = 0; // Starting Y coordinate of the area to update
    public int updateWidth = 200;  // Width of the area to update
    public int updateHeight = 200; // Height of the area to update

    private float updateInterval = 0.2f; // Update every 0.2 seconds for 5 updates per second
    private float timeSinceLastUpdate = 0.0f;

    public override void Init()
    {
        _terrain = GetComponentInChildren<Terrain>();
        if (_terrain != null)
        {
            // Terrain size in world units
            Vector3 size = _terrain.terrainData.size;

            // Terrain detail size
            int detailWidth = _terrain.terrainData.detailWidth;
            int detailHeight = _terrain.terrainData.detailHeight;

            // Calculate the size of the box in detail units (10% of the terrain area, square root for a side length)
            int boxSideLength = (int)Mathf.Sqrt(0.1f * detailWidth * detailHeight);

            // Ensure the box dimensions are within the terrain's bounds
            boxSideLength = Mathf.Min(boxSideLength, detailWidth, detailHeight);

            // Calculate start positions for the box to be centered
            updateStartX = (detailWidth - boxSideLength) / 2;
            updateStartY = (detailHeight - boxSideLength) / 2;
            updateWidth = boxSideLength;
            updateHeight = boxSideLength;

            // Initialize an empty grass map
            int[,] emptyGrassMap = new int[detailWidth, detailHeight];

            int detailLayerCount = _terrain.terrainData.detailPrototypes.Length; // Get the number of detail layers
            for (int layer = 0; layer < detailLayerCount; layer++)
            {
                // Clear each detail layer across the entire terrain
                _terrain.terrainData.SetDetailLayer(0, 0, layer, emptyGrassMap);
            }

            terrainMaterial = _terrain.materialTemplate;
            if (terrainMaterial != null && terrainMaterial.HasProperty("_Blend"))
            {
                terrainMaterial.SetFloat("_Blend", 0.0f);
            }
        }
    }

    public override void UpdateAnimation(float progress)
    {
        if (_terrain == null || terrainMaterial == null) return;

        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            UpdateGrassDensity(progress);
            if (terrainMaterial.HasProperty("_Blend"))
            {
                terrainMaterial.SetFloat("_Blend", progress);
            }
            timeSinceLastUpdate = 0.0f;
        }
    }

    private void UpdateGrassDensity(float progress)
    {
        int detailLayerCount = _terrain.terrainData.detailPrototypes.Length; // Get the number of detail layers

        for (int layer = 0; layer < detailLayerCount; layer++)
        {
            // Update only the specified patch of grass
            int[,] growingGrassMap = new int[updateWidth, updateHeight];

            for (int y = 0; y < updateHeight; y++)
            {
                for (int x = 0; x < updateWidth; x++)
                {
                    growingGrassMap[x, y] = (int)(maxGrassDensity * progress);
                }
            }

            _terrain.terrainData.SetDetailLayer(updateStartX, updateStartY, layer, growingGrassMap);
        }
    }

}
