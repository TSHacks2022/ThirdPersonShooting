using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public static class WeaponUpgrade
    {
        public static float DelayBetweenShots = 0.5f;
        public static float AmmoReloadRate = 16f;
        public static float AmmoReloadDelay = 2f;
        public static int MaxAmmo = 16;

        public static void Upgrade()
        {
            DelayBetweenShots *= 0.95f;
            AmmoReloadDelay *= 0.95f;
            MaxAmmo += 1;
            AmmoReloadRate = MaxAmmo;
        }
    }
}

