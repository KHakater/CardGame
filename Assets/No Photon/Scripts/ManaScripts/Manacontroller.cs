using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manacontroller : MonoBehaviour
{
    public ManaModel model;
    public ManaView view;
    public int ID;
    // Start is called before the first frame update
    private void Awake()
    {
        view = GetComponent<ManaView>();
    }

    public void Init(int cardID, bool playerCard,int maxmana,int useful) // カードを生成した時に呼ばれる関数
    {
        model = new ManaModel(cardID, playerCard,1,useful); // カードデータを生成
        view.Show(model); // 表示
        ID = cardID;
    }
    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }
    private void OnButton()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().OnButton(model.CardID);
    }
}
