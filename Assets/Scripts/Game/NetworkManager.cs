using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string map;
    private string playerNickName;
    public List<RoomInfo> existsRooms = new List<RoomInfo>();
    private string pendingRoomName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LoadingProgress.isLoading = false;
        LoadingProgress.minimizeWindow = true;
        if (!string.IsNullOrEmpty(pendingRoomName))
        {
            JoinSpecificRoom(pendingRoomName);
            pendingRoomName = null;
        }
    }

    public void CreateRoom(string roomName, int playerNumbers, string mapName, string nickName)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)playerNumbers,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "map", mapName } },
            CustomRoomPropertiesForLobby = new string[] { "map" }
        };

        Debug.Log($"Creating room: {roomName} with max players: {playerNumbers}");

        PhotonNetwork.CreateRoom(roomName, roomOptions);

        map = mapName;
        playerNickName = nickName;
    }

    public void JoinSpecificRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            Debug.Log("Not ready to join room, pending connection...");
            pendingRoomName = roomName;
        }
    }

    public override void OnJoinedRoom()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        if (currentRoom != null)
        {
            map = currentRoom.CustomProperties["map"] as string;
            if (currentRoom.PlayerCount <= currentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel(map);
                StartCoroutine(DelayAfterLevelLoading());
            }
            else
            {
                Debug.LogWarning("Комната заполнена");
                PhotonNetwork.LeaveRoom();
                ShowRoomFullMessage();
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось создать комнату: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось подключиться к комнате: " + message);
        if (message.Contains("full"))
        {
            ShowRoomFullMessage();
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        existsRooms.Clear();
        foreach (RoomInfo roomInfo in roomList)
        {
            existsRooms.Add(roomInfo);
            if (roomInfo.CustomProperties.TryGetValue("map", out object mapName))
            {
                Debug.Log($"Room: {roomInfo.Name}, Map: {mapName}");
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.IsMasterClient)
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
                foreach (var player in roomPlayers.Values)
                {
                    if (player.NickName == otherPlayer.NickName)
                    {
                        continue;
                    }
                    else
                    {
                        PhotonNetwork.SetMasterClient(player);
                        break;
                    }
                }
            }
    }

    private void ShowRoomFullMessage()
    {
        Debug.Log("Комната заполнена. Пожалуйста, выберите другую комнату.");
    }


    private IEnumerator DelayAfterLevelLoading()
    {
        yield return new WaitForSeconds(3);
        Transform spawnPoint = RespawnManager.instance.spawnPoints[Random.Range(0, RespawnManager.instance.spawnPoints.Length - 1)];
        PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
    }
}