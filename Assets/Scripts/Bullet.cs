using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private Rigidbody2D rb;

    private PhotonView pv;

    //Obtenemos referencia al rigid body y al script de photon view
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    //Movimiento muy basico de solo avanzar
    private void Start()
    {
        rb.velocity = new Vector2(speed, 0f);
    }

    //Detectamos las colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Aplico el daño al jugador (para este ejemplo de codigo lo destruyo directamente)
        collision.gameObject.TryGetComponent(out CharacterControl c);

        if (c)
        {
            c.Damage();
        }

        //Uso RPC para llamar la funcion de todos los clientes conectados en la SALA 
        pv.RPC("NetworkDestroy", RpcTarget.All);
    }

    //La directiva PunRPC indica que la funcion puede ser llamada mediante RPC
    [PunRPC]
    public void NetworkDestroy() {
        
        //Destruye el objeto
        Destroy(this.gameObject);
    }
}
