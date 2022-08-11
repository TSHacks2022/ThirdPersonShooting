using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    public class PickedMessageManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI pickedMessageText;
        float time = 0;

        void Update()
        {

            if(ItemPickup.timeFlag){
                time = 0;
                ItemPickup.timeFlag = false;
            }

            if(ItemPickup.pickedMessageFlag){            
                pickedMessageText.text = "Received\n" + ItemPickup.pickedMessage + "\n";
                time += Time.deltaTime;
                if(time > 3.0f)
                {
                    time = 0;
                    ItemPickup.pickedMessageFlag=false;
                }
            }else{
                pickedMessageText.text = "";  
            }
        }
    }
}