using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;
using Photon.Realtime;

public class DropPlace : MonoBehaviour, IDropHandler
{
    public GameManager GM;
    public int Num;
    public bool SelectPhaze;
    public GameObject MirrorObj;
    public GameObject Mirror;
    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        if (SelectPhaze == false)
        {
            if (eventData.pointerDrag.GetComponent<CardController>().model.CTM == "Mirror")
            {
                Mirror = eventData.pointerDrag;
                StartCoroutine("MirrorWait", Num);
            }
            else
            {
                CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>(); // ドラッグしてきた情報からCardMovementを取得
                if (card != null) // もしカードがあれば、
                {
                    if (GM.FieldList[Num].transform.childCount == 0)
                    {
                        card.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする
                        card.GetComponent<CardController>().model.MoveNumber = 1;
                        GM.MoveCard(card.GetComponent<CardController>().model.CardPlace,Num,0);
                        card.GetComponent<CardController>().model.CardPlace = Num;
                        //card.GetComponent<CardController>().model.CardPlace カードの位置情報取得
                    }
                }
            }
        }
    }
    IEnumerator MirrorWait(int a)
    {
        GM.WhatDropPlace = this.gameObject;
        GM.MirrorSelect();
        SelectPhaze = true;
        yield return new WaitWhile(() => SelectPhaze); // flg がfalseになるまで処理が止まる
        Debug.Log(MirrorObj);
        Debug.Log(MirrorObj.GetComponent<CardController>().ID);
        string MirrorPorE = "P";
        if (a < 5)
        {
            MirrorPorE = "P";
        }
        if (a > 9)
        {
            MirrorPorE = "E";
        }
        GM.photonView.RPC("CreateCard", RpcTarget.All, MirrorObj.GetComponent<CardController>().ID,a,MirrorPorE);
        Destroy(Mirror);
    }
}
