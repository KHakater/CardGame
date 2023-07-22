using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Hand : MonoBehaviourPunCallbacks
{
    public List<int> Nohand = new List<int>() { };
    public List<int> Mashand = new List<int>() { };

    public void DrawCard(string DrawMN) // カードを引く
    {
        var v = GameManager.instance;
        if (DrawMN == "MH")//Masterにドロー   
        {
            if (v.deck.MasDeck.Count == 0)
            {
                return;
            }
            if (Mashand.Count < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = v.deck.MasDeck[0];
                v.deck.MasDeck.RemoveAt(0);
                if (v.ImMorN)
                {
                    v.photonView.RPC("CreateCard", RpcTarget.All, cardID, 20, "MH", "M" + v.HandNameNum, false);
                    v.HandNameNum += 1;
                }
                else
                {
                    v.photonView.RPC("CreateCard", RpcTarget.All, cardID, 20, "MH", "N" + v.HandNameNum, false);
                    v.HandNameNum += 1;
                }

            }
        }
        else
        {
            if (v.deck.NoDeck.Count == 0)///////         
            {
                Debug.Log("DeckNone");
                return;
            }
            if (Nohand.Count < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = v.deck.NoDeck[0];
                v.deck.NoDeck.RemoveAt(0);
                if (v.ImMorN)
                {
                    v.photonView.RPC("CreateCard", RpcTarget.All, cardID, 21, "NH", "M" + v.HandNameNum, false);
                    v.HandNameNum += 1;
                }
                else
                {
                    v.photonView.RPC("CreateCard", RpcTarget.All, cardID, 21, "NH", "N" + v.HandNameNum, false);
                    v.HandNameNum += 1;
                }
            }

        }
    }
}
