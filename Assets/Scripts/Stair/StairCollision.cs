using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;

public class StairCollision : MonoBehaviour
{
    public GameObject StairMenu;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var objName = hit.gameObject.name;
        Debug.Log(objName);

        if(objName == "Stairs") 
        {
            /*
            StairMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null);
            */

            SceneManager.LoadScene("Dungeon");
        }
    }
}
