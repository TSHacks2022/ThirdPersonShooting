using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ProjectileParameterSet : MonoBehaviour
    {
        void Start()
        {
            ProjectileStandard ps = gameObject.GetComponent<ProjectileStandard>();

            ps.Damage = ProjectileUpgrade.Damage;
        }
    }
}

