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
        SceneManager.LoadScene("Dungeon");
    }

    public void Escape()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        StairMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}