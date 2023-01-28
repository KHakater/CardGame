using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ManaEntity", menuName = "Create ManaEntity")]
public class ManaEntity :ScriptableObject
{
    public int ManaID;
    public string ManaName;
    public int MaxMana;
    public int NowMana;
    public int ManaColor;
    public Sprite ImageMana;
}
