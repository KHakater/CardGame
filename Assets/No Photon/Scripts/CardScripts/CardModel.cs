using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int CardID;
    public string name;
    public List<int> Mlist = new List<int>();
    public int power;
    public int Defence;
    public Sprite icon;
    public bool canAttack = false;

    public bool MastersCard = false;
    public string CTM;
    public bool MSelectable = false;
    public int CardPlace;
    public int MirrorRange;
    public int MirrorType;


    public string Reversename;
    public List<int> ReverseMlist = new List<int>();
    public int Reversepower;
    public int ReverseDefence;
    public Sprite Reverseicon;
    public string ReverseCTM;
    public int ReverseMirrorRange;
    public int ReverseMirrorType;
    public bool IsFace = true;
    public CardModel(int cardID, bool playerCard, int CP, bool isfase) // データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        MastersCard = playerCard;
        CardID = cardEntity.cardID;
        MSelectable = false;
        CardPlace = CP;
        IsFace = isfase;
        if (isfase)
        {
            name = cardEntity.name;
            Mlist = cardEntity.manaList;
            power = cardEntity.power;
            Defence = cardEntity.Defence;
            icon = cardEntity.icon;
            CTM = cardEntity.CT.ToString();
            MirrorRange = cardEntity.MirrorRange;
            MirrorType = cardEntity.MirrorType;
        }
        else
        {
            Reversename = cardEntity.ReverseName;
            ReverseMlist = cardEntity.ReversemanaList;
            Reversepower = cardEntity.Reversepower;
            ReverseDefence = cardEntity.ReverseDefence;
            Reverseicon = cardEntity.ReverseIcon;
            ReverseCTM = cardEntity.ReverseCT.ToString();
            ReverseMirrorRange = cardEntity.ReverseMirrorRange;
            ReverseMirrorType = cardEntity.ReverseMirrorType;
        }
    }
}
