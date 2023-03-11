using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class CardMovement : MonoBehaviourPunCallbacks, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        if (GameManager.instance.isMyTurn)
        {
            cardParent = transform.parent;
            transform.SetParent(cardParent.parent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
        }
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        if (GameManager.instance.isMyTurn)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (GameManager.instance.isMyTurn)
        {
            transform.SetParent(cardParent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        }
    }
}
