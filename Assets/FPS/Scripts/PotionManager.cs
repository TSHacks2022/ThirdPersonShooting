using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    public class PotionManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI potionNumText;
        //public float HealAmount;

        void Update()
        {
            //inventoryText.text = ItemManager.instance.GetInventory();
            potionNumText.text = "Potion:" + ItemManager.instance.GetNum("Potion");
            
            //　左のシフトキーをキーコードで判断
            if(Input.GetKey(KeyCode.R)) {
                //Damageable.Health.Heal(40);
                if(ItemManager.instance.GetNum("Potion") > 0){
                    ItemManager.instance.UsedItem("Potion");
                    Debug.Log("Used Potion");
                }
            }
        }
    }
}