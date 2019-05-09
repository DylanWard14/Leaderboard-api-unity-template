using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class GlobalLeaderboard : MonoBehaviour
{
    private LeaderboardAPI leaderboardAPI = new LeaderboardAPI();

    public GameObject leaderboard;
    public Text leaderboardTypeText;

    public string gameID;

    private string sortBy = "score";
    private string sortOrder = "desc";
    private bool friendsOnly = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        LoadLeaderboard();
    }

    void LoadLeaderboard()
    {
        JArray scores = leaderboardAPI.GetScores(gameID, sortBy, sortOrder, friendsOnly);
        Debug.Log(scores);
        UpdateLeaderboard(scores);
    }

    //Updates the leaderboard with the newly found scores
    void UpdateLeaderboard(JArray scores)
    {
        for (int i = 0; i < leaderboard.transform.childCount - 1; i++)
        {
            Transform parentComponent = leaderboard.transform.GetChild(i + 1);
            parentComponent.GetChild(0).GetComponent<Text>().text = "";
            parentComponent.GetChild(1).GetComponent<Text>().text = "";
        }

        for (int i = 0; i < scores.Count; i++)
        {
            Transform parentComponent = leaderboard.transform.GetChild(i + 1);
            parentComponent.GetChild(0).GetComponent<Text>().text = scores[i]["owner"].ToString();
            parentComponent.GetChild(1).GetComponent<Text>().text = scores[i]["score"].ToString();

        }
    }

    public void UpdateSortOrder()
    {
        if (sortOrder == "asc")
        {
            sortOrder = "desc";
        }
        else
        {
            sortOrder = "asc";
        }
        LoadLeaderboard();
    }

    public void FilterFriends()
    {
        if (friendsOnly)
        {
            friendsOnly = false;
            leaderboardTypeText.text = "Global";
        }
        else
        {
            friendsOnly = true;
            leaderboardTypeText.text = "Friends Only";
        }

        LoadLeaderboard();
    }
}
