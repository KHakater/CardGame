using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaView : MonoBehaviour
{
    
    [SerializeField] public Text nameText, MaxText, nowText;
    [SerializeField] Image iconImage;
    public void Show(ManaModel m) // cardModelのデータ取得と反映
    {
        nameText.text = m.Name;
        MaxText.text = m.maxmana.ToString();
        nowText.text = m.nowmana.ToString();
        //iconImage.sprite = cardModel.icon;
    }
}
