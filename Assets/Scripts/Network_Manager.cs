using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool connected = false;

    //Server variables
    const string host = "192.168.0.10";
    const int port = 6543;

    private void Awake()
    {
        //Si ya existe una instancia del manager y es diferente de la instancia creada en este script destruyo por duplicado
        if (_NETWORK_MANAGER != null && _NETWORK_MANAGER != this)
        {
            Destroy(_NETWORK_MANAGER);
        }
        else
        {
            //Defino esta instancia como network manager y la asigno como dont destroy para evitar que se borre al cambiar de escena
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        //Si estoy conectado reviso si existen datos
        if (connected)
        {
            //Si hay datos disponibles para leer
            if (stream.DataAvailable)
            {
                //Leo una linea de datos
                string data = reader.ReadLine();
                
                //Si los datos no son nulos los trabajo
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }

    public void TryLogin(string nick, string password)
    {
        try
        {   
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            
            //Si hay streaming de datos hay conexion
            connected = true;
            
            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            
            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine($"Login/{nick}/{password}");
            
            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ManageData(string data)
    {
        string[] parameters = data.Split('/');
        //Si recibo ping devuelvo 1 como respuesta al servidor
        if (data == "Ping")
        {
            Debug.Log("Recibo ping");
            writer.WriteLine("Ping");
            writer.Flush();
        }
        else if (parameters[0].Equals("LoginRespuesta"))
        {
            Debug.Log(parameters[1]);
            if (parameters[1].Equals("True"))
            {
                //Aceptar el login
            }
            else
            {
                Debug.LogError("Logeate bien, tonto");
            }
        }
        else if (parameters[0].Equals("RegisterRespuesta"))
        {
            //En este caso, si devuelve True es que el usuario ya existe por lo que no se puede registrar.
            if (parameters[1].Equals("True"))
            {
                Debug.LogError("Te has registrado mal, tonto");
            }
            else
            {
                //Aceptar registro
            }
        }
    }
}
