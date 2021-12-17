﻿using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public static string setPName;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Debug.Log("A NetworkManager already exists. Destroying this one!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constants.targetFPS;
        GameServer.StartServer(10, 3600);
    }

    private void OnApplicationQuit()
    {
        GameServer.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0.2f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public GameObject InstantiateEnemy(float x, float y, float z)
    {
        return Instantiate(enemyPrefab, new Vector3(x, y, z), Quaternion.identity);
    }

    public static bool AttemptNewSession(string _username, string _password)
    {
        string accountJSON = ConnectAndAuth(_username, _password);
        string _sessionPlayerName = JSON.HandleUserJSON(accountJSON);
        if (_sessionPlayerName != null)
        {
            setPName = _sessionPlayerName;
            return true;
        }
        else
            return false;

    }

    private static string ConnectAndAuth(string _username, string _password)
    {
        string url = $"http://127.0.0.1:3000/login/{_username}/{_password}";
        return HTTP.GET(url);
    }

    class JSON
    {
        class AccountData
        {
            public string email;
            public string password;
            public string username;
        }
        public static string HandleUserJSON(string _json)
        {
            AccountData jObj = JsonUtility.FromJson<AccountData>(_json);
            return jObj.username;
        }
    }

    class HTTP
    {
        //HTTP&WEB
        public static string GET(string _uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
