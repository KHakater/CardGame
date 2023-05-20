using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    SaveData _data;
    public string[] field1 = new string[60], field2 = new string[2], field3 = new string[2], field4 = new string[2];
    public List<string[]> FIELD1;

    // Use this for initialization
    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start()
    {
        FIELD1 = new List<string[]>() { field1, field2, field3, field4 };
        //field1をデッキ保存用として利用
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameDataSending()
    {
        //データの初期化
        _data = new SaveData();
        _data.main();

        //ここからセーブしたいデータを格納する.
        int p = 0;
        foreach (string[] g in FIELD1)//ストリング配列のリストから　一つの配列を選択
        {
            int e = 0;
            foreach (string h in g)//ストリング配列の一個一個
            {
                _data.FIELD1[p][e] = h;//SaveDataのスクリプトにデータを移動
                e += 1;
            }
            p += 1;
        }
        p = 0;

        SaveGame();

    }

    public void GameDateReceiving()
    {
        _data = new SaveData();
        _data = LoadFromJson(SaveData.FilePath);
        _data.main();

        FIELD1 = _data.FIELD1;
        if (field1 != null)
        {
            foreach (string s in field1)
            {
                Debug.Log(s);
            }
        }
    }

    /// <summary>
    /// ファイル書き込み
    /// </summary>
    /// <param name="filePath">ファイルのある場所</param>
    public void SaveToJson(string filePath, SaveData data)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(JsonUtility.ToJson(data));
                sw.Flush();
                sw.Close();
            }
            fs.Close();
        }
    }

    /// <summary>
    /// ファイル読み込みする
    /// </summary>
    /// <param name="filePath">ファイルのある場所</param>
    /// <returns></returns>
    public SaveData LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {//ファイルがない場合FALSE.
            Debug.Log("FileEmpty!");
            return new SaveData();//ファイルが無いときはnewする.
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                SaveData sd = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
                if (sd == null) return new SaveData();
                return sd;
            }
        }
    }

    public void SaveGame()
    {
        SaveToJson(SaveData.FilePath, _data);
    }

    public void DeleteSave()
    {
        File.Delete(SaveData.FilePath);
    }
}
