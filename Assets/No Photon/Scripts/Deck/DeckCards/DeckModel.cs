using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hensu;
public class DeckModel : MonoBehaviour
{
    public int CardID;
    public string Name;
    public int power;
    public int Defence;
    public Sprite icon;
    public string CTM;
    public int MirrorRange;
    public int MirrorType;
    public bool IsFace = true;
    public List<EffectSetting> effects;
    // public DeckModel(int cardID, bool isfase) // データを受け取り、その処理
    // {
    //     CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
    //     IsFace = isfase;
    //     CTM = cardEntity.CT.ToString();
    //     if (isfase)
    //     {
    //         Name = cardEntity.name;
    //         Mlist = cardEntity.manaList;
    //         power = cardEntity.power;
    //         Defence = cardEntity.Defence;
    //         icon = cardEntity.icon;
    //         MirrorRange = cardEntity.MirrorRange;
    //         MirrorType = cardEntity.MirrorType;
    //         effects = cardEntity.effect;

    //     }
    //     else
    //     {
    //         Name = cardEntity.ReverseName;
    //         Mlist = cardEntity.ReversemanaList;
    //         power = cardEntity.Reversepower;
    //         Defence = cardEntity.ReverseDefence;
    //         icon = cardEntity.ReverseIcon;
    //         MirrorRange = cardEntity.ReverseMirrorRange;
    //         MirrorType = cardEntity.ReverseMirrorType;
    //         effects = cardEntity.Reverseeffect;
    //         CTM = cardEntity.ReverseCT.ToString();
    //     }
    //     CardID = cardID;
    // }
    // public string GetReverseCTMValue()
    // {
    //     CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + CardID);
    //     return cardEntity.ReverseCT.ToString();
    // }
}
