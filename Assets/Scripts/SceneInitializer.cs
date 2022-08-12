using UnityEngine;
using System.Collections.Generic;

public class SceneInitializer : MonoBehaviour
{

	public const int MAP_SIZE_X = 30;
	public const int MAP_SIZE_Y = 30;

    public const int ONE_TILE_SIZE = 3;
	public const int ITEM_NUM = 3;

	public const int MAX_ROOM_NUMBER = 6;

	public GameObject _player;
	public GameObject _stair;

	private GameObject floorPrefab;
	private GameObject wallPrefab;
	private GameObject enemyPrefab;

	public int enemyNum = 3;

	private string enemyObject;

	private int[,] map;

	// Use this for initialization
	void Start()
	{
		GenerateMap();
		EnemySelect();
		InstantiateMap();
		EnemyUpgrade();
	}

	private void GenerateMap()
	{
		map = new MapGenerator().GenerateMap(MAP_SIZE_X, MAP_SIZE_Y, MAX_ROOM_NUMBER, enemyNum + Random.Range(-2, 3), ITEM_NUM);
	}

	private void InstantiateMap()
    {
		wallPrefab = Resources.Load("Prefabs/Wall") as GameObject;
		enemyPrefab = Resources.Load(enemyObject) as GameObject;

		var floorList = new List<Vector3>();
		var wallList = new List<Vector3>();


		for (int y = 0; y < MAP_SIZE_Y; y++)
		{
			for (int x = 0; x < MAP_SIZE_X; x++)
			{
				if (map[x, y] == 2)
				{
					_stair.transform.position = new Vector3(ONE_TILE_SIZE * x, 0, ONE_TILE_SIZE * y);
				}
				else if (map[x, y] == 3)
				{
					_player.transform.position = new Vector3(ONE_TILE_SIZE * x, 0.1f, ONE_TILE_SIZE * y);
				}
				else if (map[x, y] == 4)
				{
					Instantiate(enemyPrefab, new Vector3(ONE_TILE_SIZE * x, 0, ONE_TILE_SIZE * y), new Quaternion());
				}
				else if (map[x, y] == 5)
				{
					Instantiate(ItemSelect(), new Vector3(ONE_TILE_SIZE * x, 0.1f, ONE_TILE_SIZE * y), new Quaternion());
				}
				else if (map[x, y] != 1)
				{
					Instantiate(wallPrefab, new Vector3(ONE_TILE_SIZE * x, 2.5f, ONE_TILE_SIZE * y), new Quaternion());
				}
			}
		}
	}

	private void EnemyUpgrade()
    {
		Health _health;
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (var enemy in enemies)
		{
			_health = enemy.GetComponent<Health>();
			_health.MaxHealth += StaticData.floor * 10;
		}

		if(StaticData.floor % 3 == 0)
        {
			enemyNum++;
		}
	}

	private void EnemySelect()
    {
		if(StaticData.floor / 5 < 1)
        {
			enemyObject = "Prefabs/Enemy_HoverBot_Easy";
		}
		else if(StaticData.floor / 5 < 2)
        {
			enemyObject = "Prefabs/Enemy_HoverBot_Normal";

		}
		else
		{
			enemyObject = "Prefabs/Enemy_HoverBot_Hard";
		}
	}

	private GameObject ItemSelect()
    {
        int rand = Random.Range(0, 5);
		var itemPath = "";
		GameObject item;

		if(rand == 0)
        {
			itemPath = "Prefabs/Pickup_Potion";
		}
		else if(rand == 1)
        {
			itemPath = "Prefabs/Pickup_HitPointPart";
		}
		else if (rand == 2)
		{
			itemPath = "Prefabs/Pickup_AttackPart";
		}
		else if (rand == 3)
		{
			itemPath = "Prefabs/Pickup_SpeedPart";
		}
		else if (rand == 4)
		{
			itemPath = "Prefabs/Pickup_RapidPart";
		}

		item = Resources.Load(itemPath) as GameObject;

		return item;
	}
}