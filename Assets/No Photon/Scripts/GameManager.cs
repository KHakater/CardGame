using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CardController cardPrefab;
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
    public List<GameObject> MasObjhand = new List<GameObject>() { };
    public List<GameObject> NoObjhand = new List<GameObject>() { };
    public int HandNameNum = 0;

    public GameObject scroll;
    public GameObject content;
    public bool ClickMode = false;
    public bool ifFinish;
    public GameObject test1;
    public GameObject test2;
    public GameObject test3;
    public void Awake()
    {
        HandNameNum = 0;
        PhotonNetwork.IsMessageQueueRunning = true;
        if (instance == null)//GameManager.instanceで利用できるように！！
        {
            instance = this;
        }
        GameObject akst = GameObject.Find("MMV");
        scroll.SetActive(false);
        if (akst.GetComponent<MatchmakingView>() != null)
        {
            test1.SetActive(true);
        }
        if (akst.GetComponent<MatchmakingView>().rndc["FS"] is int value)
        {
            test2.SetActive(true);
            int FS1 = (int)akst.GetComponent<MatchmakingView>().rndc["FS"];
            if (FS1 == 1)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("あなたは先行です-master");
                    isMyTurn = true;
                    isMasterTurn = true;
                }
                else
                {
                    Debug.Log("あなたは後行です-master");
                    isMyTurn = false;
                    isMasterTurn = true;
                }
            }
            else
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("あなたは後行です-no");
                    isMyTurn = false;
                    isMasterTurn = false;
                }
                else
                {
                    Debug.Log("あなたは先行です-no");
                    isMyTurn = true;
                    isMasterTurn = false;
                }
            }
        }
        else
        {
            test3.SetActive(true);
        }
        NoDeck = akst.GetComponent<MatchmakingView>().MMVNoDeck;
        MasDeck = akst.GetComponent<MatchmakingView>().MMVMasDeck;
        if (PhotonNetwork.IsMasterClient)
        {
            ImMorN = true; //PEdeck.Add("Pldeck", HL.ConvertAll<object>(item => (object)item).ToArray());

        }
        else
        {
            ImMorN = false;
        }
        ManaDic = new Dictionary<int, Manacontroller>() { };
        noManaDic = new Dictionary<int, Manacontroller>() { };
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
        else//NOにドロー   
        {
            if (NoDeck.Count == 0)///////         
            {
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
        Debug.Log("a");
    }
    void MasterTurn()//Masterのターン
    {
        if (ImMorN)
        {
            DrawCard("MH"); // 手札を一枚加える
        }
        else
        {
            DrawCard("NH"); // 手札を一枚加える
        }
        if (isMyTurn)
        {
            for (int i = 0; i < 5; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
    }
    void NoMasterTurn()//Masterに関係なく相手のターン
    {
        if (ImMorN)
        {
            DrawCard("NH"); // 手札を一枚加える
        }
        else
        {
            DrawCard("MH"); // 手札を一枚加える
        }
        if (isMyTurn)
        {
            for (int i = 0; i < 5; i++)
            {
                if (FieldList[i].transform.childCount != 0)
                {
                    FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
    }
    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // 攻撃カードがアタック可能でなければ攻撃しないで処理終了する
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        if (attackCard.model.MastersCard == defenceCard.model.MastersCard)//攻撃カードと防御カードのコントローラーが同じ場合処理終了
        {
            return;
        }
        photonView.RPC(nameof(PunBattle), RpcTarget.All, attackCard, defenceCard);
    }
    [PunRPC]
    public void PunBattle(CardController ac, CardController dc)
    {//Select,Damage,Endの順に実行
        Select(ac, dc);
    }
    public void Select(CardController ac, CardController dc)
    {
        Damage(ac, dc);
    }
    public void Damage(CardController ac, CardController dc)
    {
        //返り値のある関数を実行、与えるダメージを算出した後、そのダメージを与える
        int AsD = ac.model.Defence;
        int DsD = dc.model.Defence;
        int Adam = ac.model.power;
        int Ddam = dc.model.power;
        AsD -= Ddam;
        DsD -= Adam;
        ac.StatusChange(Adam, AsD);
        dc.StatusChange(Adam, AsD);
    }
    public void end(CardController ac, CardController dc)
    {
        //死んでも発生する処理
        if (ac.model.Defence <= 0)
        {
            ac.DestroyCard(ac);
        }
        if (dc.model.Defence <= 0)
        {
            dc.DestroyCard(dc);
        }
        ac.frame(false, ac.model.MSelectable);
        dc.frame(false, dc.model.MSelectable);
        //死んだら発生しない処理？
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
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, true);
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 9000, 1, false);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, true);
            photonView.RPC(nameof(ManaCreate), RpcTarget.All, 1000, 1, false);
        }
    }
    [PunRPC]
    public void CreateCard(int cardID, int NumI, string PorE, string cardname)
    {
        CardController card = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject card2 = PhotonNetwork.Instantiate("Card-Field", new Vector3(0, 0, 0), Quaternion.identity);
        // CardController card = card2.GetComponent<CardController>();
        card.gameObject.name = cardname;
        if (PorE == "MH")//いずれ手札、フィールドなどによって分けていく
        {
            if (ImMorN)
            {
                card.Init(cardID, true, 20);//trueならマスターのカード
                card.transform.SetParent(playerHand);
            }
            else
            {
                card.Init(cardID, true, 21);//falseなら非のカード
                card.transform.SetParent(enemyHand);
            }

        }
        else if (PorE == "NH")
        {
            if (ImMorN)
            {
                card.Init(cardID, false, 21);///falseなら非のカード
                card.transform.SetParent(enemyHand);
            }
            else
            {
                card.Init(cardID, false, 20);
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
                card.Init(cardID, true, 14 - NumI);
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
                card.Init(cardID, false, NumI);
                card.transform.SetParent(FieldList[NumI]);
            }
        }
    }

    public void MoveCard(int afterP, string PPname)
    {
        photonView.RPC(nameof(PunMoveCard), RpcTarget.Others, afterP, PPname);
    }
    [PunRPC]
    public void PunMoveCard(int afP, string Pname)
    {
        if (afP < 15)
        {
            afP = 14 - afP;
        }
        else if (afP > 19 && afP < 22)
        {
            afP = 21 - (afP - 20);
        }
        GameObject card = GameObject.Find(Pname);
        card.transform.SetParent(FieldList[afP]);
    }
    [PunRPC]
    public void ManaCreate(int Mnum, int useful, bool MasCard)//usefulが１で使える状態で追加　０で使えない状態で追加
    {
        if (MasCard)
        {
            if (ManaDic.TryGetValue(Mnum, out var item))//既に存在する種類のマナの場合、最大数を増やす
            {
                item.model.maxmana += 1;
                ManaDic[Mnum].model.nowmana += useful;
                ManaDic[Mnum].view.Show(ManaDic[Mnum].model);
            }
            else
            {
                Manacontroller mana = Instantiate(ManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                if (ImMorN)
                {
                    mana.Init(Mnum, true, 1, useful);//どちらのプレイヤーの情報か
                    mana.transform.SetParent(playerMana);
                    ManaDic.Add(Mnum, mana);
                }
                else
                {
                    mana.Init(Mnum, false, 1, useful);
                    mana.transform.SetParent(enemyMana);
                    ManaDic.Add(Mnum, mana);
                }
            }
        }
        else
        {
            if (noManaDic.TryGetValue(Mnum, out var item2))//既に存在する種類のマナの場合、最大数を増やす
            {
                noManaDic[Mnum].model.maxmana += 1;
                noManaDic[Mnum].model.nowmana += useful;
                noManaDic[Mnum].view.Show(noManaDic[Mnum].model);
            }
            else
            {
                Manacontroller mana = Instantiate(ManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                if (ImMorN)
                {
                    mana.Init(Mnum, false, 1, useful);//どちらのプレイヤーの情報か
                    mana.transform.SetParent(enemyMana);
                    noManaDic.Add(Mnum, mana);
                }
                else
                {
                    mana.Init(Mnum, true, 1, useful);
                    mana.transform.SetParent(playerMana);
                    noManaDic.Add(Mnum, mana);
                }
            }
        }
    }

    public void Activation(bool morn, List<int> NeedMana, int afterP, string PPname)//カードの発動に対して呼ばれる
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
            StartCoroutine(Col1(morn, NeedMana, afterP, PPname));
        }
        else
        {
            Debug.Log(morn.ToString() + ImMorN.ToString() + isMyTurn.ToString());
        }
    }
    public void ColorSet(int colornum)//1とか9とか色が代入される
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var value in TempDic)//一時的マナリストの中から
        {
            if (value.Value.model.color.Contains(colornum))//支払うべきマナと同色のマナを検索
            {
                Manacontroller smana = Instantiate(SelectManaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                smana.transform.SetParent(content.transform);//条件に合うマナを作成・表示
                smana.Init(value.Key, true, value.Value.model.maxmana, value.Value.model.nowmana);
            }
        }
        Debug.Log(colornum);
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
            TempDic[ID].model.nowmana -= 1;
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
                TempDic[v].model.nowmana += 1;
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
    IEnumerator Col1(bool b, List<int> needL, int ap, string s)//大きなループ　支払い終わったら返す
    {
        while (PayOrder.Count < needL.Count)
        {
            ColorSet(needL[PayOrder.Count]);
            ClickMode = true;
            yield return new WaitWhile(() => ClickMode);
        }
        if (needL.Count == PayOrder.Count)
        {
            MoveCard(ap, s);
            GameObject card = GameObject.Find(s);
            card.transform.SetParent(FieldList[ap]);
            card.GetComponent<CardController>().model.CardPlace = ap;
            if (b)//テンポラリーマナリストを適用
            {
                ManaDic = TempDic;
                foreach (var m in ManaDic)
                {
                    m.Value.view.Show(m.Value.model);
                }
            }
            else
            {
                noManaDic = TempDic;
                foreach (var n in noManaDic)
                {
                    n.Value.view.Show(n.Value.model);
                }
            }
            scroll.SetActive(false);
        }
        else//キャンセルされた場合      
        {
            scroll.SetActive(false);
        }
        yield break;
        //yield return new WaitUntil(() => PayManaDic.Count >= i);
    }
}