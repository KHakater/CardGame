using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckDroped : MonoBehaviour, IDropHandler
{
    public bool MyPlace;//Trueならばデッキ側、Flaseならリスト側
    public DeckManager DM;
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        //eventData.pointerDrag
        //ドロップされたカードの情報をManagerに送りそこで処理を行う
        var v = eventData.pointerDrag.GetComponent<DeckController>();
        if (v == null)
        {
            Debug.Log("NoDC");
        }
        else
        {
            if (v.Place != MyPlace)
            {
                Debug.Log("Yesplace");
                if (MyPlace)
                {
                    DM.deckadd(v);
                    //増える処理 デッキリストに追加
                }
                else
                {
                    DM.deckremove(v);
                    //減らす処理　デッキリストから削除
                }
            }
            else
            {
                Debug.Log("place");
            }
        }
    }
}
