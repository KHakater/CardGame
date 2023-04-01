using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hensu;

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
    public bool IsFace = true;
    public int LeaderHP;
    public List<EffectSetting> effects;
    public bool CanSee;
    public CardModel(int cardID, bool playerCard, int CP, bool isfase, bool cansee) // データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        IsFace = isfase;
        CTM = cardEntity.CT.ToString();
        if (CTM == "Leader")
        {
            LeaderHP = 20;
        }
        else
        {
            if (isfase)
            {
                name = cardEntity.name;
                Mlist = cardEntity.manaList;
                power = cardEntity.power;
                Defence = cardEntity.Defence;
                icon = cardEntity.icon;
                MirrorRange = cardEntity.MirrorRange;
                MirrorType = cardEntity.MirrorType;
                effects = cardEntity.effect;
            }
            else
            {
                name = cardEntity.ReverseName;
                Mlist = cardEntity.ReversemanaList;
                power = cardEntity.Reversepower;
                Defence = cardEntity.ReverseDefence;
                icon = cardEntity.ReverseIcon;
                MirrorRange = cardEntity.ReverseMirrorRange;
                MirrorType = cardEntity.ReverseMirrorType;
                effects = cardEntity.Reverseeffect;
                CTM = cardEntity.ReverseCT.ToString();
            }

        }
        MastersCard = playerCard;
        CardID = cardEntity.cardID;
        MSelectable = false;
        CardPlace = CP;
        CanSee = cansee;
    }
    public string GetReverseCTMValue()
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + CardID);
        return cardEntity.ReverseCT.ToString();
    }
}
