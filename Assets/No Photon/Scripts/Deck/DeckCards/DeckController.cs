using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public DeckView view; // カードの見た目の処理
    public DeckModel model; // カードのデータを処理
    public int ID;
    public bool Place;
    // Start is called before the first frame update
    private void Awake()
    {
        view = GetComponent<DeckView>();
    }
    public void Init(int cardID, bool isfase) // カードを生成した時に呼ばれる関数
    {
        model = new DeckModel(cardID, isfase); // カードデータを生成
        view.Show(model); // 表示
        ID = cardID;
    }
}
