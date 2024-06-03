using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public RectTransform findRoomPanel;
    public RectTransform createRoomPanel;

    private Vector3 findRoomPanelScale;

    private Button createRoomBtn;
    private Button findRoomBtn;
    private Button exitRoomBtn;

    private void Start()
    {
        createRoomBtn = GameObject.Find("CreateRoom").GetComponent<Button>();
        createRoomBtn.onClick.AddListener(CreateRoom);

        findRoomBtn = GameObject.Find("FindRoom").GetComponent<Button>();
        findRoomBtn.onClick.AddListener(OpenFindRoomPanel);

        exitRoomBtn = GameObject.Find("ExitGame").GetComponent<Button>();
        exitRoomBtn.onClick.AddListener(ExitApp);

        findRoomPanel.localScale = Vector3.zero;
        findRoomPanelScale = new Vector3(0.7f, 0.7f, 0.7f);

        createRoomPanel.localScale = Vector3.zero;
    }

    private void CreateRoom()
    {
        createRoomPanel.localScale = Vector3.one;
        //NetworkManager.Instance.CreateRoom("Room_Name", 3);
    }

    private void OpenFindRoomPanel()
    {
        findRoomPanel.localScale = findRoomPanelScale;
    }

    private void ExitApp()
    {
        Application.Quit();
    }
}
