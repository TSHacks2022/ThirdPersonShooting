using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollision : MonoBehaviour
{
    public GameObject StairMenu;

    void OnCollisionEnter(Collision col)
    {
        StairMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
