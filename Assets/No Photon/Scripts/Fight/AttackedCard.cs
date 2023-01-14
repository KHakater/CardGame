using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using UnityEngine.EventSystems;
public class AttackedCard : MonoBehaviourPunCallbacks, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.GMSelectPhaze == false)
        {
            /// 攻撃
            // attackerを選択　マウスポインターに重なったカードをアタッカーにする
            CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

            // defenderを選択　
            CardController defenceCard = GetComponent<CardController>();
            // バトルする
            GameManager.instance.CardBattle(attackCard, defenceCard);
        }
    }
}
