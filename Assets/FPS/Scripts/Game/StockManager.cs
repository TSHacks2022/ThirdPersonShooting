using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class StockManager : MonoBehaviour {


	public static StockManager instance;

	public static StockManager GetInstance(){
		return instance;
	}
 
	Dictionary<string, int> itemDictionary = new Dictionary<string, int>();
 
	// Use this for initialization
	void Start () {
		instance = this;

		itemDictionary.Add("HitPointPart", 0);
		itemDictionary.Add("AttackPart", 0);
		itemDictionary.Add("SpeedPart", 0);
		itemDictionary.Add("Attack", 0);
		itemDictionary.Add("Rapid", 0);

		foreach (var item in itemDictionary) {
			Debug.Log (item.Key + " : " + GetNum(item.Key));
		}
	}

    //　アイテムをどれだけ持っているかの数を返す
	public void PickItem(string key) {
		itemDictionary [key] += 1;
		Debug.Log (key + " : " + GetNum(key));
	}

	
 
	//　アイテムをどれだけ持っているかの数を返す
	public int GetNum(string key) {
		return itemDictionary [key];
	}
}