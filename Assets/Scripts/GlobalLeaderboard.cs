using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class GlobalLeaderboard : MonoBehaviour
{
    private LeaderboardAPI leaderboardAPI = new LeaderboardAPI();

    public GameObject leaderboard;
    public InputField searchField;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Search()
    {
        string gameTitle = searchField.text;
        JArray scores = leaderboardAPI.GetGameScores(gameTitle);
        Debug.Log(scores);

        for (int i = 0; i < leaderboard.transform.childCount - 1; i++)
        {
            Transform parentComponent = leaderboard.transform.GetChild(i + 1);
            parentComponent.GetComponent<Text>().text = "";
            parentComponent.GetChild(0).GetComponent<Text>().text = "";
        }

        for (int i = 0; i < scores.Count; i++)
        {
            Transform parentComponent = leaderboard.transform.GetChild(i + 1);
            parentComponent.GetComponent<Text>().text = scores[i]["owner"].ToString();
            parentComponent.GetChild(0).GetComponent<Text>().text = scores[i]["score"].ToString();

        }
    }
}
