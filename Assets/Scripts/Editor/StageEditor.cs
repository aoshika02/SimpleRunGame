# if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stage))]
public class StageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Stage stageDatas = (Stage)target;

        if (stageDatas.PointData == null)
            stageDatas.PointData = new StageItemDataHierarchy<Transform>();


        StageItemDataHierarchy<Transform> pointData = stageDatas.PointData;

        if (pointData.DataBases == null || pointData.DataBases.Length == 0)
            return;

        for (int z = 0; z < pointData.DataBases.Length; z++)
        {
            EditorGUILayout.LabelField($"Floor : {z}");
            EditorGUILayout.LabelField($"▶FRONT◀");
            for (int y = 0; y < pointData.DataBases[z].DataBlocks.Length; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < pointData.DataBases[z].DataBlocks[y].Cell.Length; x++)
                {
                    pointData.DataBases[z].DataBlocks[y].Cell[x] =
                        (Transform)EditorGUILayout.ObjectField(pointData.DataBases[z].DataBlocks[y].Cell[x], typeof(Transform), true, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.LabelField($"▶BACK◀");
        }
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(stageDatas);
        }
    }
}
#endif