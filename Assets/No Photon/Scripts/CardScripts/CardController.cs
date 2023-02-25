using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public int ID;
    private void Awake()
    {
        view = GetComponent<CardView>();
    }
    public void Init(int cardID, bool playerCard ,int cardplace) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard,cardplace); // カードデータを生成
        view.Show(model); // 表示
        ID = cardID;
    }
    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }
    public void StatusChange(int attack,int hp)//攻撃と体力を指定された値に変更し、表示する
    {
        model.power = attack;
        model.Defence = hp;
        view.Show(model);
    }
    public void frame(bool canATK,bool canSelect){
        model.canAttack = canATK;
        model.MSelectable = canSelect;
        view.SetCanAttackPanel(canATK);
        view.SetcanSelectPanel(canSelect);
    }
}
