using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    public class InvPanelManager : MonoBehaviour
    {

        [SerializeField] GameObject inventoryPanel;
        [SerializeField] TextMeshProUGUI inventoryText;

        void Start()
        {
            inventoryPanel.SetActive(false);
        }

        void Update()
        {
            inventoryText.text = ItemManager.instance.GetInventory();

            //　左のシフトキーをキーコードで判断
            if(Input.GetKey(KeyCode.LeftShift)) {
                //Debug.Log("LeftShiftKeyCode");
                inventoryPanel.SetActive(true);
            }else{
                inventoryPanel.SetActive(false);
            }
        }
    }
}