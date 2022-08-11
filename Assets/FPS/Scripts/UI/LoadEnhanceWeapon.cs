using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadEnhanceWeapon : MonoBehaviour
{
    public void OnclickEnhanceWeapon(){
        SceneManager.LoadScene("EnhanceWeapon");
    }
}
