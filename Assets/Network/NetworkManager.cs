using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private readonly GameServer server = new GameServer();
    private Dictionary<uint, NetworkComponent> networkComponents = new Dictionary<uint, NetworkComponent>();

    public static NetworkManager instance;

    public List<GameObject> prefabs = new List<GameObject>();

    public string playerName;


    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject npcPrefab;

    public GameServer Server => server;

    public class HTTP
    {
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

        //Retrieve all data starting from specified index. 0 for all.
        Spellbook.instance.RetrieveSpellDataFromServer(0);
        ItemDatabase.instance.RetrieveItemDataFromServer(0);
        QuestAtlas.instance.RetrieveQuestDataFromServer(0);
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

    public static bool AttemptNewSession(string username, string password)
    {
        string accountJSON = ConnectAndAuth(username, password);
        string sessionPlayerName = AccountData.HandleUserJSON(accountJSON);

        //TODO: the entire server can access the instance of NetworkManager. This type of individual data shouldn't be stored like this.
        if (sessionPlayerName != null)
        {
            instance.playerName = sessionPlayerName;
            return true;
        }
        else
            return false;

    }

    private static string ConnectAndAuth(string username, string password)
    {
        string url = $"http://127.0.0.1:3000/login/{username}/{password}";
        return HTTP.GET(url);
    }
    
    

    //TODO: define and implement InstantiateEntity(x, y, z)
    //waiting on this because i'm not sure how to make a list of prefabs while maintaining readability
    //ie, InstantiateEntity(prefabs[0], Vector3, Quaternion) is much more vague than InstantiateEntity(prefabName, ...)

    //also probably a TODO, but this feels out of place or like it doesn't belong in this class.

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
}
