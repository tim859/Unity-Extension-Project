using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPainterEditorWindow : EditorWindow
{
    // prefab painter editor settings
    GameObject editorPrefab;
    bool editorAlignWithGround;
    bool editorCanPaintOnPrefabs;

    // brush editor settings
    float editorBrushSize = 1f;
    float editorBrushThickness = 0.5f;
    Color editorBrushColour = Color.white;

    GUIStyle TitleLabel = new GUIStyle();
    GUIStyle SubTitleLabel = new GUIStyle();

    private void Awake()
    {
        // Title Label GUIStyle settings
        TitleLabel.fontSize = 30;
        TitleLabel.fontStyle = FontStyle.Bold;
        // TitleLabel.font = "Calibri"; // doesn't work

        // SubTitle Label GUIStyle settings
        SubTitleLabel.fontSize = 20;
    }

    [MenuItem("Custom Tools/Prefab Painter")]
    static void PrefabPainterWindow()
    {
        GetWindow(typeof(PrefabPainterEditorWindow));
    }

    private void OnGUI()
    {
        // get prefab painter component of selected game object
        PrefabPainter prefabPainter = Selection.activeGameObject.GetComponent<PrefabPainter>();

        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Prefab Painter Tool", TitleLabel);
            GUILayout.FlexibleSpace();
        }

        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Prefab Painter Settings", SubTitleLabel);
            GUILayout.FlexibleSpace();
        }

        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            // store new prefab
            editorPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab to paint: ", editorPrefab, typeof(GameObject), false);

            // clear prefab
            if (GUILayout.Button("Clear prefab"))
            {
                prefabPainter.prefab = null;
                editorPrefab = null;
            }
        }

        // store align choice
        editorAlignWithGround = EditorGUILayout.Toggle("Align with ground: ", editorAlignWithGround);

        // store painting on prefab choice
        editorCanPaintOnPrefabs = EditorGUILayout.Toggle("Can paint on prefabs: ", editorCanPaintOnPrefabs);

        // clear prefabs button
        if (GUILayout.Button("Clear all instantiated prefabs"))
        {
            PrefabPainterEditor.ClearPrefabs();
        }

        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Brush Settings", SubTitleLabel);
            GUILayout.FlexibleSpace();
        }

        // store new brush size
        editorBrushSize = EditorGUILayout.Slider("Brush Size: ", editorBrushSize, 0f, 50f);

        // store new brush thickness
        editorBrushThickness = EditorGUILayout.Slider("Brush Thickness: ", editorBrushThickness, 0f, 10f);

        // store new brush colour
        editorBrushColour = EditorGUILayout.ColorField("Brush Colour: ", editorBrushColour);

        if (editorPrefab != null)
        {
            EditorUtility.SetDirty(this);
        }

        if (prefabPainter != null) // ensures that prefab painter exists before we try to make any changes
        {
            // if the prefab selector in the custom inspector windows was to somehow end up null
            // but the prefab selector on the actual prefabPainter game object didn't
            // then that could be a little confusing. this ensures that can't happen
            if (editorPrefab == null && prefabPainter.prefab != null)
            {
                editorPrefab = prefabPainter.prefab;
            }
            else if (editorPrefab != null)
            {
                prefabPainter.prefab = editorPrefab; // apply the new prefab (as long as it isn't null)
                                                     // because if it can apply a null prefab then whenever
                                                     // the custom inspector is closed and reopened, the
                                                     // prefab selector on the game object will be set to null
                                                     // at least until i serialize the prefab selector in the custom inspector
            }

            prefabPainter.alignWithGround = editorAlignWithGround; // apply align choice
            prefabPainter.canPaintOnPrefabs = editorCanPaintOnPrefabs; // apply paint on prefabs choice
            prefabPainter.brushSize = editorBrushSize; // apply new brush size
            prefabPainter.brushThickness = editorBrushThickness; // apply new brush thickness
            prefabPainter.brushColour = editorBrushColour; // apply new brush colour
        }
    }
}
