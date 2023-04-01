using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTextEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("Break");
    }

    // Update is called once per frame
    IEnumerator Break()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject);
    }
}
