using UnityEngine;
using System.Collections;
using TMPro;

public class FloorText : MonoBehaviour
{
    public TextMeshProUGUI floorNumText;

    private int nowFloor;

    GameObject floorController;
    FloorController floorScript;

    void Start()
    {
        floorController = GameObject.Find("FloorControl");
        floorScript = floorController.GetComponent<FloorController>();

        nowFloor = floorScript.getFloor();

        Debug.Log(nowFloor + "F");

        floorNumText.text = nowFloor + "F";
    }
}
