using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{

    public static Photon_Manager _PHOTON_MANAGER;

    //Instanciamos el singleton del photon manager, adicionalmente iniciamos la conexion con el servidor photon
    private void Awake()
    {
        if(_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(_PHOTON_MANAGER);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this);
            PhotonConnect();
        }
    }
    //Funcion propia para conectarme al servidor Photon, AutomaticallySyncScene se asegura que si el master carga una escena todos los usuarios en la misma sala la carguen
    public void PhotonConnect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    //Callback para cuando me he conectado con el servidor
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexion realizada correctamente");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //Callback para cuando hay desconexion
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("He implosionado because: " + cause);
    }

    //Callback para cuando me he unido al lobby
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = ClassesManager.instance.playerCharacter.playerName;
        Debug.Log("Accedido al lobby");
    }

    //Callback para cuando me he unido a una sala
    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala: " + PhotonNetwork.CurrentRoom.Name + "con " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados.");
    }

    //Callback para cuando ha habido un fallo al conectarme a una sala
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No me he podido conectar a la sala dado el error: " + returnCode + " que significa: " + message);
    }

    //Creo una sala, RoomOptions es propio de photon e incluye varias funcionalidades para personalizar la sala, aqui por ejemplo defino que el maximo de jugadores son 2
    public void CreateRoom(string nameRoom) {

        Debug.Log("Hola");
        PhotonNetwork.CreateRoom(nameRoom, new RoomOptions { MaxPlayers = 2});
        Debug.Log("Hola2");
    }

    //Me uno a una sala con el nombre solicitado
    public void JoinRoom(string nameRoom) {

        PhotonNetwork.JoinRoom(nameRoom);
    }

    //Callback para cuando se une un jugador a mi sala. Si esta llena y soy el master cargo el mapa de la partida
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            //Player[] players = PhotonNetwork.PlayerListOthers;
            //Network_Manager._NETWORK_MANAGER.AskForOtherClass(players[0].NickName);

            PhotonNetwork.LoadLevel("GameScene");
            Debug.Log("Entramos al nivel pa jugar");
        }
    }

}
