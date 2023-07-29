using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            card.GetComponent<CardController>().Init(v.CardID, v.MastersCard, 0, v.IsFace, true, v.isKyouzou);
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
    public void HandSort()
    {
        var t = FieldList[20].GetComponent<GridLayoutGroup>();
        t.CalculateLayoutInputHorizontal();
        t.CalculateLayoutInputVertical();
        t.SetLayoutHorizontal();
        t.SetLayoutVertical();
        var t2 = FieldList[21].GetComponent<GridLayoutGroup>();
        t2.CalculateLayoutInputHorizontal();
        t2.CalculateLayoutInputVertical();
        t2.SetLayoutHorizontal();
        t2.SetLayoutVertical();
    }
}
