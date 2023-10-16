using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    void OnCharacterStay(PlayerController controller)
    {
        print($"Lava Player stay: {controller.name}");
    }

    void OnCharacterExit()
    {
        print("Lava Player exit");
    }

    void OnCharacterEnter()
    {
        print("Lava Player enter");
    }
}
