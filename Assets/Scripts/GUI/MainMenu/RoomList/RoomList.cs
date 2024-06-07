using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    private RectTransform findRoomPanel;
    private Button closePanBtn;
    private Button refreshRoomsBtn;

    // Select row
    private GraphicRaycaster raycaster;
    private PointerEventData clickData;
    private List<RaycastResult> clickResults;

    private GameObject clickedElement;
    private GameObject previousClickedElement;

    public Transform roomListContainer;
    public GameObject roomItemPrefab;

    private void Start()
    {
        findRoomPanel = GameObject.Find("FindRoomsPanel").GetComponent<RectTransform>();
        closePanBtn = GameObject.Find("ClosePan").GetComponent<Button>();
        refreshRoomsBtn = GameObject.Find("RefreshRoomList").GetComponent<Button>();

        closePanBtn.onClick.AddListener(ClosePan);
        refreshRoomsBtn.onClick.AddListener(RefreshRoomList);

        raycaster = GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetUIElementsClicked();
        }
    }

    private void GetUIElementsClicked()
    {
        clickData.position = Mouse.current.position.ReadValue();

        clickResults.Clear();

        raycaster.Raycast(clickData, clickResults);

        foreach (var result in clickResults)
        {
            clickedElement = result.gameObject;
            if (clickedElement.name.StartsWith("RoomItem"))
            {
                if (previousClickedElement != null && previousClickedElement != clickedElement)
                {
                    previousClickedElement.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                    previousClickedElement.GetComponent<RoomItem>().SetConnectButtonNonActive();
                }

                previousClickedElement = clickedElement;

                clickedElement.GetComponent<Image>().color = new Color(1f, 0.092f, 0.0425f, 0.4f);
                clickedElement.GetComponent<RoomItem>().SetConnectButtonActive();
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Clear previous room items
        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        // Update room list
        foreach (var room in roomList)
        {
            if (!room.RemovedFromList)
            {
                GameObject roomItem = Instantiate(roomItemPrefab, roomListContainer);
                roomItem.GetComponent<RoomItem>().roomName = room.Name;
                roomItem.GetComponentInChildren<TMP_Text>().text = room.Name;
            }
        }
    }

    private void ClosePan()
    {
        findRoomPanel.localScale = Vector3.zero;
    }

    private void RefreshRoomList()
    {
        if (PhotonNetwork.InLobby)
        {
            // This will trigger the OnRoomListUpdate callback
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
