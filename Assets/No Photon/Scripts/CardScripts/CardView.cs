using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardView : MonoBehaviourPunCallbacks
{
    [SerializeField] Text nameText, powerText, defenseText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel;
    [SerializeField] GameObject canSelectPanel;
    [SerializeField] GameObject ActiveButton;
    [SerializeField] Transform ManaUIList;
    [SerializeField] GameObject MaruPanel;
    [SerializeField] GameObject SikakuPanel;
    [SerializeField] GameObject ura;
    public GameObject numObj;
    GameObject tempatk, tempdef;
    public void Show(CardModel cardModel, bool isSelectCard) // cardModelのデータ取得と反映
    {
        if (cardModel.CTM == "Leader")
        {

        }
        else
        {
            if (cardModel.CanSee)
            {
                //nameText.text = cardModel.name;
                //powerText.text = cardModel.power.ToString();
                //defenseText.text = cardModel.Defence.ToString();
                if (cardModel.CTM == "Monster")
                {
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
                        n1.Init(cardModel.power, new Vector3(-55f, -103f, 0f));
                        n2.size_w = 30; n2.img_size = 0.5f;
                        n2.Init(cardModel.Defence, new Vector3(55f, -103f, 0f));
                    }
                    else
                    {
                        n1.size_w = 20; n1.img_size = 0.33f;
                        n1.Init(cardModel.power, new Vector3(-32f, -50.5f, 0f));
                        n2.size_w = 20; n2.img_size = 0.33f;
                        n2.Init(cardModel.Defence, new Vector3(15f, -50.5f, 0f));
                    }
                }
                iconImage.sprite = cardModel.icon;
                if (ura != null)
                {
                    ura.SetActive(false);
                }
                ManaShow(cardModel);
            }
            else
            {
                ura.SetActive(true);
            }
        }
    }
    void ManaShow(CardModel CM)
    {
        var v = CM.Mlist;
        int Back = -999;
        int count = 1;
        GameObject panel = null;
        foreach (Transform child in ManaUIList.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < v.Count; i++)
        {
            if (v[i] == Back)//直前に追加したマナならば数字を一つ増やす
            {
                count += 1;
                panel.transform.GetChild(0).GetComponent<Text>().text = count.ToString();
            }
            else
            {
                if (panel != null)
                {
                    panel.transform.GetChild(0).GetComponent<Text>().text = count.ToString();
                }
                if (v[i] / 100 == 1)
                {
                    panel = Instantiate(SikakuPanel, new Vector3(0, 0, 0), Quaternion.identity);//新しい種類を追加
                }
                else
                {
                    panel = Instantiate(MaruPanel, new Vector3(0, 0, 0), Quaternion.identity);//新しい種類を追加
                }
                if (v[i] % 10 == 9)
                {
                    panel.GetComponent<Image>().color = Color.white;
                }
                if (v[i] % 10 == 2)
                {
                    panel.GetComponent<Image>().color = Color.blue;
                }
                panel.transform.SetParent(ManaUIList);
                panel.transform.localScale = new Vector3(1, 1, 1);
                panel.transform.position = new Vector3(0, 0, 0);
                panel.transform.GetChild(0).GetComponent<Text>().text = "1";
                count = 1;
                Back = v[i];
            }
        }
    }
    public void SetCanAttackPanel(bool flag)
    {
        canAttackPanel.SetActive(flag);
    }
    public void SetcanSelectPanel(bool flag)
    {
        canSelectPanel.SetActive(flag);
    }
    public void ifOnMouse(bool b)
    {
        ActiveButton.SetActive(b);
    }
}

