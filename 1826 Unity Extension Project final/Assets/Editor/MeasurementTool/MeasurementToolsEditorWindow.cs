using UnityEditor;

public class MeasurementToolsEditorWindow : EditorWindow
{
    [MenuItem("Custom Tools/Measurement Tools/Distance Measuring Tool/Measure on XY axis")]
    static void MeasureDistanceXY()
    {
        MeasurementToolsEditor.measurementToolType = 1;
    }

    [MenuItem("Custom Tools/Measurement Tools/Distance Measuring Tool/Measure on XZ axis")]
    static void MeasureDistanceXZ()
    {
        MeasurementToolsEditor.measurementToolType = 2;
    }

    [MenuItem("Custom Tools/Measurement Tools/Distance Measuring Tool/Measure on YZ axis")]
    static void MeasureDistanceYZ()
    {
        MeasurementToolsEditor.measurementToolType = 3;
    }
}
