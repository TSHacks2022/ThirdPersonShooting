using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay{
    public class LoadPlayerParameter : MonoBehaviour
    {

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            Health m_PlayerHealth = playerCharacterController.GetComponent<Health>();

            playerCharacterController.MaxSpeedOnGround += ParameterUpgrade.maxSpeedOnGround;
            playerCharacterController.MaxSpeedInAir += ParameterUpgrade.maxSpeedInAir;
            m_PlayerHealth.MaxHealth += ParameterUpgrade.maxHealth;
            m_PlayerHealth.CurrentHealth = m_PlayerHealth.MaxHealth;

        }
    }
}