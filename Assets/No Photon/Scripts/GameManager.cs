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
    [SerializeField] CardController SelectCardPrefab;//将来的にカードを選択する効果処理をする時が来るので残す
    public bool isMasterTurn;
    public bool isMyTurn;
    public static GameManager instance;
    public List<GameObject> SelectableList;
    public bool GMSelectPhaze = false;
    public bool ImMorN;//自分がマスタークライアントならP、違うならEとして判定される
    public List<CardController> CardList = new List<CardController>();
    public List<CardController> AllCardList = new List<CardController>();
    public int HandNameNum = 0;

    public GameObject scroll2;
    public GameObject content, content2;
    public bool ClickMode = false;
    public bool ActChecker;
    public bool actReturn;
    public GameObject MirrorSelectedObj;
    public int GMSelectNum;
    public bool SelectSuccess;
    public CardController MyLeader;//自分から見て相手  Masではない
    public CardController OpLeader;//相手から見て自分　Masではない
    public bool Working;
    public bool MirrorFlag;
    public int MirrorNum;
    public Hand hand;
    public Field field;
    public Deck deck;
    public CardCreate cardCreate;
    public TurnManager turnManager;
    public Mana mana;

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
        scroll2.SetActive(false);
        deck.NoDeck = akst.MMVNoDeck;
        deck.MasDeck = akst.MMVMasDeck;
        if (PhotonNetwork.IsMasterClient)
        {
            deck.MasDeck = akst.MasDeck;
            deck.NoDeck = akst.MMVNoDeck;
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
            MyLeader.Init(-999, true, 999, true, true, false);
            OpLeader.Init(-999, false, 998, true, true, false);
        }
        else
        {
            deck.NoDeck = akst.NoDeck;
            deck.MasDeck = akst.MMVMasDeck;
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
            OpLeader.Init(-999, true, 999, true, true, false);
            MyLeader.Init(-999, false, 998, true, true, false);
        }
        Working = false;
        StartGame();
    }
    void StartGame()
    {
        ShowLeaderHP();
        // // 初期手札を配る
        SetStartHand();
        mana.StartManaSet();
        // // ターンの決定
        TurnCalc();
    }

    void SetStartHand() // 手札を3枚配る
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            for (int i = 0; i < 3; i++)
            {
                hand.DrawCard("MH");

            }
            for (int ii = 0; ii < 4; ii++)
            {
                hand.DrawCard("NH");
            }
        }
    }
    void TurnCalc() // ターンを管理する
    {
        if (isMasterTurn)
        {
            turnManager.MasterTurn();
        }
        else
        {
            turnManager.NoMasterTurn();
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
            }
            else
            {
                UIM.SetLeaderHPText(false, DsD, true);
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
    public void end(CardController ac, CardController dc)
    {
        if (dc.model.CTM == "Leader")
        {
            Vector3 v3 = field.FieldList[7].transform.localPosition;
            Vector3 v1 = field.FieldList[ac.model.CardPlace].transform.localPosition;
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
            Vector3 v3 = field.FieldList[7].transform.localPosition;
            Vector3 v1 = field.FieldList[ac.model.CardPlace].transform.localPosition;
            Vector3 v2 = field.FieldList[dc.model.CardPlace].transform.localPosition;
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
            MyLeader.model.LeaderHP = 20;
            OpLeader.model.LeaderHP = 20;
        }
        else
        {
            MyLeader.model.LeaderHP = 20;
            OpLeader.model.LeaderHP = 20;
        }
        MyLeader.model.LeaderHP = 20;
        OpLeader.model.LeaderHP = 20;
        UIM.SetLeaderHPText(true, MyLeader.model.LeaderHP, false);
        UIM.SetLeaderHPText(false, OpLeader.model.LeaderHP, false);
    }
    [PunRPC]
    public void CreateCard(int cardID, int NumI, string PorE, string cardname, bool isMI)//カード名は移動に必要らしい 起点はDropplace
    {
        cardCreate.CreateCard(cardID, NumI, PorE, cardname, isMI);
    }
    [PunRPC]
    public void PunMoveCard(int afP, string Pname)
    {
        field.MoveCard(afP, Pname);
    }
    public bool Activation(bool morn, int NeedMana)//カードの発動に対して呼ばれる
    {
        if (morn == ImMorN && isMyTurn)//自分のターンかつ自分のカード
        {
            if (ImMorN)
            {
                if (NeedMana <= mana.MasterNowMana)
                {
                    return true;
                }
            }
            else
            {
                if (NeedMana <= mana.NoMasterNowMana)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void MonsterSummon(int ap, string s)
    {
        GameObject card = GameObject.Find(s);
        card.transform.SetParent(field.FieldList[ap]);
        var v = field.FieldList[ap];
        card.transform.SetParent(v);
        Debug.Log(card.transform.parent);
        if (ImMorN)
        {
            card.GetComponent<CardController>().Move(ap);
        }
        else
        {
            card.GetComponent<CardController>().Move(14 - ap);
        }
        var t = v.GetComponent<GridLayoutGroup>();
        t.CalculateLayoutInputHorizontal();
        t.CalculateLayoutInputVertical();
        t.SetLayoutHorizontal();
        t.SetLayoutVertical();
        photonView.RPC(nameof(PunMoveCard), RpcTarget.Others, ap, s);
    }
    public void ifDestroyed(CardController c)
    {
        CardList.Remove(c);
        AllCardList.Remove(c);
    }
    public IEnumerator CardListSet()//将来的にカードを選択する効果処理をする時が来るので残す
    {
        scroll2.SetActive(true);
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 9;
        foreach (GameObject obj in SelectableList)
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
            sc.Init(v.model.CardID, true, 100, v.model.IsFace, true, v.model.isMImage);
            v.IndividualNumber = i;
            sc.IndividualNumber = i;
            i += 1;
        }
        yield return null;
    }
    public void Mirror()//コピー対象を選ぶ処理
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
    public void Mirror2()//枠を外す処理
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
    public void MirrorPutSelect()
    {
        foreach (DropPlace d in field.MirrorFieldList)
        {
            if (d.transform.childCount == 0)
            {
                d.DPclickmode = true;
            }
        }
    }
    public void MirrorSelectFinish(int i)
    {
        MirrorNum = i;
        foreach (DropPlace d in field.MirrorFieldList)
        {
            d.DPclickmode = false;
            //光るみたいな処理
        }
        MirrorFlag = true;
    }
    public void CreateMirror(int NumI, bool PorE, bool isMI)//位置、PorE
    {
        if (ImMorN)
        {
            photonView.RPC(nameof(PunCreateMirror), RpcTarget.All, NumI, PorE, "M" + HandNameNum, isMI);
        }
        else
        {
            photonView.RPC(nameof(PunCreateMirror), RpcTarget.All, NumI, PorE, "N" + HandNameNum, isMI);
        }
    }
    [PunRPC]
    public void PunCreateMirror(int NumI, bool PorE, string MirrorName, bool isMImage)//カード名は移動に必要らしい 起点はDropplace
    {
        cardCreate.CreateMirror(NumI, PorE, MirrorName, isMImage);
    }
    public void DoMirror()//Master側のみが行う？ コピーを作成する処理
    {
        foreach (DropPlace d in field.MirrorFieldList)
        {
            if (d.transform.childCount != 0)
            {
                if (d.Num > 21 && d.Num < 28)//Master側
                {
                    for (int i = 22; i < 28; i++)
                    {
                        if (field.MirrorFieldList[i - 22].transform.childCount != 0)
                        {
                            if (2 * d.Num - i >= 0 && 2 * d.Num - i <= 6)
                            {
                                if (field.MirrorFieldList[2 * d.Num - i].transform.childCount == 0)
                                {
                                    cardCreate.CreateMirror(2 * d.Num - i, true, "M" + HandNameNum, true);
                                    //コピーされた鏡を置く処理
                                    HandNameNum += 1;
                                }
                            }
                        }
                    }
                    for (int ii = 0; ii < 5; ii++)
                    {
                        if (field.FieldList[ii].transform.childCount != 0)
                        {
                            if (2 * d.Num - 45 - ii >= 0 && 2 * d.Num - 45 - ii <= 4)
                            {
                                if (field.FieldList[2 * d.Num - 45 - ii].childCount == 0)
                                {
                                    var v = field.FieldList[ii].GetChild(0).GetComponent<CardController>().model;
                                    cardCreate.CreateCard(v.CardID, 2 * d.Num - 45 - ii, "MF", "M" + HandNameNum, true);
                                    //コピーされたカードを置く処理
                                    HandNameNum += 1;
                                }
                            }
                        }
                    }
                }
                if (d.Num > 33 && d.Num < 40)//非Master側
                {
                    for (int i = 34; i < 40; i++)
                    {
                        if (field.MirrorFieldList[i - 22].transform.childCount != 0)
                        {
                            if (2 * d.Num - i >= 7 && 2 * d.Num - i <= 12)//ほんとにこの数値かは謎
                            {
                                // if (field.MirrorFieldList[2 * d.Num - i].transform.childCount == 0)
                                // {
                                //     //コピーされた鏡を置く処理
                                //     HandNameNum += 1;
                                // }
                            }
                        }
                    }
                    for (int ii = 10; ii < 15; ii++)
                    {
                        if (field.FieldList[ii].transform.childCount != 0)
                        {
                            if (2 * d.Num - 49 - ii >= 10 && 2 * d.Num - 49 - ii <= 15)
                            {
                                if (field.FieldList[2 * d.Num - 49 - ii].childCount == 0)
                                {
                                    var v = field.FieldList[ii].GetChild(0).GetComponent<CardController>().model;
                                    cardCreate.CreateCard(v.CardID, 2 * d.Num - 49 - ii, "NF", "M" + HandNameNum, true);
                                    //コピーされたカードを置く処理
                                    HandNameNum += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public bool isPlayerField(int DropPlaceNumber)
    {
        return 0 <= DropPlaceNumber && DropPlaceNumber <= 4;
    }
    public bool isEnemyField(int DropPlaceNumber)
    {
        return 10 <= DropPlaceNumber && DropPlaceNumber <= 14;
    }
    public bool CanSummon(CardModel v, int Num)
    {
        if (20 <= v.CardPlace && v.CardPlace <= 22
            && field.FieldList[Num].transform.childCount == 0)//フィールドからは動かせない
        {
            if ((isPlayerField(Num) && ImMorN) || (isEnemyField(Num) && !ImMorN))
            {
                if (Activation(v.MastersCard, v.NeedMana))
                {
                    return true;
                }
            }
        }
        return false;
    }
}