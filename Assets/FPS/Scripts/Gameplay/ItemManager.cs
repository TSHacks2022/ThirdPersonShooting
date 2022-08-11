using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {


	public static ItemManager instance;

 
	// Use this for initialization
	void Start () {
		
		instance = this;

		

		//foreach (var item in itemDictionary) {
		//	Debug.Log (item.Key + " : " + GetNum(item.Key));
		//}
	}

    
	public void PickItem(string key) {
		ItemStaticData.itemDictionary [key] += 1;
		Debug.Log (key + " : " + GetNum(key));
	}

	public void UsedItem(string key) {
		ItemStaticData.itemDictionary [key] -= 1;
		Debug.Log (key + " : " + GetNum(key));
	}

	public string GetInventory(){
		string strText = "";
		foreach (var item in ItemStaticData.itemDictionary) {
			strText = strText + item.Key + " : " + GetNum(item.Key) + "\n";
		}		
		return strText;
	}
 
	//　アイテムをどれだけ持っているかの数を返す
	public int GetNum(string key) {
		return ItemStaticData.itemDictionary [key];
	}
}