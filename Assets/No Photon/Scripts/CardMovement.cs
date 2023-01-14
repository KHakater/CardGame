using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class CardMovement : MonoBehaviourPunCallbacks, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    GameManager GM;
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        if (GM.isMyTurn)
        {
            cardParent = transform.parent;
            transform.SetParent(cardParent.parent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
        }
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        if (GM.isMyTurn)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (GM.isMyTurn)
        {
            transform.SetParent(cardParent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GM.isMyTurn)
            {
                if (photonView.IsMine)
                {
                    if (gameObject.GetComponent<CardController>().model.MSelectable == true)
                    {
                        gameObject.GetComponent<CardController>().model.MSelectable = false;
                        GM.MirrorWhatSelect(this.gameObject);
                    }
                }
            }
        }
    }
}
