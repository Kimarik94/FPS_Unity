using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RoomCreation : MonoBehaviour
{
    private UnityEngine.UI.Button backButton;
    private UnityEngine.UI.Button createSpecificRoomButton;

    private GameObject playerNickname;
    private GameObject roomName;
    private GameObject playersQty;
    private GameObject mapType;

    private void Start()
    {
        backButton = GameObject.Find("BackButton").GetComponent<UnityEngine.UI.Button>();
        backButton.onClick.AddListener(Back);

        createSpecificRoomButton = GameObject.Find("CreateRoomButton").GetComponent<UnityEngine.UI.Button>();
        createSpecificRoomButton.onClick.AddListener(TakeInfoFormAndCreateRoom);

        playerNickname = GameObject.Find("InputFieldNickName");
        roomName = GameObject.Find("InputFieldRoomName");
        playersQty = GameObject.Find("DropDownPlayersQty");
        mapType = GameObject.Find("InputDropFieldMap");
    }

    private void Back()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    private void TakeInfoFormAndCreateRoom()
    {
        string nickname = playerNickname.GetComponent<TMP_InputField>().text;
        string roomname = roomName.GetComponent<TMP_InputField>().text;
        string players = playersQty.GetComponent<TMP_Dropdown>().itemText.text;

        RoomOptions roomOptions = new RoomOptions();
        int.TryParse(players, out roomOptions.MaxPlayers);
        
        PhotonNetwork.CreateRoom(roomname, roomOptions);
    }
}
