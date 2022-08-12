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
        public float HealAmount = 40;
        public bool getKeyFlag = true;
        Health m_PlayerHealth;

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
        }

        void Update()
        {
            potionNumText.text = "x" + ItemManager.instance.GetNum("Potion");
            
            //　左のシフトキーをキーコードで判断
            if(Input.GetKey(KeyCode.R) && getKeyFlag && m_PlayerHealth.CanPickup()) {
                if(ItemManager.instance.GetNum("Potion") > 0){
                    ItemManager.instance.UsedItem("Potion");
                    m_PlayerHealth.Heal(HealAmount);
                    Debug.Log("Used Potion");
                }
                getKeyFlag = false;
            }
            if(Input.GetKeyUp(KeyCode.R)){
                getKeyFlag = true;
            }
        }
    }
}