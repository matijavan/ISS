
//chatgpt
using UnityEngine;

public class TerrainLinePainter : MonoBehaviour
{
    public Terrain terrain;           // Assign your Terrain here
    public int textureIndex = 0;      // Which terrain layer to paint
    public float brushSize = 5f;      // Brush radius in meters
    public Vector3 startPoint;        // First point of the line
    public Vector3 endPoint;          // Second point of the line

    void Start()
    {
        PaintLine();
    }

    void PaintLine()
    {
        if (terrain == null) return;

        TerrainData tData = terrain.terrainData;
        int alphaRes = tData.alphamapWidth;

        Vector3 terrainPos = terrain.transform.position;

        int steps = Mathf.CeilToInt(Vector3.Distance(startPoint, endPoint));
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);

            int mapX = Mathf.RoundToInt((point.x - terrainPos.x) / tData.size.x * alphaRes);
            int mapZ = Mathf.RoundToInt((point.z - terrainPos.z) / tData.size.z * alphaRes);

            int brushRadius = Mathf.RoundToInt((brushSize / tData.size.x) * alphaRes);

            float[,,] alphas = tData.GetAlphamaps(0, 0, alphaRes, alphaRes);

            for (int x = -brushRadius; x <= brushRadius; x++)
            {
                for (int z = -brushRadius; z <= brushRadius; z++)
                {
                    int px = Mathf.Clamp(mapX + x, 0, alphaRes - 1);
                    int pz = Mathf.Clamp(mapZ + z, 0, alphaRes - 1);

                    alphas[pz, px, textureIndex] = 1f; // paint fully
                    // Optional: reset other textures
                    for (int tIndex = 0; tIndex < alphas.GetLength(2); tIndex++)
                    {
                        if (tIndex != textureIndex)
                            alphas[pz, px, tIndex] = 0f;
                    }
                }
            }

            tData.SetAlphamaps(0, 0, alphas);
        }

        Debug.Log("Line painted!");
    }
}
