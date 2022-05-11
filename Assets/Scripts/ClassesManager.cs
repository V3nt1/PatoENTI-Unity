using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassesManager : MonoBehaviour
{
    public enum Classes { Mage, Archer, Shooter};
    Classes classe;

    // Update is called once per frame
    void Update()
    {
        System.Enum.TryParse("Mage", out classe);
    }
}
