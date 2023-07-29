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
    public bool DPclickmode;
    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        DPclickmode = false;
    }
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        if (!GM.isMyTurn) return;
        if (GM.GMSelectPhaze == true) return;
        if (eventData.pointerDrag.GetComponent<CardMovement>() == null) return;
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        var v = eventData.pointerDrag.GetComponent<CardController>().model;
        if (card == null || v == null) return;
        if (v.CTM == "Mirror")
        {
            Mirror = eventData.pointerDrag;
            StartCoroutine("MirrorWait");
        }
        else if (v.CTM != "Magic")
        {
            if (GameManager.instance.CanSummon(v, Num))
            {
                if (v.CTM == "Monster")
                {
                    GM.MonsterSummon(Num, card.gameObject.name);
                }
            }
            else
            {
                Debug.Log("CantSummon");
                card.CantSummon();
            }
        }
    }
    IEnumerator MirrorWait()//OnDropから
    {
        var a = Mirror.GetComponent<CardController>();
        GM.ActChecker = false;
        // GM.Activation(a.model.MastersCard, a.model.NeedMana);
        // yield return new WaitUntil(() => GM.ActChecker);//マナの支払いを待機
        if (GM.Activation(a.model.MastersCard, a.model.NeedMana))
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
    public void Click()
    {
        if (DPclickmode == true)
        {
            GameManager.instance.MirrorSelectFinish(Num);
        }
    }
}
