# if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SpawnItemDatas))]
public class SpawnItemDatasEditor : Editor
{
    private List<bool> _isOpenStageDatas = new List<bool>();
    private List<bool> _isOpenFloors = new List<bool>();
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SpawnItemDatas stageDatas = (SpawnItemDatas)target;

        if (stageDatas.StageItemDataList == null)
            stageDatas.StageItemDataList = new List<StageItemDataHierarchy<ItemType>>();

        for (int i = 0; i < stageDatas.StageItemDataList.Count; i++)
        {
            _isOpenStageDatas.Add(false);
            StageItemDataHierarchy<ItemType> stageData = stageDatas.StageItemDataList[i];

            if (stageData.DataBases == null || stageData.DataBases.Length == 0)
                return;
            _isOpenStageDatas[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_isOpenStageDatas[i], $"SpawnItemData : {i + 1}");
            if (_isOpenStageDatas[i])
            {
                for (int z = 0; z < stageData.DataBases.Length; z++)
                {
                    _isOpenFloors.Add(false);
                    int floorIndex = i * stageData.DataBases.Length + z;
                    _isOpenFloors[floorIndex] = EditorGUILayout.Foldout(_isOpenFloors[floorIndex], $"Floor : {z + 1}");
                    if (_isOpenFloors[floorIndex])
                    {
                        EditorGUILayout.LabelField($"▶FRONT◀");
                        for (int y = 0; y < stageData.DataBases[z].DataBlocks.Length; y++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            for (int x = 0; x < stageData.DataBases[z].DataBlocks[y].Cell.Length; x++)
                            {
                                stageData.DataBases[z].DataBlocks[y].Cell[x] =
                                    (ItemType)EditorGUILayout.EnumPopup(stageData.DataBases[z].DataBlocks[y].Cell[x], GUILayout.Width(80));
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.LabelField($"▶BACK◀");
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
                GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        if (GUILayout.Button($"Add {nameof(StageItemDataHierarchy<ItemType>)}"))
        {
            stageDatas.StageItemDataList.Add(new StageItemDataHierarchy<ItemType>());
        }

        if (GUILayout.Button($"Remove Last {nameof(StageItemDataHierarchy<ItemType>)}") && stageDatas.StageItemDataList.Count > 0)
        {
            stageDatas.StageItemDataList.RemoveAt(stageDatas.StageItemDataList.Count - 1);
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(stageDatas);
        }
    }
}
#endif