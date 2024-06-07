using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContent;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleLeaderboard();
        }
    }

    public void ToggleLeaderboard()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeSelf);
    }
}
