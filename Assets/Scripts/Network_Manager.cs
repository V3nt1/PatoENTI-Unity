using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.SceneManagement;

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

    [SerializeField] Register_Screen register;
    [SerializeField] CanvasManager canvasManager;
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

    public void TryRegister(string nick, string password, string selectClass)
    {
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            connected = true;

            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            writer.WriteLine($"Register/{nick}/{password}/{selectClass}");

            writer.Flush();
        }
        catch(Exception e)
        {

            Debug.Log(e.ToString());
        }

    }

    public void AskForOtherClass(string playerName)
    {
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            connected = true;

            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            writer.WriteLine($"OtherClass/{playerName}");

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
                ClassesManager.instance.SetPlayerCharacter(parameters[2], (int)(Class)System.Enum.Parse(typeof(Class), parameters[3]));
                SceneManager.LoadScene("LobbyScene");
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
               //Decir que ya existe una cuenta con ese nombre de user
            }
            else
            {
                canvasManager.LoadLoginScreen();
            }
        }
        else if (parameters[0].Equals("Classes")){

            string allClasses = data.Replace("Classes/", string.Empty);
            List<Character> classes = new List<Character>();

            try
            {
                string[] stringClasses = allClasses.Split('|');

                for (int i = 0; i< stringClasses.Length; i++)
                {
                    if (i % 2 == 0 && i != stringClasses.Length-1)
                    {
                        Character newChar = new Character();

                        string[] classStats = stringClasses[i + 1].Split('/');

                        System.Enum.TryParse(stringClasses[i].ToString(), out newChar.m_Class);
                        
                        int.TryParse(classStats[0], out newChar.health);
                        int.TryParse(classStats[1], out newChar.damage);
                        int.TryParse(classStats[2], out newChar.speed);
                        int.TryParse(classStats[3], out newChar.jump);
                        int.TryParse(classStats[4], out newChar.cadency);

                        classes.Add(newChar);
                    }
                }

                ClassesManager.instance.classes = classes;
                register.LoadAllOptions();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        //else if (parameters[0].Equals("OtherClass"))
        //{
        //    ClassesManager.instance.enemyClass = (Class)System.Enum.Parse(typeof(Class), parameters[1]);
        //}
    }

    public void LoadAllClasses()
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
            writer.WriteLine("GetClasses");

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
