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
    public bool MastersCard = false;
    public List<int> color;
    public ManaModel(int cardID, bool playerCard, int max, int now) // データを受け取り、その処理
    {
        ManaEntity me = Resources.Load<ManaEntity>("ManaEntityList/Mana" + cardID);
        CardID = me.ManaID;
        Name = me.ManaName;
        maxmana = max;
        nowmana = now;
        icon = me.ImageMana;
        MastersCard = playerCard;
        color = me.ManaColor;
    }
}
