using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hensu;
[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{//HENSUUがEditor内に存在
    public int cardID;
    public new string name;
    public int NeedMana;
    public int power;
    public int Defence;
    [SerializeField] public Sprite icon;

    //[SerializeField]public List<string> effectlist;
    public Cardtype CT;
    public int MirrorRange;
    public int MirrorType;


    public string ReverseName;
    public int ReverseNeedMana;
    public int Reversepower;
    public int ReverseDefence;
    [SerializeField] public Sprite ReverseIcon;

    //[SerializeField]public List<string> effectlist;
    public Cardtype ReverseCT;
    public int ReverseMirrorRange;
    public int ReverseMirrorType;
    [SerializeField] public List<EffectSetting> effect;
    [SerializeField] public List<EffectSetting> Reverseeffect;
    [SerializeField] public List<int> color, Reversecolor;
}
