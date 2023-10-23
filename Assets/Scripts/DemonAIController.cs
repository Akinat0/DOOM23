using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAIController : AIController
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
            MoveTo(FindObjectOfType<PlayerController>().transform.position);
    }
}
