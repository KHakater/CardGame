using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaView : MonoBehaviour
{

    [SerializeField] public Text nameText, MaxText, nowText;
    [SerializeField] Image iconImage;
    public GameObject numObj;
    GameObject tempatk, tempdef;
    public void Show(ManaModel m, bool isSelectCard) // cardModelのデータ取得と反映
    {
        nameText.text = m.Name;
        MaxText.text = m.maxmana.ToString();
        nowText.text = m.nowmana.ToString();
        iconImage.sprite = m.icon;
        if (tempatk != null)
        {
            Destroy(tempatk);
            Destroy(tempdef);
        }
        var v = Instantiate(numObj, new Vector3(0, 0, 0), Quaternion.identity);
        v.transform.SetParent(this.transform);
        v.transform.localPosition = new Vector3(0, 0, 0);
        v.transform.localScale = new Vector3(1, 1, 1);
        var n1 = v.GetComponent<Number_test>();

        tempatk = v;

        var vv = Instantiate(numObj, new Vector3(0, 0, 0), Quaternion.identity);
        vv.transform.SetParent(this.transform);
        vv.transform.localPosition = new Vector3(0, 0, 0);
        vv.transform.localScale = new Vector3(1, 1, 1);
        var n2 = vv.GetComponent<Number_test>();
        n2.size_w = 20; n2.img_size = 0.33f;
        tempdef = vv;
        if (isSelectCard)
        {
            n1.size_w = 30; n1.img_size = 0.5f;
            n1.Init(m.nowmana, new Vector3(-55f, -103f, 0f));
            n2.size_w = 30; n2.img_size = 0.5f;
            n2.Init(m.maxmana, new Vector3(55f, -103f, 0f));
        }
        else
        {
            n1.size_w = 15; n1.img_size = 0.25f;
            n1.Init(m.nowmana, new Vector3(-18f, -46.5f, 0f));
            n2.size_w = 15; n2.img_size = 0.25f;
            n2.Init(m.maxmana, new Vector3(16f, -46.5f, 0f));
        }
    }
}
