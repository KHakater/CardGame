using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hensu;
[CustomEditor(typeof(CardEntity))]
public class HENSUU : Editor
{
    private string[] ranges1 = { "自分フィールド", "自分手札", "自分墓地" };//1,2,3桁目
    private string[] Types1 = { "モンスター", "魔法", "ミラー" };//1,2,3桁目

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();
        var entity = target as CardEntity;
        var r = EditorGUILayout.MaskField("対象範囲", entity.MirrorRange, ranges1);
        entity.MirrorRange = r;
        var t = EditorGUILayout.MaskField("対象種類", entity.MirrorType, Types1);
        entity.MirrorType = t;
        entity.cardID = EditorGUILayout.IntField("cardID", entity.cardID);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), true);//こっちも変更
        var color = serializedObject.FindProperty("color");
        EditorGUILayout.BeginHorizontal();
        using (new EditorGUILayout.HorizontalScope())
        {
            // 要素を追加
            if (GUILayout.Button("Add"))
            {
                color.InsertArrayElementAtIndex(color.arraySize);
            }
            // 要素を削除
            if (GUILayout.Button("Remove"))
            {
                if (color.arraySize >= 1)
                {
                    color.DeleteArrayElementAtIndex(color.arraySize - 1);
                }
            }
            // 要素をすべて削除
            if (GUILayout.Button("Clear"))
            {
                color.ClearArray();
            }
        }
        EditorGUILayout.EndHorizontal();

        entity.name = EditorGUILayout.TextField("name", entity.name);
        entity.NeedMana = EditorGUILayout.IntField("NeedMana", entity.NeedMana);
        entity.power = EditorGUILayout.IntField("power", entity.power);
        entity.Defence = EditorGUILayout.IntField("defence", entity.Defence);
        EditorGUILayout.PrefixLabel("Source Image");
        entity.icon = (Sprite)EditorGUILayout.ObjectField(entity.icon, typeof(Sprite), allowSceneObjects: true);

        entity.CT = (Cardtype)EditorGUILayout.EnumPopup("EnumPopup", (System.Enum)entity.CT);
        EditorGUILayout.Space();
        var rr = EditorGUILayout.MaskField("Reverse対象範囲", entity.ReverseMirrorRange, ranges1);
        entity.ReverseMirrorRange = rr;
        var rt = EditorGUILayout.MaskField("Reverse対象種類", entity.ReverseMirrorType, Types1);
        entity.ReverseMirrorType = rt;
        entity.ReverseName = EditorGUILayout.TextField("Reversename", entity.ReverseName);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Reversecolor"), true);//こっちも変更
        var rcolor = serializedObject.FindProperty("Reversecolor");
        EditorGUILayout.BeginHorizontal();
        using (new EditorGUILayout.HorizontalScope())
        {
            // 要素を追加
            if (GUILayout.Button("Add"))
            {
                rcolor.InsertArrayElementAtIndex(rcolor.arraySize);
            }
            // 要素を削除
            if (GUILayout.Button("Remove"))
            {
                if (rcolor.arraySize >= 1)
                {
                    rcolor.DeleteArrayElementAtIndex(rcolor.arraySize - 1);
                }
            }
            // 要素をすべて削除
            if (GUILayout.Button("Clear"))
            {
                rcolor.ClearArray();
            }
        }
        EditorGUILayout.EndHorizontal();

        entity.ReverseNeedMana = EditorGUILayout.IntField("ReverseNeedMana", entity.ReverseNeedMana);
        entity.Reversepower = EditorGUILayout.IntField("Reversepower", entity.Reversepower);
        entity.ReverseDefence = EditorGUILayout.IntField("Reversedefence", entity.ReverseDefence);
        EditorGUILayout.PrefixLabel("Rev Source Image");
        entity.ReverseIcon = (Sprite)EditorGUILayout.ObjectField(entity.ReverseIcon, typeof(Sprite), allowSceneObjects: true);
        entity.ReverseCT = (Cardtype)EditorGUILayout.EnumPopup("ReverseEnumPopup", (System.Enum)entity.ReverseCT);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("effect"), true);
        var ef = serializedObject.FindProperty("effect");
        EditorGUILayout.BeginHorizontal();
        using (new EditorGUILayout.HorizontalScope())
        {
            // 要素を追加
            if (GUILayout.Button("Add"))
            {
                ef.InsertArrayElementAtIndex(ef.arraySize);
            }
            // 要素を削除
            if (GUILayout.Button("Remove"))
            {
                if (ef.arraySize >= 1)
                {
                    ef.DeleteArrayElementAtIndex(ef.arraySize - 1);
                }
            }
            // 要素をすべて削除
            if (GUILayout.Button("Clear"))
            {
                ef.ClearArray();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Reverseeffect"), true);
        var ef2 = serializedObject.FindProperty("Reverseeffect");
        EditorGUILayout.BeginHorizontal();
        using (new EditorGUILayout.HorizontalScope())
        {
            // 要素を追加
            if (GUILayout.Button("Add"))
            {
                ef2.InsertArrayElementAtIndex(ef2.arraySize);
            }
            // 要素を削除
            if (GUILayout.Button("Remove"))
            {
                if (ef2.arraySize >= 1)
                {
                    ef2.DeleteArrayElementAtIndex(ef2.arraySize - 1);
                }
            }
            // 要素をすべて削除
            if (GUILayout.Button("Clear"))
            {
                ef2.ClearArray();
            }
        }
        EditorGUILayout.EndHorizontal();


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(entity, "Change Property");
            EditorUtility.SetDirty(entity);
        }
        serializedObject.ApplyModifiedProperties();
    }
}