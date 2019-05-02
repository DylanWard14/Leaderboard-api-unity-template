using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Menu : MonoBehaviour
{
    public InputField emailField, passwordField;
    private LeaderboardAPI leaderboard = new LeaderboardAPI();

    public GameObject loginPanel, profilePanel;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        JObject login = leaderboard.LoginUser(emailField.text, passwordField.text);
        JToken token = login["error"];
        if (token != null)
        {
            Debug.Log(login["error"]);
        }
        else
        {
            Debug.Log("login successful");
            loginPanel.SetActive(false);
            profilePanel.SetActive(true);
            PopulateProfile();
        }  
    }

    public void PopulateProfile()
    {
        
    }
}
