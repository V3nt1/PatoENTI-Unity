using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Class { Mage, Archer, Shooter };

public struct Character
{
    public Character(Class c_Class, int iHealth, int iDamage, int iSpeed, int iJump, int iCadency, string name = "")
    {
        playerName = name;
        m_Class = c_Class;
        health = iHealth;
        damage = iDamage;   
        speed = iSpeed;
        jump = iJump;
        cadency = iCadency;
    }

    public string playerName;
    public Class m_Class;
    public int health;
    public int damage;
    public int speed;
    public int jump;
    public int cadency;
}

public class ClassesManager : MonoBehaviour
{
    public List<Character> classes = new List<Character>();

    public Character playerCharacter;

    public Class enemyClass;

    public static ClassesManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            //Defino esta instancia como network manager y la asigno como dont destroy para evitar que se borre al cambiar de escena
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        Network_Manager._NETWORK_MANAGER.LoadAllClasses();
    }

    public void SetPlayerCharacter(string name, int classType)
    {
        playerCharacter = classes[classType];
        playerCharacter.playerName = name;
    }
    public List<Character> GetAllClasses()
    {
        return classes;
    }
}
