using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class CardMovement : MonoBehaviourPunCallbacks, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    public GameObject canvas;
    private RectTransform rectTransform;
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }
    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        if (GameManager.instance.isMyTurn)
        {
            cardParent = transform.parent;
            transform.parent = canvas.transform;
            transform.position = cardParent.transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
            rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        if (GameManager.instance.isMyTurn)
        {
            rectTransform = GetComponent<RectTransform>();
            // Vector2 localPosition = GetLocalPosition(eventData.position);
            // rectTransform.localPosition = localPosition;
            // //transform.position = eventData.position;
            // if (Input.GetKeyDown(KeyCode.P))
            // {
            //     Debug.Log(GetComponent<RectTransform>().position);
            // }
            rectTransform.localPosition = new Vector3(-(float)Screen.width / 2 + eventData.position.x, -(float)Screen.height / 2 + eventData.position.y, 0);
        }
    }
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, Camera.main, out result);
        return result;
    }
    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (!GameManager.instance.isMyTurn) return;
        if (eventData.pointerEnter.GetComponent<DropPlace>() != null)
        {
            var v = eventData.pointerEnter.GetComponent<DropPlace>();
            // if (GameManager.instance.ImMorN)
            // {
            //     if (!(v.Num >= 0 && v.Num <= 4))
            //     {
            //         CantSummon();
            //     }
            // }
            // else
            // {
            //     if (!(v.Num >= 11 && v.Num <= 14))
            //     {
            //         CantSummon();
            //     }
            // }

            if (!(v.Num >= 0 && v.Num <= 4))
            {
                CantSummon();
            }
        }
        else
        {
            CantSummon();
        }
        //transform.SetParent(cardParent, false);
        rectTransform.localScale = new Vector3(1, 1, 1);
        //transform.position = cardParent.transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
    }
    public void CantSummon()
    {
        transform.SetParent(cardParent, false);
        rectTransform.localScale = new Vector3(1, 1, 1);
        transform.position = cardParent.transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
    }
}
