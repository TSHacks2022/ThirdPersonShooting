using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairMenuController : MonoBehaviour
{
    public GameObject StairMenu;

    public void Start()
    {
        StairMenu.SetActive(false);
    }

    public void NextFloor()
    {
        Debug.Log("NextFloor");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Dungeon");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Escape()
    {
        Debug.Log("Escape");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Time.timeScale = 1f;
        StairMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}