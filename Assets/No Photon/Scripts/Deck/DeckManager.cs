using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using hensu;
public class DeckManager : MonoBehaviour
{
    public List<DeckEntity> AllCardList, tempList;
    public GameObject parentObj;
    enum sortmode
    {
        mana,
        attack,
        defence,
    };
    sortmode SortMode = sortmode.mana;
    bool ASC = true;//昇順はTrue
    Dictionary<int, bool> ColorList = new Dictionary<int, bool> { };
    public GameObject DisplayPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ColorList.Add(1, true);
        ColorList.Add(2, true);
        ColorList.Add(9, true);
        LoadCardList();
    }
    public void LoadCardList()
    {
        CardEntity[] entities = Resources.LoadAll<CardEntity>("CardEntityList");
        foreach (CardEntity ce in entities)
        {
            if (ce.cardID > 0)
            {
                var v = new DeckEntity();
                v.cardID = ce.cardID;
                v.color = ce.color;
                v.CT = ce.CT;
                v.Defence = ce.Defence;
                v.manaList = ce.manaList;
                v.name = ce.name;
                v.power = ce.power;
                v.isface = true;
                var vv = new DeckEntity();
                vv.cardID = ce.cardID;
                vv.color = ce.Reversecolor;
                vv.CT = ce.ReverseCT;
                vv.Defence = ce.ReverseDefence;
                vv.manaList = ce.ReversemanaList;
                vv.name = ce.ReverseName;
                vv.power = ce.Reversepower;
                vv.isface = false;
                AllCardList.Add(v);
                AllCardList.Add(vv);
            }
        }
    }
    public void SetCardList()
    {
        tempList = AllCardList;
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }
        Color();
        Sort();

    }
    public void Color()
    {
        var tl = new List<DeckEntity>();

        foreach (KeyValuePair<int, bool> n in ColorList)//絞り込み　色
        {
            if (n.Value)
            {
                int i = n.Key;
                foreach (DeckEntity CE in tempList)
                {
                    if (CE.color.Contains(i))
                    {
                        if (!tl.Contains(CE))
                        {
                            tl.Add(CE);
                        }
                        continue;
                    }
                }
            }
        }
        tempList = tl;
    }
    public void Sort()
    {
        switch (SortMode)
        {
            case sortmode.mana:
                tempList = tempList.OrderBy(CE => CE.manaList.Count).ThenBy(e => e.color).ThenBy(e => e.name).ToList();
                break;
            case sortmode.attack:
                tempList = tempList.OrderBy(CE => CE.power).ThenBy(e => e.color).ThenBy(e => e.name).ToList();
                break;
            case sortmode.defence:
                tempList = tempList.OrderBy(CE => CE.Defence).ThenBy(e => e.color).ThenBy(e => e.name).ToList();
                break;
            default:
                break;
        }
        if (!ASC)
        {
            tempList.Reverse();
        }
    }
    public void Display()
    {
        //CardIDから元のカード情報を取得　そこからリストを表示
    }
}

public class DeckEntity : ScriptableObject
{
    public int cardID;
    public new string name;
    public List<int> manaList = new List<int>();
    public int power;
    public int Defence;

    //[SerializeField]public List<string> effectlist;
    public Cardtype CT;
    public List<int> color;
    public bool isface;
}