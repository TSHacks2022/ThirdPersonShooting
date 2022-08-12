using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    
    public class ParameterManager : MonoBehaviour
    {

        Health m_PlayerHealth;

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();

            playerCharacterController.MaxSpeedOnGround += 10;
            playerCharacterController.MaxSpeedInAir += 10;

        }

    }
}