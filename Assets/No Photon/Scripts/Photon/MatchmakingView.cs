using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MatchmakingView : MonoBehaviourPunCallbacks
{
    public ExitGames.Client.Photon.Hashtable gameRuleProp = new ExitGames.Client.Photon.Hashtable();

    public ExitGames.Client.Photon.Hashtable MasdeckHashtable = new ExitGames.Client.Photon.Hashtable();
    public ExitGames.Client.Photon.Hashtable NoMasdeckHashtable = new ExitGames.Client.Photon.Hashtable();
    [SerializeField]
    private RoomListView roomListView = default;
    [SerializeField]
    private TMP_InputField roomNameInputField = default;
    [SerializeField]
    private Button createRoomButton = default;

    public CanvasGroup canvasGroup;
    public GameObject UIcanvasGroup;
    public int FS1;
    public List<int> MMVMasDeck = new List<int>() { };
    public List<int> MMVNoDeck = new List<int>() { };

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        canvasGroup = UIcanvasGroup.GetComponent<CanvasGroup>();
        // ロビーに参加するまでは、入力できないようにする
        canvasGroup.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        // ルームリスト表示を初期化する
        roomListView.Init(this);

        roomNameInputField.onValueChanged.AddListener(OnRoomNameInputFieldValueChanged);
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        // ロビーに参加したら、入力できるようにする
        canvasGroup.interactable = true;
    }

    private void OnRoomNameInputFieldValueChanged(string value)
    {
        // ルーム名が1文字以上入力されている時のみ、ルーム作成ボタンを押せるようにする
        createRoomButton.interactable = (value.Length > 0);
    }

    private void OnCreateRoomButtonClick()
    {
        // ルーム作成処理中は、入力できないようにする
        canvasGroup.interactable = false;

        // 入力フィールドに入力したルーム名のルームを作成する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // ルームの作成が失敗したら、再び入力できるようにする
        roomNameInputField.text = string.Empty;
        canvasGroup.interactable = true;
    }

    public void OnJoiningRoom()
    {
        // ルーム参加処理中は、入力できないようにする
        canvasGroup.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        // ルームへの参加が成功したら、UIを非表示にする
        UIcanvasGroup.SetActive(false);
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount == 2)
        {
            List<int> HL = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2 };
            int i = 0;
            for (i = 0; i < HL.Count; i++)
            {
                NoMasdeckHashtable["Nodeck" + i] = HL[i];
            }
            NoMasdeckHashtable["NoDeckNum"] = i;
            PhotonNetwork.CurrentRoom.SetCustomProperties(NoMasdeckHashtable);
            photonView.RPC(nameof(StartGame), RpcTarget.All);
            //PhotonNetwork.IsMessageQueueRunning = false;
            //SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("待機中");
            List<int> HL = new List<int>{ 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2 };
            int i = 0;
            for (i = 0; i < HL.Count; i++)
            {
                MasdeckHashtable["Pldeck" + i] = HL[i];
            }
            MasdeckHashtable["PlDeckNum"] = i;
            PhotonNetwork.CurrentRoom.SetCustomProperties(MasdeckHashtable);
            // for (i = 0; i < HL.Count; i++)
            // {
            //     MasHandHashtable["Plhand" + i] = PPP[i];
            // }
            // PhotonNetwork.CurrentRoom.SetCustomProperties(MasHandHashtable);
            // photonView.RPC(nameof(PlDeckCustom), RpcTarget.All, i, "PlHand");
            // for (i = 0; i < HL.Count; i++)
            // {
            //     NoMasdeckHashtable["Nodeck" + i] = HL[i];
            // }
            // PhotonNetwork.CurrentRoom.SetCustomProperties(NoMasdeckHashtable);
            // for (i = 0; i < deck2.Count; i++)
            // {
            //     NoMasHandHashtable["Nohand" + i] = PPP[i];
            // }
            // PhotonNetwork.CurrentRoom.SetCustomProperties(NoMasHandHashtable);
            // photonView.RPC(nameof(PlDeckCustom), RpcTarget.All, i, "Nohand");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ルームへの参加が失敗したら、再び入力できるようにする
        canvasGroup.interactable = true;
    }
    [PunRPC]
    public void StartGame()
    {
        if (NoMasdeckHashtable["NoDeckNum"] is int vvalue)
        {
            int ii = (int)NoMasdeckHashtable["NoDeckNum"];
            for (int iii = 0; iii <= ii; iii++)
            {
                if (NoMasdeckHashtable["Nodeck" + iii] is int value)
                {
                    MMVNoDeck.Add((int)NoMasdeckHashtable["Nodeck" + iii]);
                }
            }
        }
        if (MasdeckHashtable["PlDeckNum"] is int vvvalue)
        {
            int ii = (int)MasdeckHashtable["PlDeckNum"];
            for (int iii = 0; iii <= ii; iii++)
            {
                if (MasdeckHashtable["Pldeck" + iii] is int value)
                {
                    MMVMasDeck.Add((int)MasdeckHashtable["Pldeck" + iii]);
                }
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {

            PhotonNetwork.CurrentRoom.IsOpen = false;
            int rnd = Random.Range(1, 3);
            gameRuleProp.Add("FS", rnd);
            PhotonNetwork.CurrentRoom.SetCustomProperties(gameRuleProp);
            //PhotonNetwork.AutomaticallySyncScene = true;
            photonView.RPC(nameof(RoadBattleScene), RpcTarget.All);
        }
    }
    [PunRPC]
    public void RoadBattleScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }
}
