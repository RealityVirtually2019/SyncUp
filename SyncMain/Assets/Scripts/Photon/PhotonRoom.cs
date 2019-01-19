#define DEBUG1

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks{

    public static PhotonRoom room;

    //sends client message to another client through RPC calls
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //player info
    //From Photon.Realtime namespace
    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    private void Awake()
    {
        //sets up our singleton
        //should probably set it up with more protection
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        //subscribe!
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Use this for initialization
    void Start () {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
	}
	
	// Update is called once per frame
	void Update () {
		//all delayed start stuff
        //like timer before we start the game
        if(MultiplayerSetting.multiplayerSetting.delayStart)
        {
            if(playersInRoom == 1)
            {
                RestartTimer();
            }
            if(!isGameLoaded)
            {
                if(readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Display time to start to the players " + timeToStart);
                if(timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
	}

    public override void OnJoinedRoom()
    {
#if DEBUG1
        print("PhotonLobby:OnJoinedRoom - player joined a room");
#endif
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            print("Displayer players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                //closes the room so no new players can join
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }


    //calls when another player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if(MultiplayerSetting.multiplayerSetting.delayStart)
        {
            print("Displayer players in room out of max players possible (" + playersInRoom + ":"
                + MultiplayerSetting.multiplayerSetting.maxPlayers + ")");
            if(playersInRoom > 1)
            {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                //if we are master client, close the room!
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

    }

    ///<summary>
    /// Loads the master client into the multiplayer scene
    /// and because we enable automatic sync scene in our lobby script
    /// all the clients will load into the into the multiplayer scene
    ///</summary>
    void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(MultiplayerSetting.multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.multiplayerScene);
        
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 2;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSetting.multiplayerSetting.multiplayerScene)
        {
            isGameLoaded = true;
            
            //if we do a delay, send out the message to all clients to start their game
            if(MultiplayerSetting.multiplayerSetting.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();

            }
        }
    }

    //you need [PunRPC] to run your RPC functions to send client to client/host2client

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);

    }
}
