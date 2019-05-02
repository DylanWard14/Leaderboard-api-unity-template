using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
public class APiTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateUser("Unity", "Unity", "unity@test.com", "1234");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateUser(string name, string username, string email, string password)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:3000/user");
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
        Debug.Log(jsonResponse);
    }

    void MakeRequest()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:3000/test");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        //string info = JsonUtility.FromJson<MyClass>(jsonResponse);
        Debug.Log(JsonUtility.FromJson<MyClass>(jsonResponse).message);

        //WWW myWww = new WWW("http://127.0.0.1/test");
    }

    public class MyClass {
        public string message;
    }
}
