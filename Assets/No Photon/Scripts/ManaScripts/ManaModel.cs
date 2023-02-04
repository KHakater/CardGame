using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaModel : MonoBehaviour
{
    public int CardID;
    public string Name;
    public int maxmana;
    public int nowmana;
    public Sprite icon;
    public bool PlayerCard = false;
    public List<int> color;
    public ManaModel(int cardID, bool playerCard,int max,int useful) // データを受け取り、その処理
    {
        ManaEntity me = Resources.Load<ManaEntity>("ManaEntityList/Mana" + cardID);
        CardID = me.ManaID;
        Name = me.ManaName;
        maxmana = max;
        nowmana = useful;
        icon = me.ImageMana;
        PlayerCard = playerCard;
        color = me.ManaColor;
    }
}
