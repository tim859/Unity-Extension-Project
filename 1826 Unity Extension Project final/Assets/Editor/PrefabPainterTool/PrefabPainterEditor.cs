using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabPainter))]
public class PrefabPainterEditor : Editor
{
    public static List<GameObject> instantiatedPrefabs = new List<GameObject>();
    Quaternion prefabRotation;
    void OnSceneGUI()
    {
        PrefabPainter prefPaint = (PrefabPainter)target; // casting the component script
        Color newColour = new Color(prefPaint.brushColour.r, prefPaint.brushColour.g, prefPaint.brushColour.b);
        Handles.color = newColour;

        // gets current position of the mouse as ray in 3d space
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        // draws the circle handle that shows the user where the prefab will be painted
        if (Physics.Raycast(ray, out RaycastHit hoverHit))
        {
            Handles.DrawWireDisc(hoverHit.point, hoverHit.normal, prefPaint.brushSize, prefPaint.brushThickness);
        }

        Event e = Event.current;

        // instantiates the prefab when the mouse is clicked
        if (e.type == EventType.MouseDown && e.button == 0) // checks whether the left mouse button has been pressed
        {
            if (Physics.Raycast(ray, out RaycastHit prefabHit)) // does a raycast at the point that the cursor is currently on
            {
                switch (prefPaint.alignWithGround) // decides the rotation of the prefab based on the user preference
                {
                    case true:
                        prefabRotation = Quaternion.identity;
                        break;

                    case false:
                        prefabRotation = Quaternion.FromToRotation(prefabHit.transform.up, prefabHit.normal);
                        break;
                }

                switch (prefPaint.canPaintOnPrefabs) // chooses whether to allow the prefab to be instantiated or not based on the users preference
                {
                    case true:
                        // paint on whatever
                        InstantiatePrefab(prefPaint, prefabHit, prefabRotation);
                        break;

                    case false:
                        // will only paint if the target for painting on is not in the list of instantiated prefabs
                        if (!instantiatedPrefabs.Contains(prefabHit.transform.gameObject))
                        {
                            InstantiatePrefab(prefPaint, prefabHit, prefabRotation);
                        }
                        break;
                }
            }
        }

        // ensures that the prefab painter game object will not become unselected,
        // either by clicking on a different game object or by unity automatically switching to the newly created prefab
        Selection.activeGameObject = prefPaint.gameObject;

        // required otherwise OnSceneGUI will only be called intermittenly (roughly once per second) when mousing over the scene view
        // and it needs to be called every frame to feel snappy and responsive
        SceneView.RepaintAll();
    }

    public static void ClearPrefabs()
    {
        foreach (GameObject obj in instantiatedPrefabs)
        {
            DestroyImmediate(obj);
        }
        instantiatedPrefabs.Clear();
    }

    void InstantiatePrefab(PrefabPainter prefPaint, RaycastHit prefabHit, Quaternion prefabRotation)
    {
        GameObject iPref = Instantiate(prefPaint.prefab, prefabHit.point, prefabRotation); // iPref means instantiated prefab
        iPref.transform.localScale *= (prefPaint.brushSize * 5);
        instantiatedPrefabs.Add(iPref);
    }
}
