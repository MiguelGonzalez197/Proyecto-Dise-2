using UnityEngine;

[CreateAssetMenu(fileName = "TerrainConfig", menuName = "AntSim/TerrainConfig")]
public class TerrainConfig : ScriptableObject
{
    [Header("Grid")]
    public int width = 96;
    public int height = 96;
    public float cellSize = 1f;

    [Header("Seed & Noise")]
    public int seed = 12345;
    [Range(1f, 200f)] public float baseNoiseScale = 32f; // terreno (obstáculo/agua/llano)
    [Range(1f, 200f)] public float foodNoiseScale = 18f; // parches de comida

    [Header("Thresholds (0..1)")]
    [Range(0f, 1f)] public float obstacleThresh = 0.62f;
    [Range(0f, 1f)] public float waterThresh = 0.70f; // > waterThresh = agua (prioridad sobre obstáculo)
    [Range(0f, 1f)] public float foodThresh = 0.66f;

    [Header("Ratios / Garantías")]
    [Range(0.0f, 0.6f)] public float maxBlockedRatio = 0.45f; // % máximo de celdas no caminables
    public int minFoodPatches = 6;
    public int minConnectionPaths = 3;

    [Header("Nest")]
    public Vector2Int nestPos = new Vector2Int(8, 8); // si (-1,-1) => auto-colocar
    public int minClearRadius = 3; // despejar alrededor del nido

    [Header("Smoothing")]
    public int smoothPasses = 1; // opcional: suaviza obstáculos

    [Header("Debug")]
    public bool forceRegenerateOnPlay = true;
}
