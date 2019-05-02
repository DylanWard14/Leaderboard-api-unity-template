using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlayerProfile : MonoBehaviour
{
    private LeaderboardAPI api = new LeaderboardAPI();
    public Text username, email;
    public GameObject leaderboard;
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
        JObject profile = api.GetUserProfile();
        Debug.Log(profile["username"]);
        username.text = profile["username"].ToString();
        email.text = profile["email"].ToString();

        leaderboard.transform.GetChild(1).GetComponent<Text>().text = "test";
        JArray scores = api.GetUserScores();

        int limit = 5;

        if (scores.Count < 5)
        {
            limit = scores.Count; 
        }

        for (int i = 0; i < limit; i++)
        {
            Transform parentComponent = leaderboard.transform.GetChild(i + 1);
            parentComponent.GetComponent<Text>().text = scores[i]["game"].ToString();
            parentComponent.GetChild(0).GetComponent<Text>().text = scores[i]["score"].ToString();

        }
    }
}
