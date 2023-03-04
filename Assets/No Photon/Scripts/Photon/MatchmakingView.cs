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
    public ExitGames.Client.Photon.Hashtable rndc = new ExitGames.Client.Photon.Hashtable();

    public ExitGames.Client.Photon.Hashtable deckHashtable = new ExitGames.Client.Photon.Hashtable();
    [SerializeField]
    private RoomListView roomListView = default;
    [SerializeField]
    private TMP_InputField roomNameInputField = default;
    [SerializeField]
    private Button createRoomButton = default;
    public static MatchmakingView instance;
    public CanvasGroup canvasGroup;
    public GameObject UIcanvasGroup;
    public int FS1;
    public List<int> MMVMasDeck = new List<int>() { };
    public List<int> MMVNoDeck = new List<int>() { };
    public int rnd = 0;
    public List<int> MyDeck = new List<int>() { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2 };
    public int CustomCheck1;
    public bool CustomCheck2;
    public int Cus2DeckListCounter;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)//GameManager.instanceで利用できるように！！
        {
            instance = this;
        }
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
            CustomCheck2 = false;
            int i = 0;
            for (i = 0; i < MyDeck.Count; i++)
            {
                deckHashtable["NoDeck" + i] = MyDeck[i];
            }
            deckHashtable["NoDeckNum"] = i;
            PhotonNetwork.CurrentRoom.SetCustomProperties(deckHashtable);
            new WaitUntil(() => PhotonNetwork.CurrentRoom.CustomProperties["PlDeckNum"] is int value);
            new WaitUntil(() => CustomCheck2 == true);
            photonView.RPC(nameof(StartGame), RpcTarget.All);
        }
        else
        {
            Debug.Log("待機中");
            int i = 0;
            for (i = 0; i < MyDeck.Count; i++)
            {
                deckHashtable["PlDeck" + i] = MyDeck[i];
            }
            deckHashtable["PlDeckNum"] = i;
            PhotonNetwork.CurrentRoom.SetCustomProperties(deckHashtable);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ルームへの参加が失敗したら、再び入力できるようにする
        canvasGroup.interactable = true;
    }
    [PunRPC]
    public IEnumerator StartGame()
    {
        Cus2DeckListCounter=0;
        Custom2DeckList("No",MMVNoDeck);
        Custom2DeckList("Pl",MMVMasDeck);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            rnd = Random.Range(1, 3);
            CustomCheck1 = 0;
            rndc.Add("FS", rnd);
            PhotonNetwork.CurrentRoom.SetCustomProperties(rndc);
            yield return new WaitUntil(() => CustomCheck1 == rnd && Cus2DeckListCounter ==2);
            photonView.RPC(nameof(RoadBattleScene), RpcTarget.All);
        }
        else
        {
            yield return null;
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged["FS"] is int value)
        {
            CustomCheck1 = (int)propertiesThatChanged["FS"];
        }
        if (propertiesThatChanged["NoDeckNum"] is int valuee)
        {
            CustomCheck2 = true;
        }
    }
    [PunRPC]
    public void RoadBattleScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }
    private void Custom2DeckList(string a,List<int> l)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[a + "DeckNum"] is int vvalue)
        {
            int ii = (int)vvalue;
            for (int iii = 0; iii <= ii; iii++)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties[a + "Deck" + iii] is int value)
                {
                    l.Add((int)PhotonNetwork.CurrentRoom.CustomProperties[a+ "Deck" + iii]);
                }
            }
            Cus2DeckListCounter+=1;
        }
    }
}
