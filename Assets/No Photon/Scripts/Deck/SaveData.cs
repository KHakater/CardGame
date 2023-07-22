using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class SaveData : MonoBehaviour
{
    private static string filePath = Application.persistentDataPath + "/savedata.json";//セーブデータのファイルパス
    // public static string FilePath
    // {//ファイルパスのプロパティ
    //     get { return filePath; }
    // }

    // //ここにSaveManagerと同じ構造の変数を用意する.
    // public string[] field1 = new string[60], field2 = new string[2], field3 = new string[2], field4 = new string[2];
    // public List<string[]> FIELD1;


    // public void main()
    // {
    //     //Listなど初期化
    //     FIELD1 = new List<string[]>() { field1, field2, field3, field4 };
    // }
}
