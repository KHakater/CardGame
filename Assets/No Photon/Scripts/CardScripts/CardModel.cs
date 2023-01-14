using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int CardID;
    public string name;
    public int cost;
    public int power;
    public int Defence;
    public Sprite icon;
    public bool canAttack = false;

    public bool PlayerCard = false;
    public string CTM;
    public bool MSelectable = false;
    public int CardPlace;
    public int MoveNumber;
    public CardModel(int cardID, bool playerCard, int CP) // データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        CardID = cardEntity.cardID;
        name = cardEntity.name;
        cost = cardEntity.cost;
        power = cardEntity.power;
        Defence = cardEntity.Defence;
        icon = cardEntity.icon;
        MSelectable = false;
        PlayerCard = playerCard;
        CTM = cardEntity.CT.ToString();
        CardPlace = CP;
        MoveNumber = 0;
    }
}
