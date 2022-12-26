using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ServerManagement : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        /* make sure assets > photon > photonunitynetwork > resources > photonserversettings.asset: 'start in offline mode' is UNchecked */
        //PhotonNetwork.ConnectUsingSettings(); // connect to server, 
        //PhotonNetwork.LeaveLobby();
        /*
         */
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby(); // connect to lobby
        //It checks/controls the connection of the server
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to the lobby");
        PhotonNetwork.JoinOrCreateRoom("AkdenizCSRoom", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true}, TypedLobby.Default);
        //connect to random room or create 
        //it checks the connection of the Lobby
    }

    public override void OnJoinedRoom()
    {
        GameObject myObject = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, null);
        myObject.GetComponent<PhotonView>().Owner.NickName = "Player 1";
        Debug.Log("Connected to the Room");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the Room");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left the Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any random room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not create room");
    }
}
