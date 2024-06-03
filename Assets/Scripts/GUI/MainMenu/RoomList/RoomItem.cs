using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button connectButton;
    public string roomName;

    private void Start()
    {
        connectButton.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void SetConnectButtonActive()
    { 
        connectButton.GetComponent<RectTransform>().localScale = Vector3.one;
        if(roomName != null)
        {
            connectButton.onClick.AddListener(ConnectThisRoom);
        }
    }

    private void ConnectThisRoom()
    {
        NetworkManager.Instance.JoinSpecificRoom(roomName);
    }

    public void SetConnectButtonNonActive()
    {
        connectButton.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}

