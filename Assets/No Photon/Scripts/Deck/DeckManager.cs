using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using hensu;
using UnityEngine.UI;
public class DeckManager : MonoBehaviour
{
    public List<DeckEntity> AllCardList, tempList;
    public GameObject parentObj;
    public GameObject DeckparentObj;
    public DeckController CardObj;
    Dictionary<DeckController, int> DeckDic = new Dictionary<DeckController, int> { };//種類と個数
    enum sortmode
    {
        mana,
        attack,
        defence,
    };
    sortmode SortMode = sortmode.mana;
    bool ASC = true;//昇順はTrue
    Dictionary<int, bool> ColorList = new Dictionary<int, bool> { };
    // Start is called before the first frame update
    void Start()
    {
        ColorList.Add(1, true);
        ColorList.Add(2, true);
        ColorList.Add(9, true);
        LoadCardList();
        SaveManager.Instance.GameDateReceiving();
    }
    public void LoadCardList()
    {
        CardEntity[] entities = Resources.LoadAll<CardEntity>("CardEntityList");
        foreach (CardEntity ce in entities)
        {
            if (ce.cardID > 0)
            {
                AllCardList.Add(CreateEntity(ce, true));
                AllCardList.Add(CreateEntity(ce, false));
            }
        }
        SetCardList();
    }
    DeckEntity CreateEntity(CardEntity ce, bool b)
    {
        var v = new DeckEntity();
        if (b)
        {
            v.cardID = ce.cardID;
            v.color = ce.color;
            v.CT = ce.CT;
            v.Defence = ce.Defence;
            v.manaList = ce.manaList;
            v.name = ce.name;
            v.power = ce.power;
            v.isface = true;
        }
        else
        {
            v.cardID = ce.cardID;
            v.color = ce.Reversecolor;
            v.CT = ce.ReverseCT;
            v.Defence = ce.ReverseDefence;
            v.manaList = ce.ReversemanaList;
            v.name = ce.ReverseName;
            v.power = ce.Reversepower;
            v.isface = false;
        }
        return v;
    }
    public void SetCardList()
    {
        tempList = AllCardList;
        Color();
        Sort();
        Display();
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
        if (tempList != null)
        {
            switch (SortMode)
            {
                case sortmode.mana://.ThenBy(e => e.color).ThenBy(e => e.name).ToList()
                    tempList = tempList.OrderBy(CE => CE.manaList.Count).ToList();
                    break;
                case sortmode.attack:
                    tempList = tempList.OrderBy(CE => CE.power).ToList();
                    break;
                case sortmode.defence:
                    tempList = tempList.OrderBy(CE => CE.Defence).ToList();
                    break;
                default:
                    break;
            }
            if (!ASC)
            {
                tempList.Reverse();
            }
        }
    }
    public void Display()
    {
        //CardIDから元のカード情報を取得　そこからリストを表示
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var value in tempList)//一時的マナリストの中から
        {
            DeckController smana = Instantiate(CardObj, new Vector3(0, 0, 0), Quaternion.identity);
            smana.transform.SetParent(parentObj.transform);//条件に合うマナを作成・表示
            smana.Init(value.cardID, value.isface);
            smana.Place = false;
            smana.transform.localScale = new Vector3(1, 1, 1);
            smana.GetComponent<DeckCardMovement>().content = parentObj.transform;
            LayoutGroupSetting(smana.gameObject, parentObj);
        }
    }
    void LayoutGroupSetting(GameObject s, GameObject t)
    {
        s.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        GridLayoutGroup layoutGroup = t.GetComponent<GridLayoutGroup>();
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.SetLayoutHorizontal();
        s.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }
    public void deckadd(DeckController dc)
    {
        bool b = false;
        DeckController v = null;
        foreach (KeyValuePair<DeckController, int> ce in DeckDic)
        {
            if (ce.Key.ID == dc.ID)
            {
                b = true;
                v = ce.Key;
                break;
            }
        }
        if (b)
        {
            var i = DeckDic[v];
            DeckDic[v] = i + 1;
        }
        else
        {
            //dcからID　IDからEntity　Entityから新しいdcを生成しインスタンスも作る
            DeckController smana = Instantiate(CardObj, new Vector3(0, 0, 0), Quaternion.identity);
            smana.transform.SetParent(DeckparentObj.transform);
            smana.Init(dc.ID, dc.model.IsFace);
            smana.Place = true;
            smana.transform.localScale = new Vector3(1, 1, 1);
            smana.GetComponent<DeckCardMovement>().content = DeckparentObj.transform;
            LayoutGroupSetting(smana.gameObject, DeckparentObj);
            DeckDic.Add(smana, 1);
        }
    }
    public void deckremove(DeckController dc)
    {
        int i = DeckDic[dc];
        if (i > 1)
        {
            DeckDic[dc] = i - 1;
        }
        else
        {
            DeckDic.Remove(dc);
            Destroy(dc.gameObject);
        }
    }

    public void ButtonPush()
    {
        int p = 0;
        if (DeckDic != null)
        {
            foreach (KeyValuePair<DeckController, int> ce in DeckDic)
            {
                for (int i = 0; i < ce.Value; i++)
                {
                    SaveManager.Instance.FIELD1[0][p] = ce.Key.ID.ToString();
                }
                p += 1;
            }
            SaveManager.Instance.GameDataSending();
        }
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