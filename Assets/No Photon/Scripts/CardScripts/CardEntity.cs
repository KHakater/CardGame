using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public int cardID;
    public new string name;
    public List<int> manaList = new List<int>();
    public int power;
    public int Defence;
    public Sprite icon;
    public enum Cardtype
    {
        Mirror,
        Magic,
        Unit,
        Instant,
        Field,
        Building,
    };
    public List<string> effectlist;
    public Cardtype CT; 
}
