using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckCardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject canvas;
    private RectTransform rectTransform;
    public Transform content;
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }
    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        transform.parent = canvas.transform;
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
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
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, Camera.main, out result);
        return result;
    }
    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        transform.parent = content;
        rectTransform.localScale = new Vector3(1, 1, 1);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
    }
}
