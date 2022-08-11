using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CloseRuseltPanel : MonoBehaviour
{
    public GameObject OkButton;
    public GameObject ResultPanel;

    public void OnClickOk(){
        ResultPanel.SetActive(false);
    }
}
