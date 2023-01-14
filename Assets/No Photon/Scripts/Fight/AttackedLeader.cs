using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class AttackedLeader : MonoBehaviourPunCallbacks, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            /// 攻撃
            // attackerを選択　マウスポインターに重なったカードをアタッカーにする
            CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

            GameManager.instance.AttackToLeader(attackCard, true);

        }
    }
}
