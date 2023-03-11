using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public int ID;
    public int IndividualNumber = -999;
    private void Awake()
    {
        view = GetComponent<CardView>();
    }
    public void Init(int cardID, bool playerCard, int cardplace, bool isfase) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard, cardplace, isfase); // カードデータを生成
        view.Show(model); // 表示
        ID = cardID;
    }
    public void DestroyCard(CardController card)
    {
        GameManager.instance.ifDestroyed(this.gameObject.GetComponent<CardController>());
        Destroy(card.gameObject);
    }
    public void StatusChange(int attack, int hp)//攻撃と体力を指定された値に変更し、表示する
    {
        model.power = attack;
        model.Defence = hp;
        view.Show(model);
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
            if (!(c.model.CTM == "Monster" && (model.MirrorType & 1 << 0) != 0) &&
            !(c.model.CTM == "Magic" && (model.MirrorType & 2 << 0) != 0) &&
            !(c.model.CTM == "Mirror" && (model.MirrorType & 3 << 0) != 0))
            {//どれか一つでもあてはまったら続行、当てはまらなかったらcontinue
                continue;
            }
            a.SelectableList.Add(c.gameObject);
            c.frame(c.model.canAttack, true);
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
}
