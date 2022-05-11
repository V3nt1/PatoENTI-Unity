using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Login_Screen : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private TextMeshProUGUI passwordText;

    private void Awake()
    {
        //Defino el listener para cada vez que se haga click al boton
        loginButton.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        //Llamo a la funcion del network manager para conectarme al servidor pasando nick y contraseña

        Network_Manager._NETWORK_MANAGER.TryLogin(loginText.text.ToString(), passwordText.text.ToString());
    }
}
