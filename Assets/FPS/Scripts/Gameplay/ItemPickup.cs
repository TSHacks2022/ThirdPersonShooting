using Unity.FPS.Game;
using UnityEngine;
using Unity.FPS.Gameplay;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
namespace Unity.FPS.Gameplay
{
    
    public class ItemPickup : Pickup 
    {

        [Header("Parameters")] [Tooltip("Item name")]
        public string ItemName;
        public static string pickedMessage = "";
        public static bool pickedMessageFlag = false;
        public static bool timeFlag = false;
        
        protected override void OnPicked(PlayerCharacterController player)
        {
            //text変更 
            ItemManager.instance.PickItem(ItemName);
            //Itemnameに応じてItemstaticを更新
            PlayPickupFeedback();
            Destroy(gameObject);

            pickedMessage = ItemName;
            timeFlag = true;
            pickedMessageFlag = true;
        }
    }
}