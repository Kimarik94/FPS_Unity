using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RoomCreation : MonoBehaviour
{
    private NetworkManager networkManager;

    private UnityEngine.UI.Button backButton;
    private UnityEngine.UI.Button createSpecificRoomButton;

    private GameObject playerNickname;
    private GameObject roomName;
    private GameObject playersQty;
    private GameObject mapType;

    private void Start()
    {
        networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();

        backButton = GameObject.Find("BackButton").GetComponent<UnityEngine.UI.Button>();
        backButton.onClick.AddListener(Back);

        createSpecificRoomButton = 
            GameObject.Find("CreateRoomButton").GetComponent<UnityEngine.UI.Button>();
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
        string players = playersQty.GetComponent<TMP_Dropdown>().options[playersQty.GetComponent<TMP_Dropdown>().value].text;
        string level = mapType.GetComponent<TMP_Dropdown>().options[mapType.GetComponent<TMP_Dropdown>().value].text;

        if (int.TryParse(players, out int intPlayers))
        {
            Debug.Log($"Creating room with {intPlayers} players");
            networkManager.CreateRoom(roomname, intPlayers, level, nickname);
        }
        else
        {
            Debug.LogError("Invalid number of players");
        }
    }
}
