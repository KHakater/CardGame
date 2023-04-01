using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumCtrl : MonoBehaviour
{
    [SerializeField] private Sprite[] sp = new Sprite[10];

    public void ChangeSprite(int no)
    {
        if (no > 9 || no < 0) no = 0;
        Image image = gameObject.GetComponent<Image>();
        image.sprite = sp[no];
    }
}
