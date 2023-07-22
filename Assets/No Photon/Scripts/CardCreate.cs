using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCreate : MonoBehaviour
{
    [SerializeField] public Transform playerHand, enemyHand;
    [SerializeField] CardController newcardPrefab;
    public void CreateCard(int cardID, int NumI, string PorE, string cardname, bool isMI)//カード名は移動に必要らしい 起点はDropplace
    {
        var v = GameManager.instance;
        CardController card = Instantiate(newcardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject card2 = PhotonNetwork.Instantiate("Card-Field", new Vector3(0, 0, 0), Quaternion.identity);
        // CardController card = card2.GetComponent<CardController>();
        card.gameObject.name = cardname;
        if (PorE == "MH")//いずれ手札、フィールドなどによって分けていく
        {
            if (v.ImMorN)
            {
                card.Init(cardID, true, 20, true, true, isMI);//trueならマスターのカード
                card.transform.SetParent(playerHand);
                card.transform.position = playerHand.transform.position;
            }
            else
            {
                card.Init(cardID, true, 20, true, false, isMI);//falseなら非のカード
                card.transform.SetParent(enemyHand);
                card.transform.position = enemyHand.transform.position;
            }

        }
        else if (PorE == "NH")
        {
            if (v.ImMorN)
            {
                card.Init(cardID, false, 21, true, false, isMI);///falseなら非のカード
                card.transform.SetParent(enemyHand);
                card.transform.position = enemyHand.transform.position;

            }
            else
            {
                card.Init(cardID, false, 21, true, true, isMI);//数値はMaster側なら必ず20、違うなら必ず21
                card.transform.SetParent(playerHand);
                card.transform.position = playerHand.transform.position;
            }
        }
        else if (PorE == "MF")
        {
            if (v.ImMorN)
            {
                card.Init(cardID, true, NumI, true, true, isMI);
                card.transform.SetParent(v.field.FieldList[NumI]);
                card.transform.position = v.field.FieldList[NumI].position;
            }
            else
            {
                card.Init(cardID, true, NumI, true, true, isMI);
                card.transform.SetParent(v.field.FieldList[14 - NumI]);
                card.transform.position = v.field.FieldList[14 - NumI].position;
            }

        }
        else if (PorE == "NF")
        {
            if (v.ImMorN)
            {
                card.Init(cardID, true, NumI, true, true, isMI);
                card.transform.SetParent(v.field.FieldList[NumI]);
                card.transform.position = v.field.FieldList[NumI].position;
            }
            else
            {
                card.Init(cardID, true, NumI, true, true, isMI);
                card.transform.SetParent(v.field.FieldList[14 - NumI]);
                card.transform.position = v.field.FieldList[14 - NumI].position;
            }
        }
        card.transform.localScale = new Vector3(1, 1, 1);
        v.CardList.Add(card);
        v.AllCardList.Add(card);
    }
    public void CreateMirror(int NumI, bool PorE, string MirrorName, bool isMImage)//カード名は移動に必要らしい 起点はDropplace
    {
        var v = GameManager.instance;
        CardController card = Instantiate(newcardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        card.gameObject.name = MirrorName;//識別番号付ける
        if (PorE)//マスター側に置く
        {
            if (v.ImMorN)
            {
                card.MirrorInit(-1, true, NumI, isMImage);
                card.transform.SetParent(v.field.FieldList[NumI]);
                card.transform.position = v.field.FieldList[NumI].position;
            }
            else
            {
                card.MirrorInit(-1, true, NumI, isMImage);
                card.transform.SetParent(v.field.FieldList[61 - NumI]);
                card.transform.position = v.field.FieldList[61 - NumI].position;
            }

        }
        else
        {
            if (v.ImMorN)
            {
                card.MirrorInit(-1, false, NumI, isMImage);
                card.transform.SetParent(v.field.FieldList[61 - NumI]);
                card.transform.position = v.field.FieldList[61 - NumI].position;

            }
            else
            {
                card.MirrorInit(-1, false, NumI, isMImage);
                card.transform.SetParent(v.field.FieldList[NumI]);
                card.transform.position = v.field.FieldList[NumI].position;
            }
        }
        card.transform.localScale = new Vector3(1, 1, 1);
        v.CardList.Add(card);
        v.AllCardList.Add(card);
        if (v.ImMorN)
        {
            v.DoMirror();
        }
    }
}
