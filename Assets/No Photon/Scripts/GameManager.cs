using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, enemyHand;
    [SerializeField] public List<Transform> FieldList;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;
    bool isMasterTurn = true;
    public bool isMyTurn = true;
    List<int> deck = new List<int>() { 1, 2, 4, 1, 2, 4, 1, 2, 4, 1, 2, 4 };
    List<int> deck2 = new List<int>() { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
    public static GameManager instance;

    public int playerLeaderHP;//変更予定
    public int enemyLeaderHP;//変更予定
    public List<GameObject> SelectableList;
    public GameObject WhatDropPlace;
    public static bool GMSelectPhaze = false;

    public bool ImMorN;//自分がマスタークライアントならP、違うならEとして判定される
    public List<int> MasDeck = new List<int>() { };
    public List<int> Mashand = new List<int>() { };
    public List<int> NoDeck = new List<int>() { };
    public List<int> Nohand = new List<int>() { };
    //PH　
    public void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        if (instance == null)
        {
            instance = this;
        }
        GameObject akst = GameObject.Find("MMV");

        if (akst.GetComponent<MatchmakingView>().gameRuleProp["FS"] is int value)
        {
            int FS1 = (int)akst.GetComponent<MatchmakingView>().gameRuleProp["FS"];
            if (FS1 == 1)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("あなたは先行です-1");
                    isMyTurn = true;
                    isMasterTurn = true;
                }
                else
                {
                    Debug.Log("あなたは後行です-1");
                    isMyTurn = false;
                    isMasterTurn = true;
                }
            }
            else
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("あなたは後行です-2");
                    isMyTurn = false;
                    isMasterTurn = false;
                }
                else
                {
                    Debug.Log("あなたは先行です-2");
                    isMyTurn = true;
                    isMasterTurn = false;
                }
            }
        }
        NoDeck = GameObject.Find("MMV").GetComponent<MatchmakingView>().MMVNoDeck;
        MasDeck = GameObject.Find("MMV").GetComponent<MatchmakingView>().MMVMasDeck;
        if (PhotonNetwork.IsMasterClient)
        {
            ImMorN = true; //PEdeck.Add("Pldeck", HL.ConvertAll<object>(item => (object)item).ToArray());

        }
        else
        {
            ImMorN = false;
        }
        StartGame();
    }
    void StartGame()
    {
        enemyLeaderHP = 25;
        playerLeaderHP = 25;
        ShowLeaderHP();
        // // 初期手札を配る
        SetStartHand();

        // // ターンの決定
        TurnCalc();
    }
    void DrawCard(string DrawMN) // カードを引く
    {
        if (DrawMN == "MH")//Masterにドロー   
        {
            if (MasDeck.Count == 0)
            {
                return;
            }
            if (Mashand.Count < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = MasDeck[0];
                MasDeck.RemoveAt(0);
                photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 20, "MH");
            }
        }
        else//NOにドロー   
        {
            if (NoDeck.Count == 0)///////         
            {
                return;
            }
            Debug.Log(Nohand.Count);
            if (Nohand.Count < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = NoDeck[0];
                NoDeck.RemoveAt(0);
                photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 21, "NH");
            }

        }
    }
    void SetStartHand() // 手札を3枚配る
    {
        for (int i = 0; i < 5; i++)
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                DrawCard("MH");
            }
            else
            {
                DrawCard("NH");
            }
        }

    }
    void TurnCalc() // ターンを管理する
    {
        if (isMasterTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }
    public void ChangeTurn() // ターンエンドボタンにつける処理
    {
        isMasterTurn = !isMasterTurn; // ターンを逆にする
        TurnCalc(); // ターンを相手に回す
        isMyTurn = !isMyTurn;
    }
    void PlayerTurn()//Masterに関係なく自分のターン
    {
        Debug.Log("Playerのターン");
        if (isMasterTurn)
        {
            //DrawCard("MH"); // 手札を一枚加える
        }
        else
        {
            //DrawCard("NH"); // 手札を一枚加える
        }
        for (int i = 0; i < 5; i++)
        {
            if (FieldList[i].transform.childCount != 0)
            {
                FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
            }
        }
    }
    void EnemyTurn()//Masterに関係なく相手のターン
    {
        Debug.Log("Enemyのターン");
        if (isMasterTurn)
        {
            DrawCard("MH"); // 手札を一枚加える
        }
        else
        {
            DrawCard("NH"); // 手札を一枚加える
        }
        // CardController[] enemyFieldCardList = FieldList[0].GetComponentsInChildren<CardController>();
        // for (int i = 10; i < 15; i++)
        // {
        //     if (FieldList[i].transform.childCount == 0)
        //     {
        //         //CreateCard(1, i, "EF");
        //         break;
        //     }
        // }
        //ChangeTurn(); // ターンエンドする
    }
    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // 攻撃カードがアタック可能でなければ攻撃しないで処理終了する
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            return;
        }
        attackCard.model.Defence -= defenceCard.model.power;
        defenceCard.model.Defence -= attackCard.model.power;
        if (attackCard.model.Defence <= 0)
        {
            defenceCard.DestroyCard(attackCard);
        }
        if (defenceCard.model.Defence <= 0)
        {
            defenceCard.DestroyCard(defenceCard);
        }
        attackCard.model.canAttack = false;

    }
    public void AttackToLeader(CardController attackCard, bool isPlayerCard)
    {
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        enemyLeaderHP -= attackCard.model.power;

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("敵のHPは、" + enemyLeaderHP);
        ShowLeaderHP();
    }
    public void ShowLeaderHP()
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }
        playerLeaderHPText.text = "HP:" + playerLeaderHP.ToString();
        enemyLeaderHPText.text = "HP:" + enemyLeaderHP.ToString();
    }

    public void MirrorSelect()
    {
        GMSelectPhaze = true;
        SelectableList.Clear();
        for (int i = 0; i < 5; i++)
        {
            if (FieldList[i].transform.childCount != 0)
            {
                FieldList[i].GetChild(0).GetComponent<CardController>().model.MSelectable = true;
                SelectableList.Add(FieldList[i].GetChild(0).gameObject);
                FieldList[i].GetChild(0).GetComponent<CardController>().view.SetcanSelectPanel(true);
            }
        }
    }
    public void MirrorWhatSelect(GameObject SelectedObj)
    {
        WhatDropPlace.GetComponent<DropPlace>().MirrorObj = SelectedObj;
        WhatDropPlace.GetComponent<DropPlace>().SelectPhaze = false;
        GMSelectPhaze = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(GMSelectPhaze);
        }
    }
    [PunRPC]
    public void CreateCard(int cardID, int NumI, string PorE)
    {
        CardController card = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject card2 = PhotonNetwork.Instantiate("Card-Field", new Vector3(0, 0, 0), Quaternion.identity);
        // CardController card = card2.GetComponent<CardController>();
        if (PorE == "MH")//いずれ手札、フィールドなどによって分けていく
        {
            if (ImMorN)
            {
                card.Init(cardID, true, 20);//trueなら自分のカード
                card.transform.SetParent(playerHand);
            }
            else
            {
                card.Init(cardID, false, 21);
                card.transform.SetParent(enemyHand);
            }

        }
        else if (PorE == "NH")
        {
            if (ImMorN)
            {
                card.Init(cardID, false, 21);//trueなら自分のカード
                card.transform.SetParent(enemyHand);
            }
            else
            {
                card.Init(cardID, true, 20);
                card.transform.SetParent(playerHand);
            }
        }
        else if (PorE == "MF")
        {
            if (ImMorN)
            {
                card.Init(cardID, true, NumI);
                card.transform.SetParent(FieldList[NumI]);
            }
            else
            {
                card.Init(cardID, false, 14 - NumI);
                card.transform.SetParent(FieldList[14 - NumI]);
            }

        }
        else if (PorE == "NF")
        {
            if (ImMorN)
            {
                card.Init(cardID, false, 14 - NumI);
                card.transform.SetParent(FieldList[14 - NumI]);

            }
            else
            {
                card.Init(cardID, true, NumI);
                card.transform.SetParent(FieldList[NumI]);
            }
        }
    }

    public void MoveCard(int beforeP, int afterP, int childNum)
    {
        int BP1 = 0; int AP1 = 0; int siblingIndex = 0;
        if (beforeP < 15)
        {
            BP1 = 14 - beforeP;
        }
        else if (beforeP > 19 && beforeP < 22)
        {
            BP1 = 21 - (beforeP - 20);
            BP1 = beforeP;
            int ii = FieldList[BP1].childCount;
            
            for (int i = 0; i < ii; i++)
            {
                Debug.Log(FieldList[BP1].GetChild(i).GetComponent<CardController>().model.MoveNumber);
                if (FieldList[BP1].GetChild(i).GetComponent<CardController>().model.MoveNumber == 1)
                {
                    Debug.Log(FieldList[BP1].GetChild(i));
                    siblingIndex = FieldList[BP1].GetChild(i).GetSiblingIndex (); 
                    FieldList[BP1].GetChild(i).GetComponent<CardController>().model.MoveNumber = 0;
                    break;
                }
            }
        }

        if (afterP < 15)
        {
            AP1 = 14 - afterP;
        }
        else if (afterP > 19 && afterP < 22)
        {
            AP1 = 21 - (afterP - 20);
        }
        photonView.RPC(nameof(PunMoveCard), RpcTarget.Others, BP1, AP1, siblingIndex);
    }
    [PunRPC]
    public void PunMoveCard(int beP, int afP, int CN)
    {
        GameObject card = FieldList[beP].GetChild(CN).gameObject;
        card.transform.SetParent(FieldList[afP]);
    }
}
