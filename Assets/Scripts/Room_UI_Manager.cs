using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Room_UI_Manager : MonoBehaviour
{
    //Variables para acceder a la UI
    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private TextMeshProUGUI createText;

    [SerializeField]
    private TextMeshProUGUI joinText;


    //Registro los listeners
    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    //Llamamos a la funcion del manager para create salas
    private void CreateRoom()
    {
        Photon_Manager._PHOTON_MANAGER.CreateRoom(createText.text.ToString());
    }

    //LLamamos a la funcion del manager para unirme a salas
    private void JoinRoom()
    {
        Photon_Manager._PHOTON_MANAGER.JoinRoom(joinText.text.ToString());
    }
}
