using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PickUpMessageManager : MonoBehaviour
{
    TextMeshProUGUI PickUpMessage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(ItemPickup.getItem!=""){
            PickUpMessage.text="Received "+getItem;
        }
    }
    
}
