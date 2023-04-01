using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text MyLeaderHPText;
    public Text OpLeaderHPText;
    public Image TurnText;
    public Material MyTurnMat, EnemyTurnMat, WinMat, LoseMat;
    public Image PrefabTurnText;
    public EffekseerEffectAsset LeaderAttack;
    public EffekseerEffectAsset MirrorEff, MirrorEff2, MirrorEff2a;
    public GameObject MirrorImg;
    public bool flag;
    public GameObject CardImg1, CardImg2, CardImg3;
    public Image WinLoseImg;
    public GameObject AttackMoveObj;
    public GameObject number_Test;
    public GameObject MyHP, OpHP;
    // Start is called before the first frame update
    public void SetLeaderHPText(bool b, int i, bool effect)
    {
        if (b)
        {
            if (MyHP != null)
            {
                Destroy(MyHP);
            }
            var v = Instantiate(number_Test, new Vector3(0, 0, 0), Quaternion.identity);
            v.transform.SetParent(this.transform);
            v.transform.localPosition = new Vector3(0, 0, 0);
            v.transform.localScale = new Vector3(1, 1, 1);
            //MyLeaderHPText.text = "HP:" + i.ToString();
            v.GetComponent<Number_test>().Init(i, new Vector3(-507f, -313f, 0f));
            MyHP = v;
            if (effect)
            {
                EffekseerHandle handle = EffekseerSystem.PlayEffect(LeaderAttack, new Vector3(0, -5f, 9.8f));
                // transformの回転を設定する。
                handle.SetRotation(Quaternion.Euler(90, 0, 0));
            }
        }
        else
        {
            if (OpHP != null)
            {
                Destroy(OpHP);
            }
            var v = Instantiate(number_Test, new Vector3(0, 0, 0), Quaternion.identity);
            //MyLeaderHPText.text = "HP:" + i.ToString();
            v.transform.SetParent(this.transform);
            v.transform.localPosition = new Vector3(0, 0, 0);
            v.transform.localScale = new Vector3(1, 1, 1);
            v.GetComponent<Number_test>().Init(i, new Vector3(-507f, 313f, 0f));
            OpHP = v;
            if (effect)
            {
                EffekseerHandle handle = EffekseerSystem.PlayEffect(LeaderAttack, new Vector3(0, 5f, 9.8f));
                // transformの回転を設定する。
                handle.SetRotation(Quaternion.Euler(270, 0, 0));
            }
        }
        // transformの位置でエフェクトを再生する

    }
    public IEnumerator TurnStart(bool turnMyorEn)
    {
        // int a = 90;
        // int b = 8;
        // float c = 0.05f;
        // Cubeプレハブを元に、インスタンスを生成、
        if (turnMyorEn)
        {
            //var v = TurnText.transform;
            //TurnText.material = EnemyTurnMat;
            // for (int i = 0; i < a; i++)
            // {
            //     v.Rotate(0f, 0f, b);
            //     Vector3 vv = v.localScale;
            //     v.localScale = new Vector3(vv.x - c, vv.y - c, vv.z);
            //     Image nn = Instantiate(PrefabTurnText, v.position, v.rotation);
            //     nn.material = EnemyTurnMat;
            //     nn.transform.SetParent(this.gameObject.transform);
            //     nn.transform.localScale = new Vector3(v.localScale.x, v.localScale.y, 1f);
            //     yield return null;
            // }
            TurnText.material = MyTurnMat;
            // for (int i = 0; i < a; i++)
            // {
            //     v.Rotate(0f, 0f, b);
            //     Vector3 vv = v.localScale;
            //     v.localScale = new Vector3(vv.x + c, vv.y + c, vv.z);
            //     Image nn = Instantiate(PrefabTurnText, v.position, v.rotation);
            //     nn.material = MyTurnMat;
            //     nn.transform.SetParent(this.gameObject.transform);
            //     nn.transform.localScale = new Vector3(v.localScale.x, v.localScale.y, 1f);
            //     yield return null;
            // }
        }
        else
        {
            //var v = TurnText.transform;
            //TurnText.material = MyTurnMat;
            // for (int i = 0; i < a; i++)
            // {
            //     v.Rotate(0f, 0f, b);
            //     Vector3 vv = v.localScale;
            //     v.localScale = new Vector3(vv.x - c, vv.y - c, vv.z);
            //     Image nn = Instantiate(PrefabTurnText, v.position, v.rotation);
            //     nn.material = MyTurnMat;
            //     nn.transform.SetParent(this.gameObject.transform);
            //     nn.transform.localScale = new Vector3(v.localScale.x, v.localScale.y, 1f);
            //     yield return null;
            // }
            TurnText.material = EnemyTurnMat;
            // for (int i = 0; i < a; i++)
            // {
            //     v.Rotate(0f, 0f, b);
            //     Vector3 vv = v.localScale;
            //     v.localScale = new Vector3(vv.x + c, vv.y + c, vv.z);
            //     Image nn = Instantiate(PrefabTurnText, v.position, v.rotation);
            //     nn.material = EnemyTurnMat;
            //     nn.transform.SetParent(this.gameObject.transform);
            //     nn.transform.localScale = new Vector3(v.localScale.x, v.localScale.y, 1f);
            //     yield return null;
            // }
        }
        TurnText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        TurnText.gameObject.SetActive(false);
    }

    public IEnumerator ReverseEff()
    {
        MirrorImg.transform.rotation = Quaternion.identity;
        MirrorImg.transform.localPosition = new Vector3(-60f, 540, 0);
        CardImg1.SetActive(true);
        MirrorImg.SetActive(true);
        flag = false;
        StartCoroutine("MirrorOTIRU");
        yield return new WaitUntil(() => flag);
        EffekseerHandle handle = EffekseerSystem.PlayEffect(MirrorEff, new Vector3(-1f, 0, 9.8f));
        // transformの回転を設定する。
        //handle.SetRotation(Quaternion.Euler(90, 0, 0));
        CardImg1.SetActive(false);
        CardImg2.SetActive(true);
        yield return new WaitForSeconds(2f);
        flag = false;
        StartCoroutine("MirrorOTIRU");
        yield return new WaitUntil(() => flag);
        yield return null;
        CardImg1.SetActive(false);
        CardImg2.SetActive(false);
        MirrorImg.SetActive(false);
    }
    public IEnumerator DupEff()
    {
        MirrorImg.transform.rotation = Quaternion.identity;
        MirrorImg.transform.localPosition = new Vector3(-60f, 540, 0);
        CardImg1.SetActive(true);
        MirrorImg.SetActive(true);
        flag = false;
        StartCoroutine("MirrorOTIRU");
        yield return new WaitUntil(() => flag);
        EffekseerHandle handle = EffekseerSystem.PlayEffect(MirrorEff2, new Vector3(-1f, 0, 9.8f));
        // transformの回転を設定する。
        //handle.SetRotation(Quaternion.Euler(0, 0, 90));
        yield return new WaitForSeconds(2f);
        CardImg3.SetActive(true);
        flag = false;
        StartCoroutine("MirrorOTIRU");
        yield return new WaitUntil(() => flag);
        yield return null;
        CardImg1.SetActive(false);
        CardImg3.SetActive(false);
        MirrorImg.SetActive(false);
    }
    public IEnumerator MirrorOTIRU()
    {
        float a = 540;
        float b = 10f;
        float c = -0.6f;
        for (int i = 0; i < a / b; i++)
        {
            var v = MirrorImg.transform.localPosition;
            v.y -= b;
            MirrorImg.transform.localPosition = v;
            MirrorImg.transform.Rotate(0, 0, c);
            yield return null;
        }
        flag = true;
        yield return null;
    }

    public void WinLose(bool b)
    {
        if (b)
        {
            WinLoseImg.material = WinMat;
        }
        else
        {
            WinLoseImg.material = LoseMat;
        }
        WinLoseImg.gameObject.SetActive(true);
    }

    public void AttackMove(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        GameObject am = Instantiate(AttackMoveObj, new Vector3(0, 0, 0), Quaternion.identity);
        am.transform.SetParent(this.transform);
        am.GetComponent<AMScript>().AttackMove(v1, v2, v3);
    }
}
