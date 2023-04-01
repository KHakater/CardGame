using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AMScript : MonoBehaviour
{
    bool flag;
    //public GameObject point;
    List<GameObject> List;
    GameObject UIM;
    public void AttackMove(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        StartCoroutine(c(v1, v2, v3));
    }
    public IEnumerator c(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        UIM = GameObject.Find("Canvas");
        flag = false;
        a(v1, v2, v3);
        bb();
        yield return new WaitUntil(() => flag);
        StopCoroutine("b");
        yield return new WaitForSeconds(1f);
        // foreach (GameObject child in List)
        // {
        //     Destroy(child);
        // }
        // Destroy(this.gameObject);
    }
    void a(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3[] path = {
            new Vector3(v1.x- Screen.width / 2, v1.y - Screen.height / 2, v1.z),
            new Vector3(v3.x- Screen.width / 2, v3.y - Screen.height / 2, v3.z),
            new Vector3(v2.x- Screen.width / 2, v2.y - Screen.height / 2, v2.z),
    };
        this.transform.localPosition = new Vector3(v1.x - Screen.width / 2, v1.y - Screen.height / 2, v1.z);
        transform.DOLocalPath(path, 0.5f, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f, Vector3.forward)
                .SetOptions(false, AxisConstraint.Z).OnComplete(pp);
    }
    void pp()
    {
        flag = true;
    }
    void bb()
    {
        StartCoroutine("b");
    }
    IEnumerator b()
    {
        // while (true)
        // {
        //     yield return new WaitForSeconds(0.05f);
        //     GameObject r = Instantiate(point, transform.localPosition, Quaternion.identity);
        //     //Debug.Log(transform.localPosition);
        //     r.transform.SetParent(UIM.transform);
        //     r.transform.localScale = new Vector3(1, 1, 1);
        //     r.transform.localPosition = transform.localPosition;
        //     //List.Add(r);
        // }
        yield return null;
    }
}
