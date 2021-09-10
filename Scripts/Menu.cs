using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button joinRoomButton;

    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText;
    public Button startGameButton;

    // Start is called before the first frame update
    void Start()
    {
        // disable the buttons at the start as we're not connected to the server yet
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }

    // Called when we connect to the master server
    // Enables the "Create Room" and "Join Room" buttons
    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    void SetScreen(GameObject screen)
    {
        // deactivate all screens
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        // enable the requested screen
        screen.SetActive(true);
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {
        Debug.Log("Room name is " + roomNameInput.text);
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("In OnJoinedRoom()");
        SetScreen(lobbyScreen);

        // since there's now a player in the lobby, tell everyone to update the lobby ui
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateLobbyUI()
    {
        playerListText.text = "";

        // Display all the players currently in the lobby
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }

        // Only the host can start the game
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        } else
        {
            startGameButton.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

}
