using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public static class ProjectileUpgrade
    {
        public static float Damage = 15f;

        public static void Upgrade()
        {
            Damage *= 1.1f;
        }
    }
}

