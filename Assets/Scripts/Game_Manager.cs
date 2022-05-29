using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game_Manager : MonoBehaviour
{
    //Uso dos empty objects como spawn points
    [SerializeField]
    private GameObject spawnPlayer1;

    [SerializeField]
    private GameObject spawnPlayer2;

    private string classToInstantiate;
    private void Awake()
    {
        switch (ClassesManager.instance.playerCharacter.m_Class)
        {
            case Class.Mage:
                classToInstantiate = "Player-Mage";
                break;
            case Class.Shooter:
                classToInstantiate = "Player-Shooter";
                break;
            case Class.Archer:
                classToInstantiate = "Player-Archer";
                break;
            default:
                break;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(classToInstantiate, spawnPlayer1.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(classToInstantiate, spawnPlayer2.transform.position, Quaternion.identity);
        }
    }
}
