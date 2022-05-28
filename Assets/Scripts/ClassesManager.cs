using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Class { Mage, Archer, Shooter };

public struct Character
{
    public Character(Class c_Class, int iHealth, int iDamage, int iSpeed, int iJump, int iCadency)
    {
        m_Class = c_Class;
        health = iHealth;
        damage = iDamage;   
        speed = iSpeed;
        jump = iJump;
        cadency = iCadency;
    }

    public Class m_Class;
    public int health;
    public int damage;
    public int speed;
    public int jump;
    public int cadency;
}

public class ClassesManager : MonoBehaviour
{
    public static List<Character> classes;

    private void Awake()
    {
        classes = new List<Character>();
    }

    private void Start()
    {
        Network_Manager._NETWORK_MANAGER.LoadAllClasses();
    }
    void Update()
    {

    }

    public static List<Character> GetAllClasses()
    {
        return classes;
    }
}
