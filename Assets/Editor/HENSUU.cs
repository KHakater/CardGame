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
        var entity = target as CardEntity;
        var r = EditorGUILayout.MaskField("対象範囲", entity.MirrorRange, ranges1);
        entity.MirrorRange = r;
        var t = EditorGUILayout.MaskField("対象種類", entity.MirrorType, Types1);
        entity.MirrorType = t;
        entity.cardID = EditorGUILayout.IntField("cardID", entity.cardID);
        entity.name = EditorGUILayout.TextField("name", entity.name);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manaList"), true);//こっちも変更
        entity.power = EditorGUILayout.IntField("power", entity.power);
        entity.Defence = EditorGUILayout.IntField("defence", entity.Defence);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Source Image");
        entity.icon = (Sprite)EditorGUILayout.ObjectField(entity.icon, typeof(Sprite), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();
        entity.CT = (Cardtype)EditorGUILayout.EnumPopup("EnumPopup", (System.Enum)entity.CT);

        EditorGUILayout.Space();

        var rr = EditorGUILayout.MaskField("Reverse対象範囲", entity.ReverseMirrorRange, ranges1);
        entity.ReverseMirrorRange = rr;
        var rt = EditorGUILayout.MaskField("Reverse対象種類", entity.ReverseMirrorType, Types1);
        entity.ReverseMirrorType = rt;
        entity.ReverseName = EditorGUILayout.TextField("Reversename", entity.ReverseName);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ReversemanaList"), true);
        entity.Reversepower = EditorGUILayout.IntField("Reversepower", entity.Reversepower);
        entity.ReverseDefence = EditorGUILayout.IntField("Reversedefence", entity.ReverseDefence);
        //entity.ReverseIcon = EditorGUILayout.PropertyField(serializedObject.FindProperty("ReverseIcon"), new GUIContent("ReverseIcon"));
        //EditorGUILayout.ObjectField("Icon", entity.ReverseIcon, typeof(Sprite), false);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Source Image");
        entity.ReverseIcon = (Sprite)EditorGUILayout.ObjectField(entity.ReverseIcon, typeof(Sprite), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();
        entity.ReverseCT = (Cardtype)EditorGUILayout.EnumPopup("ReverseEnumPopup", (System.Enum)entity.ReverseCT);
        // pokemon.Type == 0 の場合、未選択
        // pokemon.Type == 1 の場合、「ほのお」
        // pokemon.Type == 2 の場合、「みず」
        // pokemon.Type == 3 の場合、「ほのお」「みず」
        // pokemon.Type == 4 の場合、「くさ」
        // pokemon.Type == 5 の場合、「ほのお」「くさ」
        // pokemon.Type == 6 の場合、「みず」「くさ」
        // pokemon.Type == 7 の場合、「ほのお」「みず」「くさ」
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(entity, "Change Property");
            EditorUtility.SetDirty(entity);
        }
    }
}
