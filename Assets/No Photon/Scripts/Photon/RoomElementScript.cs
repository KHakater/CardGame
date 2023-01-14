using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RoomElementScript : MonoBehaviour
{//Room情報UI表示用
    public Text PlayerNumber;   //人数
    public Text Opponent;    //部屋作成者名
 
    //入室ボタンroomname格納用
    private string roomname;
 
    //GetRoomListからRoom情報をRoomElementにセットしていくための関数
    public void SetRoomInfo(string _RoomName,string _Opponent)
    {
        //入室ボタン用roomname取得
        roomname = _RoomName;
        Opponent.text = "作成者："+_Opponent;
    }
 
    //入室ボタン処理
    public void OnJoinRoomButton()
    {
        //roomnameの部屋に入室
        PhotonNetwork.JoinRoom(roomname);
    }
}
