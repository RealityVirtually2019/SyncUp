#define DEBUG1

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    //sets up our singleton
    public static PhotonLobby lobby;

    public GameObject roomSearchButton;
    public GameObject cancelButtom;

    private void Awake()
    {
        //this instance
        lobby = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        //from Photon.pun namespace
        //Connects to Master photon Server.
        PhotonNetwork.ConnectUsingSettings();
    }

    //automatically called once we establish connection to photon server
    public override void OnConnectedToMaster()
    {
#if DEBUG1
        print("Player has connected to the Photon Master server");
#endif
        roomSearchButton.SetActive(true);
        //When master client loads scene, all other clients connected will load scene
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnSearchButtonClicked()
    {
#if DEBUG1
        print("PhotonLobby:OnSearchButtonClicked");
#endif
        roomSearchButton.SetActive(false);
        cancelButtom.SetActive(true);
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Attempt to join random game has failed. No games available");
        CreateRoom();
    }

    void CreateRoom()
    {

        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);

#if DEBUG1
        print("PhotonLobby:CreateRoom - Trying to create Room" + randomRoomName);
#endif
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Tried to create a room but failed, there must be a room of the same name");
        CreateRoom();
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        cancelButtom.SetActive(false);
        roomSearchButton.SetActive(true);
        PhotonNetwork.LeaveRoom();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
