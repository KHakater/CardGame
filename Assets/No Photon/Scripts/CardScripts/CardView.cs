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
        if (cardModel.IsFace)
        {
            nameText.text = cardModel.name;
            powerText.text = "ATK:" + cardModel.power.ToString();
            defenseText.text = "DEF:" + cardModel.Defence.ToString();
            //costText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;
        }
        else
        {
            nameText.text = cardModel.Reversename;
            powerText.text = "ATK:" + cardModel.Reversepower.ToString();
            defenseText.text = "DEF:" + cardModel.ReverseDefence.ToString();
            //costText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.Reverseicon;
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
}

