using UnityEngine;
using System.Collections.Generic;

public class SceneInitializer : MonoBehaviour
{

	public const int MAP_SIZE_X = 30;
	public const int MAP_SIZE_Y = 30;

    public const int ONE_TILE_SIZE = 3;
	public const int ENEMY_NUM = 3;

	public const int MAX_ROOM_NUMBER = 6;

	public GameObject _player;
	public GameObject _enemy;
	public GameObject _stair;

	private GameObject floorPrefab;
	private GameObject wallPrefab;

	private int[,] map;

	// Use this for initialization
	void Start()
	{
		GenerateMap();
		//SponePlayer();
		//SponeEnemy();
		InstantiateMap();
	}

	private void GenerateMap()
	{
		map = new MapGenerator().GenerateMap(MAP_SIZE_X, MAP_SIZE_Y, MAX_ROOM_NUMBER, ENEMY_NUM);

		string log = "";
		for (int y = 0; y < MAP_SIZE_Y; y++)
		{
			for (int x = 0; x < MAP_SIZE_X; x++)
			{
				if(map[x, y] == 1)
                {
					log += "1";
				}
				else if(map[x, y] == 2)
                {
					log += "2";
                }
                else
                {
					log += " ";
                }
			}
			log += "\n";
		}
		Debug.Log(log);
	}

	private void InstantiateMap()
    {
		//floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;
		wallPrefab = Resources.Load("Prefabs/Wall") as GameObject;

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
					Instantiate(_enemy, new Vector3(ONE_TILE_SIZE * x, 0, ONE_TILE_SIZE * y), Quaternion.identity);
				}
				else if (map[x, y] != 1)
				{
					Instantiate(wallPrefab, new Vector3(ONE_TILE_SIZE * x, 2.5f, ONE_TILE_SIZE * y), new Quaternion());
				}
			}
		}

		for (int y = 0; y < MAP_SIZE_Y; y++)
		{
			for (int x = 0; x < MAP_SIZE_X; x++)
			{
				if (map[x, y] == 2)
				{
					_stair.transform.position = new Vector3(ONE_TILE_SIZE * x, 0.2f, ONE_TILE_SIZE * y);
					break;
				}
			}
		}
	}

	private void SponePlayer()
	{
		if (!_player)
		{
			return;
		}

		Position position;
		do
		{
			var x = RogueUtils.GetRandomInt(0, MAP_SIZE_X - 1);
			var y = RogueUtils.GetRandomInt(0, MAP_SIZE_Y - 1);
			position = new Position(x, y);
		} while (map[position.X, position.Y] != 1);

		_player.transform.position = new Vector3(ONE_TILE_SIZE * position.X, 0, ONE_TILE_SIZE * position.Y);
	}

	private void SponeEnemy()
	{
		if (!_enemy)
		{
			return;
		}

		Position position;

		for(int i = 0; i < ENEMY_NUM; i++)
        {
			do
			{
				var x = RogueUtils.GetRandomInt(0, MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, MAP_SIZE_Y - 1);
				position = new Position(x, y);
			} while (map[position.X, position.Y] != 1);

			Instantiate(_enemy, new Vector3(ONE_TILE_SIZE * position.X, 0, ONE_TILE_SIZE * position.Y), Quaternion.identity);
		}
	}
}