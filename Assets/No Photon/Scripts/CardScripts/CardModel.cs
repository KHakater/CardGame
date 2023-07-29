using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hensu;

public class CardModel
{
    public int CardID;
    public string name;
    public int NeedMana;
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
    public bool isKyouzou;//鏡像かどうか
    public CardModel OriginalCM;
    public CardModel(int cardID, bool playerCard, int CP, bool isfase, bool cansee, bool isKYOUZOU) // データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        if (cardID == -1)//フィールド上の鏡
        {
            CTM = cardEntity.CT.ToString();
            MastersCard = playerCard;
            CardID = cardEntity.cardID;
            isKyouzou = isKYOUZOU;
            CardPlace = CP;
        }
        else
        {
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
                    NeedMana = cardEntity.NeedMana;
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
                    NeedMana = cardEntity.ReverseNeedMana;
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
            isKyouzou = isKYOUZOU;
        }
    }
    public string GetReverseCTMValue()
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + CardID);
        return cardEntity.ReverseCT.ToString();
    }
    public void Mirrorinit()
    {

    }
    public void ParamCopyFrom(CardModel model)
    {
        power = model.power;
    }
}
