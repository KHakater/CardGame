using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeckView : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Transform ManaUIList;
    [SerializeField] GameObject MaruPanel;
    [SerializeField] GameObject SikakuPanel;
    public GameObject numObj;
    GameObject tempatk, tempdef;
    // public void Show(DeckModel cardModel) // cardModelのデータ取得と反映
    // {
    //     if (cardModel.CTM == "Monster")
    //     {
    //         // if (tempatk != null)
    //         // {
    //         //     Destroy(tempatk);
    //         //     Destroy(tempdef);
    //         // }
    //         // var v = Instantiate(numObj, new Vector3(0, 0, 0), Quaternion.identity);
    //         // v.transform.SetParent(this.transform);
    //         // v.transform.localPosition = new Vector3(0, 0, 0);
    //         // v.transform.localScale = new Vector3(1, 1, 1);
    //         // var n1 = v.GetComponent<Number_test>();

    //         // tempatk = v;

    //         // var vv = Instantiate(numObj, new Vector3(0, 0, 0), Quaternion.identity);
    //         // vv.transform.SetParent(this.transform);
    //         // vv.transform.localPosition = new Vector3(0, 0, 0);
    //         // vv.transform.localScale = new Vector3(1, 1, 1);
    //         // var n2 = vv.GetComponent<Number_test>();
    //         // n2.size_w = 20; n2.img_size = 0.33f;
    //         // tempdef = vv;
    //         // if (isSelectCard)
    //         // {
    //         //     n1.size_w = 30; n1.img_size = 0.5f;
    //         //     n1.Init(cardModel.power, new Vector3(-55f, -103f, 0f));
    //         //     n2.size_w = 30; n2.img_size = 0.5f;
    //         //     n2.Init(cardModel.Defence, new Vector3(55f, -103f, 0f));
    //         // }
    //         // else
    //         // {
    //         //     n1.size_w = 20; n1.img_size = 0.33f;
    //         //     n1.Init(cardModel.power, new Vector3(-32f, -50.5f, 0f));
    //         //     n2.size_w = 20; n2.img_size = 0.33f;
    //         //     n2.Init(cardModel.Defence, new Vector3(15f, -50.5f, 0f));
    //         // }
    //     }
    //     if (cardModel.icon != null)
    //     {
    //         iconImage.sprite = cardModel.icon;
    //     }
    //     ManaShow(cardModel);
    // }
    // void ManaShow(DeckModel CM)
    // {

    // }
}
