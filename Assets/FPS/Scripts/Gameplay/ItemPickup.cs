using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ItemPickup : Pickup
    {
        [Header("Parameters")] [Tooltip("Item name")]
        public string ItemName;

        protected override void OnPicked(PlayerCharacterController player)
        {
            ItemManager.instance.PickItem(ItemName);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}