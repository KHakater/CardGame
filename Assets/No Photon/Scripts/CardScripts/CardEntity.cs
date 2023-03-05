using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hensu;
[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public int cardID;
    public new string name;
    [SerializeField]public List<int> manaList = new List<int>();
    public int power;
    public int Defence;
    [SerializeField]public Sprite icon;
    
    //[SerializeField]public List<string> effectlist;
    public Cardtype CT;
    public int MirrorRange;
    public int MirrorType;
}
