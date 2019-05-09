using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LeaderboardAPI
{
    // Create user account
    public string CreateUser(string name, string username, string email, string password)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/user");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = "{\"name\": \""+ name + "\", \"username\": \"" + username + "\"," +
                " \"email\": \"" + email + "\", \"password\": \"" + password + "\"}";

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return jsonResponse;
    }

    // Sign user in
    public JObject LoginUser(string email, string password)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/user/login");
        try
        {
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string jsonRequest = "{\"email\": \"" + email + "\", \"password\": \"" + password + "\"}";

                streamWriter.Write(jsonRequest);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            JObject json = JObject.Parse(jsonResponse);
            PlayerPrefs.SetString("AuthToken", json["token"].ToString());
            return json;
        }
        catch (WebException wex)
        {
            if (wex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)wex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log(error);
                        JObject json = JObject.Parse(error);
                        Debug.Log(json);
                        return json;
                    }
                }
            }
        }
        return new JObject {"error", "Unable to login" };

    }

    // Get user info
    public JObject GetUserProfile()
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/user/me");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers["Authorization"] = ("Bearer " + PlayerPrefs.GetString("AuthToken"));


        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        JObject json = JObject.Parse(jsonResponse);
        return json;
    }

    public JArray GetUserScores()
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/scores/me");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers["Authorization"] = ("Bearer " + PlayerPrefs.GetString("AuthToken"));


        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        JArray json = JArray.Parse(jsonResponse);
        return json;
    }

    // Get global leadeboard (ability to sort)
    public JArray GetScores(string gameID, string sortBy = null, string sortOrder = null, bool friends = false)
    {
        string query = "?gameID=" + gameID;
        if (sortBy != null && sortOrder != null)
        {
            query += "&sortBy=" + sortBy + ":" + sortOrder;
        }
        if (friends)
        {
            query += "&friends=true";
        }
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/scores/game" + query);
        try
        {
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers["Authorization"] = ("Bearer " + PlayerPrefs.GetString("AuthToken"));
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            JArray json = JArray.Parse(jsonResponse);
            return json;
        }
        catch (WebException wex)
        {
            if (wex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)wex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log(error);
                        JArray json = new JArray();
                            json.Add(JToken.Parse(error));
                        Debug.Log(json);
                        return json;
                    }
                }
            }
        }
        return new JArray { "error", "Unable to find game" };
    }

    // Sign user out
    public JObject LogOut()
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/user/logout");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers["Authorization"] = ("Bearer " + PlayerPrefs.GetString("AuthToken"));


        HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        JObject json = JObject.Parse(jsonResponse);
        PlayerPrefs.SetString("AuthToken", "");
        return json;
    }

    // Upload User Score

    // Get User Score

    // Get friends leaderboard (ability to sort)

    // Get friends

    // Add friend
}
