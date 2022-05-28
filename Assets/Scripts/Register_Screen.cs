using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Register_Screen : MonoBehaviour
{
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_Dropdown classSelector;
    [SerializeField] Button sendQueryButton;
    List<string> stringClasses;
    // Start is called before the first frame update
    void Start()
    {
        stringClasses = new List<string>();
        sendQueryButton.onClick.AddListener(SendQuery);  
    }

    public void LoadAllOptions()
    {
        foreach (Character playerClass in ClassesManager.GetAllClasses())
        {
            stringClasses.Add(playerClass.m_Class.ToString());
        }

        classSelector.AddOptions(stringClasses);
    }
    void SendQuery()
    {
        if(nickname.text == string.Empty)
        {
            Debug.Log("Pon algo de nombre");
            return;
        }
        else if(password.text == string.Empty)
        {
            Debug.Log("Pon algo de password");
            return;
        }

        Network_Manager._NETWORK_MANAGER.TryRegister(nickname.text.ToString(), password.text.ToString(), ((Class)classSelector.value).ToString());
    }
}
