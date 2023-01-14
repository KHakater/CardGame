using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera; //対象とするカメラ

    [SerializeField]
    private Vector2 aspectVec; //目的解像度
    //public Canvas SetCanvas;
    public CanvasScaler CS;
    void Start()
    {
        //CS = SetCanvas.GetComponent<CanvasScaler>();   
    }
    void Update()
    {
        var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
        var targetAspect = aspectVec.x / aspectVec.y; //目的のアスペクト比

        var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率

        var viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成
        
        if (magRate < 1)
        {
            viewportRect.width = magRate; //使用する横幅を変更
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
            CS.matchWidthOrHeight = 1;
        }
        else
        {
            viewportRect.height = 1 / magRate; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央余生
            CS.matchWidthOrHeight = 0;
        }

        targetCamera.rect = viewportRect; //カメラのViewportに適用
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(magRate);
            Debug.Log(viewportRect);
        }
    }
}
