using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class ItemManager : MonoBehaviour {


	public static ItemManager instance;
	public void Awake()
	{
	if(instance == null)
	{
		instance = this;
	}
	}
 
	//　アイテムの種類を設定
	public enum Item {
		Knife,
		Sword,
		Book
	};
 
	Dictionary<Item, int> itemDictionary = new Dictionary<Item, int>();
 
	// Use this for initialization
	void Start () {
		itemDictionary [Item.Knife] = 0;
		itemDictionary [Item.Sword] = 0;
 
		foreach (var item in itemDictionary) {
			Debug.Log (item.Key + " : " + GetNum(item.Key));
		}
	}

    //　アイテムをどれだけ持っているかの数を返す
	public void PickItem() {
		itemDictionary [Item.Sword] += 1;
		foreach (var item in itemDictionary) {
			Debug.Log (item.Key + " : " + GetNum(item.Key));
		}
	}
 
	//　アイテムをどれだけ持っているかの数を返す
	public int GetNum(Item key) {
		return itemDictionary [key];
	}
}