using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ColumnColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Material> column;
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material levelMaterial = GetMaterialByIndex(column, Level.level - 1);
        if (mr != null && levelMaterial != null)
        {
            mr.material = levelMaterial;
        }
    }

    Material GetMaterialByIndex(List<Material> source, int rawIndex)
    {
        if (source == null || source.Count == 0)
            return null;

        int safeIndex = ((rawIndex % source.Count) + source.Count) % source.Count;
        return source[safeIndex];
    }
}
