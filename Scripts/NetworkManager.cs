using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // instance
    public static NetworkManager instance;

    void Awake()
    {
        // If an instance already exists and it's not this one, destroy this
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            // Set the instance
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Attempt to create a new room
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    // Attempt to join an existing room
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // Changes the scene using Photon's system
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    
    
}
