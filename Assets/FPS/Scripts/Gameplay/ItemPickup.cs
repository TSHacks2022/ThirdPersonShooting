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
        public string getItem="";
        private GameObject manage;

        [Header("Parameters")] [Tooltip("Item name")]
        public string ItemName;
        
        protected override void OnPicked(PlayerCharacterController player)
        {
            //text変更 
            getItem=ItemName;
            ItemManager.instance.PickItem(ItemName);
            PlayPickupFeedback();
            Destroy(gameObject);
            
        }
    }
}