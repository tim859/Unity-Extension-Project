using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPainter : MonoBehaviour
{
    // prefab painter settings
    public GameObject prefab;
    public bool alignWithGround;
    public bool canPaintOnPrefabs;

    // brush settings
    public float brushSize;
    public float brushThickness;
    public Color brushColour;
}
