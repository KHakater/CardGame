using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectList : MonoBehaviour
{
    public GameManager GM;
    int count = 1;
    public void Draw(List<int> entities)//回数・自分0相手1
    {
        if (entities[1] == 0)
        {
            if (GM.ImMorN)
            {
                for (int p = 0; p < entities[0]; p++)
                {
                    GM.DrawCard("MH");
                }
            }
            else
            {
                for (int p = 0; p < entities[0]; p++)
                {
                    GM.DrawCard("NH");
                }
            }
        }
        else if (entities[1] == 1)
        {
            if (GM.ImMorN)
            {
                for (int p = 0; p < entities[0]; p++)
                {
                    GM.DrawCard("NH");
                }
            }
            else
            {
                for (int p = 0; p < entities[0]; p++)
                {
                    GM.DrawCard("MH");
                }
            }
        }
    }
    public void AtkUp(List<int> nulllist)
    {
        Debug.Log("a");
        if (GM.FieldList[3].GetChild(0) != null)
        {
            Debug.Log("b");
            var v = GM.FieldList[3].GetChild(0).GetComponent<CardController>();
            v.StatusChange(v.model.power + 1 + count, v.model.Defence + 1 + count);
            count += 1;
        }
    }
    public void N(List<int> nulllist)
    {
        for (int i = 0; i < 5; i++)
        {
            if (GM.FieldList[i].transform.childCount != 0)
            {
                GM.FieldList[i].GetChild(0).GetComponent<CardController>().model.canAttack = true;
                GM.FieldList[i].GetChild(0).GetComponent<CardController>().view.SetCanAttackPanel(true);
            }
        }
    }
}
