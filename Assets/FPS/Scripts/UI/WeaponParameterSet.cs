using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class WeaponParameterSet : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            WeaponController wc = gameObject.GetComponent<WeaponController>();

            wc.DelayBetweenShots = WeaponUpgrade.DelayBetweenShots;

            wc.AmmoReloadRate = WeaponUpgrade.AmmoReloadRate;

            wc.AmmoReloadDelay = WeaponUpgrade.AmmoReloadDelay;

            wc.MaxAmmo = WeaponUpgrade.MaxAmmo;
        }
    }
}