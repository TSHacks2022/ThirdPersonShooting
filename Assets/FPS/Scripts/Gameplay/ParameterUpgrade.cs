using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    
    public class ParameterUpgrade : MonoBehaviour
    {
        
        public static int maxSpeedOnGround = 0,maxSpeedInAir = 0,maxHealth = 0;

        public static void SpeedUpgrade(){
            maxSpeedOnGround += 1;
            maxSpeedInAir += 1;
        }

        public static void HealthUpgrade(){
            maxHealth += 10;
        }

    }
}