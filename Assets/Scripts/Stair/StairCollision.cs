using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairCollision : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var objName = hit.gameObject.name;

        if(objName == "Stairs") 
        {
            SceneManager.LoadScene("Dungeon");
        }
    }
}
