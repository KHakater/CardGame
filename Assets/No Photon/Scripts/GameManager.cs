using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Effekseer;

public class GameManager : MonoBehaviourPunCallbacks
{
    public UIManager UIM;
    [SerializeField] CardController newcardPrefab;
    [SerializeField] CardController SelectCardPrefab;
    [SerializeField] Manacontroller ManaPrefab;
    [SerializeField] Manacontroller SelectManaPrefab;
    [SerializeField] Transform playerHand, enemyHand;
    [SerializeField] Transform playerMana, enemyMana;
    [SerializeField] public List<Transform> FieldList;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;
    public Dictionary<int, Manacontroller> ManaDic, noManaDic, TempDic, PayManaDic = new Dictionary<int, Manacontroller>();
    public Dictionary<int, int> PayQuantity = new Dictionary<int, int>();
    public List<int> PayOrder = new List<int>();
    public bool isMasterTurn;
    public bool isMyTurn;
    public static GameManager instance;
    public List<GameObject> SelectableList;
    public bool GMSelectPhaze = false;

    public bool ImMorN;//自分がマスタークライアントならP、違うならEとして判定される
    public List<int> MasDeck = new List<int>() { };
    public List<int> Mashand = new List<int>() { };
    public List<int> NoDeck = new List<int>() { };
    public List<int> Nohand = new List<int>() { };
    public List<GameObject> MasObjhand = new List<GameObject>() { };
    public List<GameObject> NoObjhand = new List<GameObject>() { };
    public List<CardController> CardList = new List<CardController>();
    public List<CardController> AllCardList = new List<CardController>();
    public int HandNameNum = 0;

    public GameObject scroll, scroll2;
    public GameObject content, content2;
    public bool ClickMode = false;
    public bool ifFinish;
    public bool ActChecker;
    public bool actReturn;
    public GameObject MirrorSelectedObj;
    public int GMSelectNum;
    public bool SelectSuccess;
    public CardController MyLeader;//自分から見て相手  Masではない
    public CardController OpLeader;//相手から見て自分　Masではない

