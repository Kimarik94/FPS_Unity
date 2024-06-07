using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class RoomItem : MonoBehaviour
{
    private NetworkManager networkManager;

    public Button connectButton;
    public string roomName;

    private void Start()
    {
        connectButton.GetComponent<RectTransform>().localScale = Vector3.zero;
        networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();
    }

    public void SetConnectButtonActive()
    {
        if (roomName != null)
        {
            foreach (RoomInfo room in networkManager.existsRooms)
            {
                if (room.Name == roomName)
                {
                    if (room.PlayerCount >= room.MaxPlayers)
                    {
                        connectButton.GetComponent<RectTransform>().localScale = Vector3.one;
                        connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Room Full";
                        connectButton.onClick.AddListener(RoomIsFull);
                    }
                    else
                    {
                        connectButton.GetComponent<RectTransform>().localScale = Vector3.one;
                        connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connect";
                        connectButton.onClick.AddListener(ConnectThisRoom);
                    }
                    break;
                }
            }
        }
    }

    private void RoomIsFull()
    {
        Debug.Log("Current room is full. You cannot join. Try later...");
    }

    private void ConnectThisRoom()
    {
        networkManager.JoinSpecificRoom(roomName);
    }

    public void SetConnectButtonNonActive()
    {
        connectButton.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}