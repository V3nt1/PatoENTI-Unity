using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterControl : MonoBehaviour, IPunObservable
{
    //Parametros para personalizar el movimiento del jugador
    [Header("Stats")]
    [SerializeField]
    Class playerClass;

    private int hp;
    private int damage;
    private int speed;
    private int jumpForce;
    private int cadency;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

    private TextMeshProUGUI playerName;

    bool isGrounded;


    //Obtengo referencia al rigidbody y al photon view
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        //Defino cuantas veces por segundo envio la información y la serializo (tanto la que quiero enviar como la que recibo)
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }

    private void Start()
    {
        isGrounded = true;
        playerName = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        Character stats = ClassesManager.instance.classes[(int)playerClass];

        hp = stats.health;
        damage = stats.damage;
        speed = stats.speed;
        jumpForce = stats.jump;
        cadency = stats.cadency;

        Debug.Log(hp);
        Debug.Log(damage);
        Debug.Log(speed);   
        Debug.Log(jumpForce);
        Debug.Log(cadency);

        CharacterControl[] players = FindObjectsOfType<CharacterControl>();

        foreach(CharacterControl c in players)
        {
            if(c != this)
            {
                c.playerName.text = PhotonNetwork.PlayerListOthers[0].NickName;
            }
        }

        playerName.text = ClassesManager.instance.playerCharacter.playerName;
    }
    private void Update()
    {
        //El prefab de personaje se instanciara dos veces en la partida, el mio y el que uso para replicar el movimiento del jugador IsMine == true cuando soy yo quien lo ha instanciado
        if (pv.IsMine) { CheckInputs(); }
        else { Replicate(); }
    }

    //Muevo el personaje
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed*2, rb.velocity.y);
        isGrounded = Physics.Raycast(transform.GetChild(1).position, Vector3.down, 1f);
        GetComponent<Animator>().SetBool("isGrounded", isGrounded);
    }

    //Esta funcion solo se llamara para mi personaje, de esta forma solo 1 de los dos respondera a mis controles
    private void CheckInputs()
    {
        //Miro si quiero moverme 
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");
        GetComponent<Animator>().SetBool("isWalking", desiredMovementAxis != 0);
        

        //Aplico fuerza si quiero saltar no hemos comprobado si estamos en el suelo, tenemos salto infinito
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce*40));
            GetComponent<Animator>().SetTrigger("Jump");
            //Animacion de salto
        }

        //Compruebo si quiero disparar
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    //Instancio por la red mi bala
    private void Shoot()
    {
        //Aqui no tenemos en cuenta donde mira el personaje ni spawns de bala, siempre se ira hacia la derecha
        PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
    }

    //Replicamos el movimiento del jugador rival accediendo a enemyPosition, variable que actualizamos cuando recibimos datos
    private void Replicate()
    {
        //Podemos obtener un movimiento mas fluido del jugador rival actualizando su posicion con transform.position = Vector3.Lerp();
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * 20);
    }

    //Esta funcion se llamara de acuerdo al SerializationRate que hayamos asignado
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        //Cada SendNext sera leido en un solo IsReading
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if(stream.IsReading)
        {
            enemyPosition = (Vector3)stream.ReceiveNext();
        }    
    }

    //Funcion que se llamara como conductor a la funcion RPC
    public void Damage()
    {
        //Reproduzco el daño (en este caso destruyo directamente) a todos los clientes. Photon ya diferencia automaticamente a que prefab debe ejecutarlo
        pv.RPC("NetworkDamage", RpcTarget.All);
    }

    //Reproduce en todos los clientes el destruir
    [PunRPC]
    public void NetworkDamage()
    {
        GetComponent<Animator>().SetTrigger("Hurt");
        //Destroy(this.gameObject);
    }
}
