using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardView : MonoBehaviourPunCallbacks
{
    [SerializeField] Text nameText, powerText, costText, defenseText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel;
    [SerializeField] GameObject canSelectPanel;
    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        nameText.text = cardModel.name;
        powerText.text = "ATK:" + cardModel.power.ToString();
        defenseText.text = "DEF:" + cardModel.power.ToString();
        //costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
    }
    public void SetCanAttackPanel(bool flag)
    {
        canAttackPanel.SetActive(flag);
    }
    public void SetcanSelectPanel(bool flag)
    {
        canSelectPanel.SetActive(flag);
    }
}

