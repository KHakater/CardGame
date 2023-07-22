using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] public List<Transform> FieldList;
    [SerializeField] public List<DropPlace> MirrorFieldList;
    public void MoveCard(int afP, string Pname)//
    {
        GameObject card = GameObject.Find(Pname);
        if (afP < 15)
        {
            afP = 14 - afP;
            var v = card.GetComponent<CardController>().model;
            card.GetComponent<CardController>().Init(v.CardID, v.MastersCard, 0, v.IsFace, true, v.isMImage);
        }
        else if (afP > 19 && afP < 22)
        {
            afP = 21 - (afP - 20);
        }
        if (GameManager.instance.ImMorN)
        {
            card.GetComponent<CardController>().Move(afP);
        }
        else
        {
            card.GetComponent<CardController>().Move(14 - afP);
        }
        card.transform.SetParent(FieldList[afP]);
    }
    public void aaa(GameObject g, int a)
    {
        g.transform.SetParent(FieldList[a]);
    }
}
