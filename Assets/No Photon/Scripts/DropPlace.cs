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
    public GameObject Mirror;
    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        if (GM.GMSelectPhaze == false)
        {
            if (eventData.pointerDrag.GetComponent<CardController>().model.CTM == "Mirror")
            {
                Mirror = eventData.pointerDrag;
                StartCoroutine("MirrorWait");
            }
            else if (eventData.pointerDrag.GetComponent<CardController>().model.CTM != "Magic")
            {
                CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>(); // ドラッグしてきた情報からCardMovementを取得
                if (card != null) // もしカードがあれば、
                {
                    var v = card.GetComponent<CardController>().model;
                    if (20 <= v.CardPlace && v.CardPlace <= 22)//フィールドからは動かせない
                    {
                        if (GM.FieldList[Num].transform.childCount == 0)
                        {
                            StartCoroutine("col", card);
                        }
                    }
                }
            }
        }
    }
    IEnumerator col(CardMovement c)
    {
        GM.ActChecker = false;
        GM.Activation(c.GetComponent<CardController>().model.MastersCard, c.GetComponent<CardController>().model.Mlist);
        yield return new WaitUntil(() => GM.ActChecker);
        if (GM.actReturn)
        {
            if (c.GetComponent<CardController>().model.CTM == "Monster")
            {
                GM.MonsterSummon(c.GetComponent<CardController>().model.Mlist, Num, c.gameObject.name);
            }
        }
        else
        {
            Debug.Log("CantSummon");
        }
        yield break;
    }
    IEnumerator MirrorWait()
    {
        var a = Mirror.GetComponent<CardController>();
        GM.ActChecker = false;
        GM.Activation(a.model.MastersCard, a.model.Mlist);
        yield return new WaitUntil(() => GM.ActChecker);//マナの支払いを待機
        if (GM.actReturn)
        {
            yield return Mirror.GetComponent<CardController>().StartCoroutine("Mirrorcheck");
            GM.GMSelectPhaze = true;
            GM.SelectableList.Clear();
            GM.Mirror();
            yield return new WaitWhile(() => GM.GMSelectPhaze); // flg がtrueになったら再開
            Debug.Log("finish");
            GM.Mirror2();
            // string MirrorPorE = "MF";
            // if (a.model.CardPlace < 5)
            // {
            //     MirrorPorE = "MF";
            // }
            // if (a.model.CardPlace > 9)
            // {
            //     MirrorPorE = "NF";
            // }
            if (GM.ImMorN)
            {
                GM.photonView.RPC("CreateCard", RpcTarget.All, GM.MirrorSelectedObj.GetComponent<CardController>().ID
                , Num, "MF", "M" + GM.HandNameNum);
            }
            else
            {
                GM.photonView.RPC("CreateCard", RpcTarget.All, GM.MirrorSelectedObj.GetComponent<CardController>().ID
                , Num, "NF", "N" + GM.HandNameNum);
            }
            GM.HandNameNum += 1;
            GameManager.instance.GMDestroyCard(a.gameObject.name);
        }
        foreach (GameObject c in GameManager.instance.SelectableList)
        {
            var v = c.GetComponent<CardController>();
            v.frame(v.model.canAttack, false);
        }
        GameManager.instance.SelectableList.Clear();
        yield return null;
    }
}
