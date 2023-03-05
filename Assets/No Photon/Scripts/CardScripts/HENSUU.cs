using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hensu;

[CustomEditor(typeof(CardEntity))]
public class HENSUU : Editor
{
    private static readonly string[] ranges = { "自分フィールド", "自分手札", "自分墓地" };//1,2,3桁目
    private static readonly string[] Types = { "モンスター", "魔法", "ミラー" };//1,2,3桁目

    public override void OnInspectorGUI()
    {
        var entity = target as CardEntity;

        entity.MirrorRange = EditorGUILayout.MaskField("対象範囲", entity.MirrorRange, ranges);
        entity.MirrorType = EditorGUILayout.MaskField("対象種類", entity.MirrorType, Types);
        entity.cardID = EditorGUILayout.IntField("cardID",entity.cardID);
        entity.name = EditorGUILayout.TextField("name",entity.name);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manaList"), true);
        entity.power = EditorGUILayout.IntField("power",entity.power);
        entity.Defence = EditorGUILayout.IntField("defence",entity.Defence);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"), true);
        entity.CT = (Cardtype)EditorGUILayout.EnumPopup( "EnumPopup",(System.Enum)entity.CT);
        // pokemon.Type == 0 の場合、未選択
        // pokemon.Type == 1 の場合、「ほのお」
        // pokemon.Type == 2 の場合、「みず」
        // pokemon.Type == 3 の場合、「ほのお」「みず」
        // pokemon.Type == 4 の場合、「くさ」
        // pokemon.Type == 5 の場合、「ほのお」「くさ」
        // pokemon.Type == 6 の場合、「みず」「くさ」
        // pokemon.Type == 7 の場合、「ほのお」「みず」「くさ」
    }
}
namespace hensu
{
    public enum Cardtype
    {
        Mirror,
        Magic,
        Monster,
        Instant,
        Field,
        Building,
    };  
}