    [SerializeField] public EffekseerEffectAsset effect;
    public void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        HandNameNum = 0;
        GMSelectPhaze = false;
        if (instance == null)//GameManager.instanceで利用できるように！！
        {
            instance = this;
        }
        var akst = MatchmakingView.instance;
        scroll.SetActive(false);
        scroll2.SetActive(false);
        NoDeck = akst.MMVNoDeck;
        MasDeck = akst.MMVMasDeck;
        if (PhotonNetwork.IsMasterClient)
        {
            MasDeck = akst.MasDeck;
            NoDeck = akst.MMVNoDeck;
            ImMorN = true; //PEdeck.Add("Pldeck", HL.ConvertAll<object>(item => (object)item).ToArray());
            if (akst.rnd == 1)
            {
                Debug.Log("あなたは先行です-Mas");
                isMyTurn = true;
                isMasterTurn = true;
            }
            else
            {
                Debug.Log("あなたは後行です-Mas");
                isMyTurn = false;
                isMasterTurn = false;
            }
            MyLeader.Init(-999, true, 999, true, true);
            OpLeader.Init(-999, false, 998, true, true);
        }
        else
        {
            NoDeck = akst.NoDeck;
            MasDeck = akst.MMVMasDeck;
            ImMorN = false;
            if (PhotonNetwork.CurrentRoom.CustomProperties["FS"] is int value)
            {
                //int FS1 = (int)akst.rndc["FS"];
                //int FS1 = (int)CustomRnd; object型
                int FS1 = value;
                if (FS1 == 1)//マスターが先行
                {
                    Debug.Log("あなたは後行です-no");
                    isMyTurn = false;
                    isMasterTurn = true;
                }
                else
                {
                    Debug.Log("あなたは先行です-no");
                    isMyTurn = true;
                    isMasterTurn = false;
                }
            }
            OpLeader.Init(-999, true, 999, true, true);
            MyLeader.Init(-999, false, 998, true, true);
        }
        ManaDic = new Dictionary<int, Manacontroller>() { };
        noManaDic = new Dictionary<int, Manacontroller>() { };
        StartGame();
    }
    void StartGame()
    {
        ShowLeaderHP();
        // // 初期手札を配る
        SetStartHand();

        // // ターンの決定
        TurnCalc();
    }
    public void DrawCard(string DrawMN) // カードを引く
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
                if (ImMorN)
                {
                    photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 20, "MH", "M" + HandNameNum);
                    HandNameNum += 1;
                }
                else
                {
                    photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 20, "MH", "N" + HandNameNum);
                    HandNameNum += 1;
                }

            }
        }
        else
        {
            if (NoDeck.Count == 0)///////         
            {
                Debug.Log("DeckNone");
                return;
            }
            if (Nohand.Count < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = NoDeck[0];
                NoDeck.RemoveAt(0);
                if (ImMorN)
                {
                    photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 21, "NH", "M" + HandNameNum);
                    HandNameNum += 1;
                }
                else
                {
                    photonView.RPC(nameof(CreateCard), RpcTarget.All, cardID, 21, "NH", "N" + HandNameNum);
                    HandNameNum += 1;
                }
            }

        }
    }
    void SetStartHand() // 手札を3枚配る
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCard("MH");

            }
            for (int ii = 0; ii < 4; ii++)
            {
                DrawCard("NH");
            }
        }
    }
    void TurnCalc() // ターンを管理する
    {
        if (isMasterTurn)
        {
            MasterTurn();
        }
        else
        {
            NoMasterTurn();
        }
    }
    public void ChangeTurn() // ターンエンドボタンにつける処理
    {
        if (isMyTurn)
        {
            photonView.RPC(nameof(PunChangeTurn), RpcTarget.All);
        }
    }
    [PunRPC]
    public void PunChangeTurn()
    {
        isMasterTurn = !isMasterTurn; // ターンを逆にする
        isMyTurn = !isMyTurn;
        TurnCalc(); // ターンを相手に回す
    }
    void MasterTurn()//Masterのターン
    {
        if (ImMorN)
        {
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, true);
            DrawCard("MH"); // 手札を一枚加える
            for (int i = 0; i < 5; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        else
        {
            for (int i = 10; i < 15; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        UIM.StartCoroutine("TurnStart", isMyTurn);
        foreach (var n in ManaDic)
        {//n.Value.model
            var v = n.Value;
            v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.maxmana);
        }
    }
    void NoMasterTurn()
    {
        if (!ImMorN)
        {
            DrawCard("NH"); // 手札を一枚加える
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, false);
            for (int i = 0; i < 5; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        else
        {
            for (int i = 10; i < 15; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        foreach (var n in noManaDic)
        {//n.Value.model
            var v = n.Value;
            v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.maxmana);
        }
        UIM.StartCoroutine("TurnStart", isMyTurn);
    }
    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // 攻撃カードがアタック可能でなければ攻撃しないで処理終了する
        if (attackCard.model.canAttack == false)
        {
            Debug.Log("CantBattle");
            return;
        }
        if (attackCard.model.MastersCard == defenceCard.model.MastersCard)//攻撃カードと防御カードのコントローラーが同じ場合処理終了
        {
            Debug.Log("CantBattle2");
            return;
        }
        photonView.RPC(nameof(PunBattle), RpcTarget.All, attackCard.model.CardPlace, defenceCard.model.CardPlace);
    }
    [PunRPC]
    public void PunBattle(int ac, int dc)
    {//Select,Damage,Endの順に実行
        int tempac = ac;
        int tempdc = dc;
        CardController ACC = null;
        CardController DCC = null;
        if (dc == 999 || dc == 998)
        {
            if (!ImMorN)
            {
                //tempac = 14 - tempac;
                tempdc = 998 + (999 - tempdc);
            }
            if (tempdc == 999)
            {
                DCC = MyLeader.GetComponent<CardController>();//ここから
            }
            if (tempdc == 998)
            {
                DCC = OpLeader.GetComponent<CardController>();
            }
            foreach (CardController c in CardList)
            {
                if (c.model.CardPlace == tempac)
                {
                    ACC = c;
                }
            }
        }
        else
        {
            // if (!ImMorN)
            // {
            //     tempac = 14 - tempac;
            //     tempdc = 14 - tempdc;
            // }
            foreach (CardController c in CardList)
            {
                if (c.model.CardPlace == tempac)
                {
                    ACC = c;
                }
                if (c.model.CardPlace == tempdc)
                {
                    DCC = c;
                }
            }
        }
        Select(ACC, DCC);
    }
    public void Select(CardController ac, CardController dc)
    {
        Damage(ac, dc);
    }
    public void Damage(CardController ac, CardController dc)
    {
        int AsD;
        int DsD;
        int Adam;
        int Ddam;
        if (dc.model.CTM == "Leader")
        {
            DsD = dc.model.LeaderHP;
            Debug.Log(ac.ToString());
            Debug.Log(ac.model.CTM);
            Adam = ac.model.power;
            DsD -= Adam;
            if (DsD <= 0)
            {
                DsD = 0;
                if ((dc.model.CardPlace == 999) == (ImMorN))
                {
                    UIM.WinLose(false);
                }
                else
                {
                    UIM.WinLose(true);
                }
            }
            dc.LeaderHPChange(DsD);
            if ((dc.model.CardPlace == 999) == (ImMorN))//999はMaster 攻撃対象が自分側なら
            {
                UIM.SetLeaderHPText(true, DsD, true);
                //photonView.RPC(nameof(SetLeaderHP), RpcTarget.Others, DsD, false);
            }
            else
            {
                UIM.SetLeaderHPText(false, DsD, true);
                //photonView.RPC(nameof(SetLeaderHP), RpcTarget.Others, DsD, true);
            }
            end(ac, dc);
        }
        else
        {
            AsD = ac.model.Defence; DsD = dc.model.Defence; Adam = ac.model.power; Ddam = dc.model.power;
            AsD -= Ddam;//返り値のある関数を実行、与えるダメージを算出した後、そのダメージを与える
            DsD -= Adam;
            ac.StatusChange(Adam, AsD);
            dc.StatusChange(Ddam, DsD);
            Debug.Log(ac.ToString() + "--" + dc.ToString());
            end(ac, dc);
        }
    }
    [PunRPC]
    public void SetLeaderHP(int DsD, bool b)
    {
        UIM.SetLeaderHPText(b, DsD, true);
    }
    public void end(CardController ac, CardController dc)
    {
        if (dc.model.CTM == "Leader")
        {
            Vector3 v3 = FieldList[7].transform.localPosition;
            Vector3 v1 = FieldList[ac.model.CardPlace].transform.localPosition;
            Vector3 v2;
            if ((dc.model.CardPlace == 999) == (ImMorN))
            {
                v2 = MyLeader.transform.localPosition;
            }
            else
            {
                v2 = OpLeader.transform.localPosition;
            }
            if (ImMorN)
            {
                UIM.AttackMove(v1, v2, v3);
            }
            else
            {
                UIM.AttackMove(v3, v2, v1);
            }
            //死んでも発生する処理
            if (ac.model.Defence <= 0)
            {
                ac.DestroyCard();
            }
            ac.frame(false, ac.model.MSelectable);
        }
        else
        {
            Vector3 v3 = FieldList[7].transform.localPosition;
            Vector3 v1 = FieldList[ac.model.CardPlace].transform.localPosition;
            Vector3 v2 = FieldList[dc.model.CardPlace].transform.localPosition;
            Debug.Log(v1.ToString() + " + " + v3.ToString() + " + " + v2.ToString());
            if (ImMorN)
            {
                UIM.AttackMove(v1, v2, v3);
            }
            else
            {
                UIM.AttackMove(v3, v2, v1);
            }
            //死んでも発生する処理
            if (ac.model.Defence <= 0)
            {
                ac.DestroyCard();
            }
            if (dc.model.Defence <= 0)
            {
                dc.DestroyCard();
            }
            ac.frame(false, ac.model.MSelectable);
            dc.frame(false, dc.model.MSelectable);
        }
        //死んだら発生しない処理？
    }
    public void ShowLeaderHP()
    {
        if (ImMorN)
        {
            MyLeader.model.LeaderHP = 12;
            OpLeader.model.LeaderHP = 13;
            for (int i = 0; i < 5; i++)
            {
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, true);
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, true);
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 2000, 1, true);
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, false);
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, false);
                photonView.RPC(nameof(ManaCreate), RpcTarget.All, 2000, 1, false);
            }
        }
        else
        {
            MyLeader.model.LeaderHP = 13;
            OpLeader.model.LeaderHP = 12;
        }
        MyLeader.model.LeaderHP = 12;
        OpLeader.model.LeaderHP = 13;
        UIM.SetLeaderHPText(true, MyLeader.model.LeaderHP, false);
        UIM.SetLeaderHPText(false, OpLeader.model.LeaderHP, false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, true);
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, false);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, true);
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 2000, 1, true);
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 2000, 1, false);
        }
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     FieldList[2].GetChild(0).GetComponent<CardController>().StatusChange(8, 8);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     FieldList[2].GetChild(0).GetComponent<CardController>().StatusChange(8, 6);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     FieldList[3].GetChild(0).GetComponent<CardController>().DestroyCard(FieldList[3].GetChild(0).GetComponent<CardController>());
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     photonView.RPC(nameof(CreateCard), RpcTarget.All, 14, 1, "NF", "M" + HandNameNum);
        //     HandNameNum += 1;
        //     DrawCard("NH");
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha5))
        // {
        //     UIM.SetLeaderHPText(true, 1, true);
        //     FieldList[2].GetChild(0).GetComponent<CardController>().DestroyCard(FieldList[2].GetChild(0).GetComponent<CardController>());
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha6))
        // {
        //     CreateCard(17, 2, "MF", "M" + HandNameNum);
        //     HandNameNum += 1;
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha7))
        // {
        //     foreach (var n in noManaDic)
        //     {//n.Value.model
        //         var v = n.Value;
        //         if (v.model.CardID == 1000)
        //         {
        //             v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, 0);
        //         }
        //     }
        //     photonView.RPC(nameof(CreateCard), RpcTarget.All, 17, 0, "MF", "M" + HandNameNum);
        //     HandNameNum += 1;
        //     photonView.RPC(nameof(CreateCard), RpcTarget.All, 17, 1, "MF", "M" + HandNameNum);
        //     HandNameNum += 1;
        //     photonView.RPC(nameof(CreateCard), RpcTarget.All, 17, 3, "MF", "M" + HandNameNum);
        //     HandNameNum += 1;
        //     photonView.RPC(nameof(CreateCard), RpcTarget.All, 17, 4, "MF", "M" + HandNameNum);
        //     HandNameNum += 1;
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha8))
        // {
        //     UIM.StartCoroutine("ReverseEff");
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha9))
        // {
        //     for (int i = 0; i < 6; i++)
        //     {
        //         photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, true);
        //     }
        //     photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, false);
        //     for (int i = 0; i < 6; i++)
        //     {
        //         photonView.RPC(nameof(ManaCreate), RpcTarget.All, 2000, 1, false);
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     var v = FieldList[12].GetChild(0).GetComponent<CardController>();
        //     v.Init(v.model.CardID, v.model.MastersCard, v.model.CardPlace, !v.model.IsFace, v.model.CanSee);
        // }
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     foreach (var n in noManaDic)
        //     {//n.Value.model
        //         var v = n.Value;
        //         if (v.model.CardID == 9000)
        //         {
        //             v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, 0);
        //         }
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     foreach (var n in noManaDic)
        //     {//n.Value.model
        //         var v = n.Value;
        //         if (v.model.CardID == 2000)
        //         {
        //             v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, 0);
        //         }
        //     }
        // }
    }
    [PunRPC]
    public void CreateCard(int cardID, int NumI, string PorE, string cardname)//カード名は移動に必要らしい 起点はDropplace
    {
        CardController card = Instantiate(newcardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject card2 = PhotonNetwork.Instantiate("Card-Field", new Vector3(0, 0, 0), Quaternion.identity);
        // CardController card = card2.GetComponent<CardController>();
        card.gameObject.name = cardname;
        if (PorE == "MH")//いずれ手札、フィールドなどによって分けていく
        {
            if (ImMorN)
            {
                card.Init(cardID, true, 20, true, true);//trueならマスターのカード
                card.transform.SetParent(playerHand);
                card.transform.position = playerHand.transform.position;
            }
            else
            {
                card.Init(cardID, true, 20, true, false);//falseなら非のカード
                card.transform.SetParent(enemyHand);
                card.transform.position = enemyHand.transform.position;
            }

        }
        else if (PorE == "NH")
        {
            if (ImMorN)
            {
                card.Init(cardID, false, 21, true, false);///falseなら非のカード
                card.transform.SetParent(enemyHand);
                card.transform.position = enemyHand.transform.position;

            }
            else
            {
                card.Init(cardID, false, 21, true, true);//数値はMaster側なら必ず20、違うなら必ず21
                card.transform.SetParent(playerHand);
                card.transform.position = playerHand.transform.position;
            }
        }
        else if (PorE == "MF")
        {
            if (ImMorN)
            {
                card.Init(cardID, true, NumI, true, true);
                card.transform.SetParent(FieldList[NumI]);
                card.transform.position = FieldList[NumI].position;
            }
            else
            {
                card.Init(cardID, true, NumI, true, true);
                card.transform.SetParent(FieldList[14 - NumI]);
                card.transform.position = FieldList[14 - NumI].position;
            }

        }
        else if (PorE == "NF")
        {
            if (ImMorN)
            {
                card.Init(cardID, false, NumI, true, true);
                card.transform.SetParent(FieldList[14 - NumI]);
                card.transform.position = FieldList[14 - NumI].position;

            }
            else
            {
                card.Init(cardID, false, NumI, true, true);
                card.transform.SetParent(FieldList[NumI]);
                card.transform.position = FieldList[NumI].position;
            }
        }
        card.transform.localScale = new Vector3(1, 1, 1);
        CardList.Add(card);
        AllCardList.Add(card);
    }

    public void MoveCard(int afterP, string PPname)
    {
        photonView.RPC(nameof(PunMoveCard), RpcTarget.Others, afterP, PPname);
    }
    [PunRPC]
    public void PunMoveCard(int afP, string Pname)//
    {
        GameObject card = GameObject.Find(Pname);
        if (afP < 15)
        {
            afP = 14 - afP;
            var v = card.GetComponent<CardController>().model;
            card.GetComponent<CardController>().Init(v.CardID, v.MastersCard, 0, v.IsFace, true);
        }
        else if (afP > 19 && afP < 22)
        {
            afP = 21 - (afP - 20);
        }
        if (ImMorN)
        {
            card.GetComponent<CardController>().Move(afP);
        }
        else
        {
            card.GetComponent<CardController>().Move(14 - afP);
        }
        card.transform.SetParent(FieldList[afP]);
    }
    [PunRPC]
    public void ManaCreate(int Mnum, int useful, bool MasCard)//usefulが１で使える状態で追加　０で使えない状態で追加
    {
        if (MasCard)
        {
            if (ManaDic.TryGetValue(Mnum, out var item))//既に存在する種類のマナの場合、最大数を増やす
            {
                item.Init(item.model.CardID, item.model.MastersCard, item.model.maxmana + 1, useful + item.model.nowmana);
            }
            else
            {
                Manacontroller mana = Instantiate(ManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                if (ImMorN)
                {
                    mana.Init(Mnum, true, 1, useful);//どちらのプレイヤーの情報か
                    mana.transform.SetParent(playerMana);
                    mana.transform.position = playerMana.transform.position;
                    ManaDic.Add(Mnum, mana);
                }
                else
                {
                    mana.Init(Mnum, false, 1, useful);
                    mana.transform.SetParent(enemyMana);
                    mana.transform.position = enemyMana.transform.position;
                    ManaDic.Add(Mnum, mana);
                }
                mana.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if (noManaDic.TryGetValue(Mnum, out var item2))//既に存在する種類のマナの場合、最大数を増やす
            {
                item2.Init(item2.model.CardID, item2.model.MastersCard, item2.model.maxmana + 1, useful + item2.model.nowmana);
            }
            else
            {
                Manacontroller mana = Instantiate(ManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                if (ImMorN)
                {
                    mana.Init(Mnum, false, 1, useful);//どちらのプレイヤーの情報か
                    mana.transform.SetParent(enemyMana);
                    mana.transform.position = enemyMana.transform.position;
                    noManaDic.Add(Mnum, mana);
                }
                else
                {
                    mana.Init(Mnum, true, 1, useful);
                    mana.transform.SetParent(playerMana);
                    mana.transform.position = playerMana.transform.position;
                    noManaDic.Add(Mnum, mana);
                }
                mana.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void Activation(bool morn, List<int> NeedMana)//カードの発動に対して呼ばれる
    {
        if (morn == ImMorN && isMyTurn)//自分のターンかつ自分のカード
        {
            if (morn)//テンポラリーマナリストを作成
            {
                TempDic = ManaDic;
            }
            else
            {
                TempDic = noManaDic;
            }
            PayManaDic.Clear();
            PayQuantity.Clear();
            PayOrder.Clear();
            ifFinish = false;
            scroll.SetActive(true);
            IEnumerator corutine = Col1(morn, NeedMana);
            StartCoroutine(corutine);
        }
    }
    IEnumerator Col1(bool b, List<int> needL)//大きなループ　支払い終わったら返す 受け取るAPは絶対的位置に調整済み
    {
        while (!ifFinish)//PayOrder.Count < needL.Count
        {
            if (PayOrder.Count == needL.Count)
            {
                break;
            }
            ColorSet(needL[PayOrder.Count]);
            ClickMode = true;
            yield return new WaitWhile(() => ClickMode);
        }
        if (needL.Count == PayOrder.Count)
        {
            if (b)//テンポラリーマナリストを適用
            {
                ManaDic = TempDic;
                foreach (var m in ManaDic)
                {
                    var v = m.Value;
                    v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.nowmana);
                    photonView.RPC("RPCMana", RpcTarget.Others, v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.nowmana);
                }
            }
            else
            {
                noManaDic = TempDic;
                foreach (var n in noManaDic)
                {//n.Value.model
                    var v = n.Value;
                    v.Init(v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.nowmana);
                    photonView.RPC("RPCMana", RpcTarget.Others, v.model.CardID, v.model.MastersCard, v.model.maxmana, v.model.nowmana);
                }
            }
            scroll.SetActive(false);
            ActChecker = true;
            actReturn = true;
            yield return null;
        }
        else//キャンセルされた場合   共通  
        {
            Debug.Log("a");
            scroll.SetActive(false);
            ActChecker = true;
            actReturn = false;
            yield return null;
        }
        //yield return new WaitUntil(() => PayManaDic.Count >= i);
    }
    public void ColorSet(int colornum)//1とか9とか色が代入される ManaListから
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var value in TempDic)//一時的マナリストの中から
        {
            if (colornum / 100 == 1)//四角
            {
                if (value.Value.model.color.Contains(colornum % 100))//支払うべきマナと同色のマナを検索
                {
                    Manacontroller smana = Instantiate(SelectManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    smana.isSelectCard = true;
                    smana.transform.SetParent(content.transform);//条件に合うマナを作成・表示
                    smana.Init(value.Key, true, value.Value.model.maxmana, value.Value.model.nowmana);
                    smana.transform.localScale = new Vector3(1, 1, 1);
                    LayoutGroupSetting(smana.gameObject);
                }
            }
            else//丸
            {
                if (colornum % 100 == 9)//何で支払ってもいい
                {
                    Manacontroller smana = Instantiate(SelectManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    smana.isSelectCard = true;
                    smana.transform.SetParent(content.transform);//条件に合うマナを作成・表示
                    smana.Init(value.Key, true, value.Value.model.maxmana, value.Value.model.nowmana);
                    smana.transform.localScale = new Vector3(1, 1, 1);
                    LayoutGroupSetting(smana.gameObject);
                }
                else//色がついてるなら
                {
                    if (value.Value.model.color.Contains(colornum % 100))//支払うべきマナと同色のマナを検索
                    {
                        Manacontroller smana = Instantiate(SelectManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        smana.isSelectCard = true;
                        smana.transform.SetParent(content.transform);//条件に合うマナを作成・表示
                        smana.Init(value.Key, true, value.Value.model.maxmana, value.Value.model.nowmana);
                        smana.transform.localScale = new Vector3(1, 1, 1);
                        LayoutGroupSetting(smana.gameObject);
                    }
                    else if (value.Value.model.color.Contains(9))//虹色で支払ってもいい　その場合染色を行う
                    {
                        Manacontroller smana = Instantiate(SelectManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        smana.isSelectCard = true;
                        smana.transform.SetParent(content.transform);//条件に合うマナを作成・表示
                        smana.Init(value.Key, true, value.Value.model.maxmana, value.Value.model.nowmana);
                        smana.transform.localScale = new Vector3(1, 1, 1);
                        LayoutGroupSetting(smana.gameObject);
                        //value.Value.model.color.Clear();
                        //value.Value.model.color.Add(colornum % 100);//染色の処理はマナの支払いが確定してから！
                    }
                }
            }
        }
    }
    void LayoutGroupSetting(GameObject s)
    {
        s.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        GridLayoutGroup layoutGroup = content.GetComponent<GridLayoutGroup>();
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.SetLayoutHorizontal();
        s.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }
    public void OnButton(int ID, Manacontroller M)//追加の処理
    {
        if (ClickMode)
        {
            if (PayManaDic.TryGetValue(M.ID, out var item2))//既に支払い候補にあるマナ
            {
                var t = PayQuantity[ID];
                PayQuantity[ID] = t + 1;
            }
            else
            {
                PayManaDic.Add(ID, M);//個数の管理どうする？
                PayQuantity.Add(ID, 1);
            }
            var v = TempDic[ID].model;
            TempDic[ID].Init(v.CardID, v.MastersCard, v.maxmana, v.nowmana - 1);
            ClickMode = false;
            PayOrder.Add(ID);
        }
    }
    public void ManaBack()//戻るボタンは既に一つ以上マナを選択したあとにしか表示されないように！！
    {
        if (ClickMode)
        {
            if (PayOrder.Count > 0)
            {
                var v = PayOrder[PayOrder.Count - 1];//マナを表す四桁の数字
                var vv = TempDic[v].model;
                TempDic[v].Init(vv.CardID, true, vv.maxmana, vv.nowmana + 1);
                if (PayQuantity[v] > 1)
                {
                    var u = PayQuantity[v];
                    PayQuantity[v] = u - 1;
                }
                else
                {
                    PayManaDic.Remove(v);//最後に追加したマナを取り消し
                    PayQuantity.Remove(v);
                }
                PayOrder.RemoveAt(PayOrder.Count - 1);
                ClickMode = false;
            }
        }
    }
    public void Cancel()//マナの支払いをキャンセル
    {
        if (ClickMode)
        {
            PayManaDic.Clear();
            scroll.SetActive(false);
            ifFinish = true;
            ClickMode = false;
        }
    }
    public void MonsterSummon(List<int> needL, int ap, string s)
    {
        GameObject card = GameObject.Find(s);
        card.transform.SetParent(FieldList[ap]);
        if (ImMorN)
        {
            card.GetComponent<CardController>().Move(ap);
        }
        else
        {
            card.GetComponent<CardController>().Move(14 - ap);
        }
        MoveCard(ap, s);
    }
    public void ifDestroyed(CardController c)
    {
        CardList.Remove(c);
        AllCardList.Remove(c);
    }
    public void dup()
    {
        StartCoroutine("Duplicate");
    }
    public void rev()
    {
        StartCoroutine("Reverse");
    }
    public IEnumerator Duplicate()
    {
        if (isMyTurn && GMSelectPhaze == false)
        {
            SelectableList.Clear();
            foreach (CardController c in AllCardList)
            {
                if (c.gameObject != null)
                {
                    SelectableList.Add(c.gameObject);
                    c.frame(c.model.canAttack, true);
                }
            }
            yield return StartCoroutine("CardListSet");
            GMSelectPhaze = true;
            yield return new WaitWhile(() => GMSelectPhaze); // flg がfalseになったら再開
            if (SelectSuccess)
            {
                if (ImMorN)
                {
                    photonView.RPC("CreateCard", RpcTarget.All, MirrorSelectedObj.GetComponent<CardController>().ID
                    , 20, "MH", "M" + HandNameNum);
                    HandNameNum += 1;
                }
                else
                {
                    photonView.RPC("CreateCard", RpcTarget.All, MirrorSelectedObj.GetComponent<CardController>().ID
                    , 21, "NH", "N" + HandNameNum);
                    HandNameNum += 1;
                }
                scroll2.SetActive(false);
                UIM.StartCoroutine("DupEff");
            }
            foreach (GameObject p in SelectableList)
            {
                var vv = p.GetComponent<CardController>();
                vv.frame(vv.model.canAttack, false);
            }
            scroll2.SetActive(false);
        }
        yield return null;
    }
    public IEnumerator Reverse()
    {
        if (isMyTurn && GMSelectPhaze == false)
        {
            GMSelectPhaze = true;
            yield return StartCoroutine("ReverseCheck");
            yield return StartCoroutine("CardListSet");
            yield return new WaitWhile(() => GMSelectPhaze);
            if (SelectSuccess)
            {
                foreach (CardController c in AllCardList)
                {
                    if (c != null)
                    {
                        if (c.gameObject != null)
                        {
                            if (c.IndividualNumber == GMSelectNum)
                            {
                                MirrorSelectedObj = c.gameObject;
                            }
                        }
                    }
                }
                var v = MirrorSelectedObj.GetComponent<CardController>();
                v.Init(v.model.CardID, v.model.MastersCard, v.model.CardPlace, !v.model.IsFace, v.model.CanSee);
                scroll2.SetActive(false);
                UIM.StartCoroutine("ReverseEff");
            }
            foreach (GameObject p in SelectableList)
            {
                var vv = p.GetComponent<CardController>();
                vv.frame(vv.model.canAttack, false);
            }
            scroll2.SetActive(false);
        }
        yield return null;
    }
    IEnumerator ReverseCheck()
    {
        SelectableList.Clear();
        foreach (CardController c in AllCardList)
        {
            if (!(c.model.MastersCard == ImMorN))
            {
                continue;
            }
            if (c.model.CardPlace != 20)
            {
                if (c.model.CardPlace < 5)
                {
                    if (c.model.GetReverseCTMValue() == "Mirror" || c.model.GetReverseCTMValue() == "Magic")//自分フィールドで反転後鏡か魔法になるものはダメ
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            if (c.gameObject != null)
            {
                SelectableList.Add(c.gameObject);
                c.frame(c.model.canAttack, true);
            }
        }
        yield return null;
    }
    public IEnumerator CardListSet()
    {
        scroll2.SetActive(true);
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 9;
        foreach (GameObject obj in SelectableList)//一時的マナリストの中から
        {
            CardController sc = Instantiate(SelectCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            sc.isSelectCard = true;
            sc.transform.SetParent(content2.transform);// int cardID, bool playerCard, int cardplace,bool isfase
            sc.transform.localScale = new Vector3(1, 1, 1);
            sc.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            GridLayoutGroup layoutGroup = content2.GetComponent<GridLayoutGroup>();
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutHorizontal();
            sc.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            var v = obj.GetComponent<CardController>();
            sc.Init(v.model.CardID, true, 100, v.model.IsFace, true);
            v.IndividualNumber = i;
            sc.IndividualNumber = i;
            i += 1;
        }
        yield return null;
    }
    public void Mirror()
    {
        foreach (CardController c in AllCardList)
        {
            if (c != null)
            {
                SelectableList.Add(c.gameObject);
                //c.frame(c.model.canAttack, true);
            }
        }
        StartCoroutine("CardListSet");
    }
    public void Mirror2()
    {
        foreach (GameObject p in SelectableList)
        {
            var vv = p.GetComponent<CardController>();
            vv.frame(vv.model.canAttack, false);
        }
        scroll2.SetActive(false);
    }
    public void CardSelectCansel()
    {
        scroll2.SetActive(false);
        GMSelectPhaze = false;
        SelectSuccess = false;
    }
    [PunRPC]
    public void RPCMana(int i1, bool b1, int i2, int i3)
    {
        if (ImMorN)
        {
            foreach (KeyValuePair<int, Manacontroller> item in noManaDic)
            {
                if (item.Key == i1)
                {
                    item.Value.Init(i1, b1, i2, i3);
                    break;
                }
            }
        }
        else
        {
            foreach (KeyValuePair<int, Manacontroller> item in ManaDic)
            {
                if (item.Key == i1)
                {
                    item.Value.Init(i1, b1, i2, i3);
                    break;
                }
            }
        }
    }
    public void GMDestroyCard(string name)
    {
        photonView.RPC("GMDC", RpcTarget.All, name);
    }
    [PunRPC]
    void GMDC(string name)
    {
        var v = GameObject.Find(name);
        Debug.Log(name + "---" + v.name);
        if (v != null)
        {
            v.GetComponent<CardController>().DestroyCard();
        }
    }
}