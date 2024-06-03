using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        LoadingProgress.isLoading = false;
        LoadingProgress.minimizeWindow = true;
    }

    public void CreateRoom(string roomName, int playerNumbers)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = playerNumbers;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Комната успешно создана");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось создать комнату: " + message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Успешно подключились к комнате");

        PhotonNetwork.LoadLevel("Level_Base");

        StartCoroutine(LevelWaitingLoading());
        Debug.Log("Level Loaded");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось подключиться к случайной комнате: " + message);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinSpecificRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    private IEnumerator LevelWaitingLoading()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Delay finished");

        GameObject _player = PhotonNetwork.Instantiate("Player", new Vector3(0f, 25f, 0f), Quaternion.identity);

        Debug.Log("Player object instantiated");
    }
}
