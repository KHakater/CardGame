using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public int ID;
    public int IndividualNumber = -999;
    bool boxFlag = true;
    EffectList ef;
    public bool isSelectCard = false;
    private void Awake()
    {
        view = GetComponent<CardView>();
        ef = GameManager.instance.gameObject.GetComponent<EffectList>();
    }
    public void Init(int cardID, bool playerCard, int cardplace, bool isfase, bool cansee, bool isMImage) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard, cardplace, isfase, cansee, isMImage); // カードデータを生成
        view.Show(model, isSelectCard); // 表示
        ID = cardID;
    }
    public void MirrorInit(int cardID, bool playerCard, int cardplace, bool MRot, bool none, bool isMImage) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard, cardplace, MRot, none, isMImage); // カードデータを生成
        view.Show(model, isSelectCard); // 表示
        ID = cardID;
    }
    public void DestroyCard()
    {
        GameManager.instance.ifDestroyed(this);
        Destroy(gameObject);
    }
    public void StatusChange(int attack, int hp)//攻撃と体力を指定された値に変更し、表示する
    {
        model.power = attack;
        model.Defence = hp;
        view.Show(model, isSelectCard);
    }
    public void LeaderHPChange(int HP)
    {
        model.LeaderHP = HP;
    }
    public void frame(bool canATK, bool canSelect)
    {
        model.canAttack = canATK;
        model.MSelectable = canSelect;
        view.SetCanAttackPanel(canATK);
        view.SetcanSelectPanel(canSelect);
    }
    public void Move(int pos)
    {
        model.CardPlace = pos;
    }
    public IEnumerator Mirrorcheck()
    {
        var a = GameManager.instance;
        //Debug.Log(model.MirrorRange.ToString() + "-" + model.MirrorType.ToString());
        //Debug.Log("range" + (model.MirrorRange & 1 << 0).ToString() + "-" + (model.MirrorRange & 2 << 0).ToString());
        //Debug.Log("type" + (model.MirrorType & 1 << 0).ToString() + "-" + (model.MirrorType & 2 << 0).ToString() + "-" + (model.MirrorType & 2 << 0).ToString());
        foreach (CardController c in a.AllCardList)
        {
            if (!(c.model.CardPlace < 5 && (model.MirrorRange & 1 << 0) != 0) &&
            !(c.model.CardPlace == 20 && (model.MirrorRange & 2 << 0) != 0))
            {//もっといい感じに 分ける
                continue;
            }
            // if (!(c.model.CTM == "Monster" && (model.MirrorType & 1 << 0) != 0) &&
            // !(c.model.CTM == "Magic" && (model.MirrorType & 2 << 0) != 0) &&
            // !(c.model.CTM == "Mirror" && (model.MirrorType & 3 << 0) != 0))
            // {//どれか一つでもあてはまったら続行、当てはまらなかったらcontinue
            //     continue;
            // }
            if (!(c.model.CTM == "Monster"))
            {
                continue;
            }
            a.SelectableList.Add(c.gameObject);
            //c.frame(c.model.canAttack, true);
        }
        yield break;
    }
    public void SelectButtonPush()
    {
        if (GameManager.instance.GMSelectPhaze)
        {
            if (GameManager.instance.isMyTurn)
            {
                GameManager.instance.SelectSuccess = true;
                GameManager.instance.MirrorSelectedObj = this.gameObject;
                GameManager.instance.GMSelectNum = IndividualNumber;
                GameManager.instance.GMSelectPhaze = false;
            }
        }
    }
    public void PointerDown()
    {
        if (model.CTM == "Magic")
        {
            boxFlag = true;
            view.ifOnMouse(true);
        }
    }
    public void PointerUp()
    {
        if (model.CTM == "Magic")
        {
            boxFlag = false;
            view.ifOnMouse(false);
        }
    }
    void MagicEffect()
    {
        var setMethod = ef.GetType().GetMethod(model.effects[0].effectname, new[] { typeof(List<int>) });
        Debug.Log(setMethod);
        if (setMethod != null)
        {
            setMethod.Invoke(ef, new object[] { model.effects[0].effectint }); // 引数を指定して実行
        }
        GameManager.instance.GMDestroyCard(gameObject.name);
    }
    public void ActivationButtonPush()
    {
        StartCoroutine("ColAct");
    }
    public IEnumerator ColAct()
    {
        GameManager.instance.Activation(model.MastersCard, model.Mlist);
        GameManager.instance.ActChecker = false;
        yield return new WaitUntil(() => GameManager.instance.ActChecker);//マナの支払いを待機
        if (GameManager.instance.actReturn)
        {
            MagicEffect();
        }
    }
    public void MirrorButtonPush()
    {
        if (GameManager.instance.Working == false)
        {
            StartCoroutine("MirrorColAct");
            GameManager.instance.Working = true;
        }
    }
    public IEnumerator MirrorColAct()
    {
        GameManager.instance.MirrorFlag = false;
        GameManager.instance.MirrorPutSelect();
        yield return new WaitUntil(() => GameManager.instance.MirrorFlag);
        //Mirrorを新たに生成
        GameManager.instance.CreateMirror(GameManager.instance.MirrorNum, model.MastersCard, true, true);//鏡の向きの決め方
        GameManager.instance.Working = false;
        DestroyCard();
    }
}
