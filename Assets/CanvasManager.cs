using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject logInScreen;
    [SerializeField] GameObject registerScreen;


    public void LoadRegisterScreen()
    {
        logInScreen.SetActive(false);
        registerScreen.SetActive(true);
    }

    public void LoadLoginScreen()
    {
        logInScreen.SetActive(true);
        registerScreen.SetActive(false);
    }
}
