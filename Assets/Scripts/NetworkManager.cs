using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private Dictionary<uint, NetworkComponent> networkComponents = new Dictionary<uint, NetworkComponent>();

    public List<GameObject> prefabs = new List<GameObject>(); 
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject npcPrefab;
    public static string setPName;

    private readonly GameServer server = new GameServer();
    public GameServer Server => server;
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
        //These settings enable to server to run PER_TICK
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constants.TARGET_FPS;
        server.StartServer(10, 3600);
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }

    public void AddNetworkComponent(NetworkComponent component)
    {
        if (component == null)
        {
            throw new ArgumentNullException("component");
        }
        
        if (component.NetworkId == 0)
        {
            throw new ArgumentNullException("NetworkComponent has network id 0");
        }
        
        networkComponents.Add(component.NetworkId, component);
    }

    public bool RemoveNetworkComponent(NetworkComponent component)
    {
        if (component == null)
        {
            throw new ArgumentNullException("component");
        }
        
        return networkComponents.Remove(component.NetworkId);
    }

    public NetworkComponent FindNetworkComponent(uint networkId)
    {
        if (networkComponents.TryGetValue(networkId, out NetworkComponent component))
        {
            return component;
        }

        return null;
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0.2f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public GameObject InstantiateEnemy(float x, float y, float z)
    {
        return Instantiate(enemyPrefab, new Vector3(x, y, z), Quaternion.identity);
    }

    public GameObject InstantiateNPC(float x, float y, float z)
    {
        return Instantiate(npcPrefab, new Vector3(x, y, z), Quaternion.identity);
    }

    public static bool AttemptNewSession(string _username, string _password)
    {
        string accountJSON = ConnectAndAuth(_username, _password);
        string _sessionPlayerName = AccountData.HandleUserJSON(accountJSON);
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


    struct AccountData
    {
        public string email;
        public string password;
        public string username;

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
