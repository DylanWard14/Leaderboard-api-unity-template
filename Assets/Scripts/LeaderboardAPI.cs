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

    public JArray GetGameScores(String gameTitle)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.20.120:3000/scores/game?title=" + gameTitle);
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
}
