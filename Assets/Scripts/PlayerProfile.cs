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
    }
}
