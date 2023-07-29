using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public void MasterTurn()//Masterのターン
    {
        var v = GameManager.instance;
        if (v.ImMorN)
        {
            v.mana.MasterMaxMana += 1;
            v.mana.MasterNowMana = v.mana.MasterMaxMana;
            v.hand.DrawCard("MH"); // 手札を一枚加える
            for (int i = 0; i < 5; i++)
            {
                if (v.field.FieldList[i].transform.childCount != 0)
                {
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        else
        {
            for (int i = 10; i < 15; i++)
            {
                if (v.field.FieldList[i].transform.childCount != 0)
                {
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        v.UIM.StartCoroutine("TurnStart", v.isMyTurn);
    }
    public void NoMasterTurn()
    {
        var v = GameManager.instance;
        if (!v.ImMorN)
        {
            v.mana.NoMasterMaxMana += 1;
            v.mana.NoMasterNowMana = v.mana.NoMasterMaxMana;
            v.hand.DrawCard("NH"); // 手札を一枚加える
            for (int i = 0; i < 5; i++)
            {
                if (v.field.FieldList[i].transform.childCount != 0)
                {
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        else
        {
            for (int i = 10; i < 15; i++)
            {
                if (v.field.FieldList[i].transform.childCount != 0)
                {
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                    v.field.FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
                }
            }
        }
        v.UIM.StartCoroutine("TurnStart", v.isMyTurn);
    }


}
