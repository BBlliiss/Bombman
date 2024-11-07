using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void StartFallFX()
    {
        player.StartFallFX();
    }
}
