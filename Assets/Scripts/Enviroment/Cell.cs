using UnityEngine;

public enum CellType { Ground, Obstacle, Water, Food, Nest }

[DisallowMultipleComponent]
public class Cell : MonoBehaviour
{
    public Vector2Int coords;
    public CellType type;
    public MeshRenderer mr;
    public BoxCollider box;

    [Header("Materials")]
    public Material groundMat, obstacleMat, waterMat, foodMat, nestMat;

    public void Init(Vector2Int c, float size)
    {
        coords = c;
        transform.localScale = new Vector3(size, 0.15f, size);
        mr = GetComponent<MeshRenderer>();
        if (!mr) mr = gameObject.AddComponent<MeshRenderer>();
        var mf = GetComponent<MeshFilter>();
        if (!mf) { mf = gameObject.AddComponent<MeshFilter>(); mf.sharedMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx"); }
        box = GetComponent<BoxCollider>(); if (!box) box = gameObject.AddComponent<BoxCollider>();
        box.isTrigger = false;
    }

    public void SetType(CellType t)
    {
        type = t;
        switch (t)
        {
            case CellType.Ground: mr.sharedMaterial = groundMat; box.isTrigger = true; break;
            case CellType.Obstacle: mr.sharedMaterial = obstacleMat; box.isTrigger = false; break;
            case CellType.Water: mr.sharedMaterial = waterMat; box.isTrigger = false; break;
            case CellType.Food: mr.sharedMaterial = foodMat; box.isTrigger = true; break;
            case CellType.Nest: mr.sharedMaterial = nestMat; box.isTrigger = true; break;
        }
    }

    public bool Walkable => type == CellType.Ground || type == CellType.Food || type == CellType.Nest;
    public float Cost => type == CellType.Ground ? 1f : (type == CellType.Food ? 0.9f : (type == CellType.Nest ? 1f : Mathf.Infinity));
}

