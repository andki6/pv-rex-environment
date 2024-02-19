using UnityEngine;

public class GrassGrow : EnvironmentAnimation
{
    private Terrain _terrain;
    private int detailLayer = 0;
    private int maxGrassDensity = 75;

    private Material terrainMaterial;

    public override void Init()
    {
        _terrain = GetComponentInChildren<Terrain>();
        if (_terrain != null)
        {
            int width = _terrain.terrainData.detailWidth;
            int height = _terrain.terrainData.detailHeight;
            int[,] emptyGrassMap = new int[width, height];

            _terrain.terrainData.SetDetailLayer(0, 0, detailLayer, emptyGrassMap);

            terrainMaterial = _terrain.materialTemplate;
            if (terrainMaterial != null && terrainMaterial.HasProperty("_Blend"))
            {
                terrainMaterial.SetFloat("_Blend", 0.0f);
            }
            UpdateGrassDensity(0);
        }
    }

    public override void UpdateAnimation(float progress)
    {
        if (_terrain == null || terrainMaterial == null) return;

        UpdateGrassDensity(progress);

        if (terrainMaterial.HasProperty("_Blend"))
        {
            terrainMaterial.SetFloat("_Blend", progress);
        }
    }

    private void UpdateGrassDensity(float progress)
    {
        int width = _terrain.terrainData.detailWidth;
        int height = _terrain.terrainData.detailHeight;
        int[,] growingGrassMap = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                growingGrassMap[x, y] = (int)(maxGrassDensity * progress);
            }
        }

        _terrain.terrainData.SetDetailLayer(0, 0, detailLayer, growingGrassMap);
    }
}
