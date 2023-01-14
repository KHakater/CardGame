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
}
