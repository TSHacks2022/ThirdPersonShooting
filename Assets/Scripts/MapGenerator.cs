using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator
{

	private const int MINIMUM_RANGE_WIDTH = 6;

	private int mapSizeX;
	private int mapSizeY;
	private int maxRoom;
	private int stairRoomIdx;
	private int playerRoomIdx;

	private List<Range> roomList = new List<Range>();
	private List<Range> rangeList = new List<Range>();
	private List<Range> passList = new List<Range>();
	private List<Range> roomPassList = new List<Range>();


	public int[,] GenerateMap(int mapSizeX, int mapSizeY, int maxRoom, int enemyNum)
	{
		this.mapSizeX = mapSizeX;
		this.mapSizeY = mapSizeY;

		int[,] map = new int[mapSizeX, mapSizeY];

		CreateRange(maxRoom);
		CreateRoom();

		// ここまでの結果を一度配列に反映する
		foreach (Range pass in passList)
		{
			for (int x = pass.Start.X; x <= pass.End.X; x++)
			{
				for (int y = pass.Start.Y; y <= pass.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}
		foreach (Range roomPass in roomPassList)
		{
			for (int x = roomPass.Start.X; x <= roomPass.End.X; x++)
			{
				for (int y = roomPass.Start.Y; y <= roomPass.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}
		foreach (Range room in roomList)
		{
			for (int x = room.Start.X; x <= room.End.X; x++)
			{
				for (int y = room.Start.Y; y <= room.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}

        TrimPassList(ref map);

		CheckRangeConnection(ref map);

		PlaceObjects(ref map, enemyNum);

		return map;
	}

	public void CreateRange(int maxRoom)
	{
		// 区画のリストの初期値としてマップ全体を入れる
		rangeList.Add(new Range(0, 0, mapSizeX - 1, mapSizeY - 1));

		bool isDevided;
		do
		{
			// 縦 → 横 の順番で部屋を区切っていく。一つも区切らなかったら終了
			isDevided = DevideRange(false);
			isDevided = DevideRange(true) || isDevided;

			// もしくは最大区画数を超えたら終了
			if (rangeList.Count >= maxRoom)
			{
				break;
			}
		} while (isDevided);

	}

	public bool DevideRange(bool isVertical)
	{
		bool isDevided = false;

		// 区画ごとに切るかどうか判定する
		List<Range> newRangeList = new List<Range>();
		foreach (Range range in rangeList)
		{
			// これ以上分割できない場合はスキップ
			if (isVertical && range.GetWidthY() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}
			else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}

			System.Threading.Thread.Sleep(1);

			// 40％の確率で分割しない
			// ただし、区画の数が1つの時は必ず分割する
			if (rangeList.Count > 1 && RogueUtils.RandomJadge(0.4f))
			{
				continue;
			}

			// 長さから最少の区画サイズ2つ分を引き、残りからランダムで分割位置を決める
			int length = isVertical ? range.GetWidthY() : range.GetWidthX();
			int margin = length - MINIMUM_RANGE_WIDTH * 2;
			int baseIndex = isVertical ? range.Start.Y : range.Start.X;
			int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + RogueUtils.GetRandomInt(1, margin) - 1;

			// 分割された区画の大きさを変更し、新しい区画を追加リストに追加する
			// 同時に、分割した境界を通路として保存しておく
			Range newRange = new Range();
			if (isVertical)
			{
				passList.Add(new Range(range.Start.X, devideIndex, range.End.X, devideIndex));
				newRange = new Range(range.Start.X, devideIndex + 1, range.End.X, range.End.Y);
				range.End.Y = devideIndex - 1;
			}
			else
			{
				passList.Add(new Range(devideIndex, range.Start.Y, devideIndex, range.End.Y));
				newRange = new Range(devideIndex + 1, range.Start.Y, range.End.X, range.End.Y);
				range.End.X = devideIndex - 1;
			}

			// 追加リストに新しい区画を退避する。
			newRangeList.Add(newRange);

			isDevided = true;
		}

		// 追加リストに退避しておいた新しい区画を追加する。
		rangeList.AddRange(newRangeList);

		return isDevided;
	}

	private void CreateRoom()
	{
		// 部屋のない区画が偏らないようにリストをシャッフルする
		rangeList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

		// 1区画あたり1部屋を作っていく。作らない区画もあり。
		foreach (Range range in rangeList)
		{
			System.Threading.Thread.Sleep(1);
			// 30％の確率で部屋を作らない
			// ただし、最大部屋数の半分に満たない場合は作る
			if (roomList.Count > maxRoom / 2 && RogueUtils.RandomJadge(0.3f))
			{
				continue;
			}

			// 猶予を計算
			int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
			int marginY = range.GetWidthY() - MINIMUM_RANGE_WIDTH + 1;

			// 開始位置を決定
			int randomX = RogueUtils.GetRandomInt(1, marginX);
			int randomY = RogueUtils.GetRandomInt(1, marginY);

			// 座標を算出
			int startX = range.Start.X + randomX;
			int endX = range.End.X - RogueUtils.GetRandomInt(0, (marginX - randomX)) - 1;
			int startY = range.Start.Y + randomY;
			int endY = range.End.Y - RogueUtils.GetRandomInt(0, (marginY - randomY)) - 1;

			// 部屋リストへ追加
			Range room = new Range(startX, startY, endX, endY);
			roomList.Add(room);

			// 通路を作る
			CreatePass(range, room);
		}
	}

	private void CreatePass(Range range, Range room)
	{
		List<int> directionList = new List<int>();
		if (range.Start.X != 0)
		{
			// Xマイナス方向
			directionList.Add(0);
		}
		if (range.End.X != mapSizeX - 1)
		{
			// Xプラス方向
			directionList.Add(1);
		}
		if (range.Start.Y != 0)
		{
			// Yマイナス方向
			directionList.Add(2);
		}
		if (range.End.Y != mapSizeY - 1)
		{
			// Yプラス方向
			directionList.Add(3);
		}

		// 通路の有無が偏らないよう、リストをシャッフルする
		directionList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

		bool isFirst = true;
		foreach (int direction in directionList)
		{
			System.Threading.Thread.Sleep(1);
			// 80%の確率で通路を作らない
			// ただし、まだ通路がない場合は必ず作る
			if (!isFirst && RogueUtils.RandomJadge(0.8f))
			{
				continue;
			}
			else
			{
				isFirst = false;
			}

			// 向きの判定
			int random;
			switch (direction)
			{
				case 0: // Xマイナス方向
					random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(range.Start.X, random, room.Start.X - 1, random));
					break;

				case 1: // Xプラス方向
					random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(room.End.X + 1, random, range.End.X, random));
					break;

				case 2: // Yマイナス方向
					random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, range.Start.Y, random, room.Start.Y - 1));
					break;

				case 3: // Yプラス方向
					random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, room.End.Y + 1, random, range.End.Y));
					break;
			}
		}

	}

	private void TrimPassList(ref int[,] map)
	{
		// どの部屋通路からも接続されなかった通路を削除する
		for (int i = passList.Count - 1; i >= 0; i--)
		{
			Range pass = passList[i];

			bool isVertical = pass.GetWidthY() > 1;

			// 通路が部屋通路から接続されているかチェック
			bool isTrimTarget = true;
			if (isVertical)
			{
				int x = pass.Start.X;
				for (int y = pass.Start.Y; y <= pass.End.Y; y++)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						isTrimTarget = false;
						break;
					}
				}
			}
			else
			{
				int y = pass.Start.Y;
				for (int x = pass.Start.X; x <= pass.End.X; x++)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						isTrimTarget = false;
						break;
					}
				}
			}

			// 削除対象となった通路を削除する
			if (isTrimTarget)
			{
				passList.Remove(pass);

				// マップ配列からも削除
				if (isVertical)
				{
					int x = pass.Start.X;
					for (int y = pass.Start.Y; y <= pass.End.Y; y++)
					{
						map[x, y] = 0;
					}
				}
				else
				{
					int y = pass.Start.Y;
					for (int x = pass.Start.X; x <= pass.End.X; x++)
					{
						map[x, y] = 0;
					}
				}
			}
		}

		// 外周に接している通路を別の通路との接続点まで削除する
		// 上下基準
		for (int x = 0; x < mapSizeX - 1; x++)
		{
			if (map[x, 0] == 1)
			{
				for (int y = 0; y < mapSizeY; y++)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
			if (map[x, mapSizeY - 1] == 1)
			{
				for (int y = mapSizeY - 1; y >= 0; y--)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
		}
		// 左右基準
		for (int y = 0; y < mapSizeY - 1; y++)
		{
			if (map[0, y] == 1)
			{
				for (int x = 0; x < mapSizeY; x++)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
			if (map[mapSizeX - 1, y] == 1)
			{
				for (int x = mapSizeX - 1; x >= 0; x--)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
		}
	}

	private Range SearchRange(int x, int y)
	{
		Range returnRange = new Range();

		foreach (Range range in rangeList)
		{
			if (x >= range.Start.X && x <= range.End.X && y >= range.Start.Y && y <= range.End.Y)
			{
				returnRange = range;
				Debug.Log(x + ", " + y);
				break;
			}
		}

		return returnRange;
	}

	private void CheckRangeConnection(ref int[,] map)
	{
		// 既に調べた区画のリスト
		List<Range> checkedRange = new List<Range>();
		int maxTotalArea = 0;
		int prevstartX = -1;
		int prevstartY = -1;
		int prevendX = -1;
		int prevendY = -1;

		foreach (Range range in rangeList)
		{
			// 既に調べた区画なら飛ばす
			if (checkedRange.Contains(range))
			{
				continue;
			}

			checkedRange.Add(range);

			Range connectedRange;
			int x, y;
			int startX = range.Start.X;
			int startY = range.Start.Y;
			int endX = range.End.X;
			int endY = range.End.Y;
			int area;

			// 一番左の辺
			x = startX;
			if (x > 0)
			{
				for (y = range.Start.Y; y <= range.End.Y; y++)
				{
					// 左の区画と繋がっている
					if (map[x, y] == 1 && map[x - 1, y] == 1 && map[x - 2, y] == 1)
					{
						connectedRange = SearchRange(x - 2, y);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// 繋がっている区画群の左端まで調べる
						do
						{
							isLimited = true;
							x = connectedRange.Start.X;
							if (x <= 0)
							{
								break;
							}
							for (y = connectedRange.Start.Y; y <= connectedRange.End.Y; y++)
							{
								if (map[x, y] == 1 && map[x - 1, y] == 1)
								{
									isLimited = false;
									connectedRange = SearchRange(x - 2, y);
									if (!checkedRange.Contains(connectedRange))
									{
										checkedRange.Add(connectedRange);
									}
								}
							}
						} while (!isLimited);
						startX = connectedRange.Start.X;
						break;
					}
				}
			}
			// 一番右の辺
			x = endX;
			if (x < mapSizeX)
			{
				for (y = range.Start.Y; y <= range.End.Y; y++)
				{
					// 右の区画と繋がっている
					if (map[x, y] == 1 && map[x + 1, y] == 1 && map[x + 2, y] == 1)
					{
						connectedRange = SearchRange(x + 2, y);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// 繋がっている区画群の右端まで調べる
						do
						{
							isLimited = true;
							x = connectedRange.End.X;
							if (x >= mapSizeX)
							{
								break;
							}
							for (y = connectedRange.Start.Y; y <= connectedRange.End.Y; y++)
							{
								if (map[x, y] == 1 && map[x + 1, y] == 1)
								{
									isLimited = false;
									connectedRange = SearchRange(x + 2, y);
									if (!checkedRange.Contains(connectedRange))
									{
										checkedRange.Add(connectedRange);
									}
								}
							}
						} while (!isLimited);
						endX = connectedRange.End.X;
						break;
					}
				}
			}
			// 一番上の辺
			y = startY;
			if (y > 0)
			{
				for (x = range.Start.X; x <= range.End.X; x++)
				{
					// 上の区画と繋がっている
					if (map[x, y] == 1 && map[x, y - 1] == 1 && map[x, y - 2] == 1)
					{
						connectedRange = SearchRange(x, y - 2);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// 繋がっている区画群の上端まで調べる
						do
						{
							isLimited = true;
							y = connectedRange.Start.Y;
							if (y <= 0)
							{
								break;
							}
							for (x = connectedRange.Start.X; x <= connectedRange.End.X; x++)
							{
								if (map[x, y] == 1 && map[x, y - 1] == 1)
								{
									isLimited = false;
									connectedRange = SearchRange(x, y - 2);
									if (!checkedRange.Contains(connectedRange))
									{
										checkedRange.Add(connectedRange);
									}
								}
							}
						} while (!isLimited);
						startY = connectedRange.Start.Y;
						break;
					}
				}
			}
			// 一番下の辺
			y = endY;
			if (y > 0)
			{
				for (x = range.Start.X; x <= range.End.X; x++)
				{
					// 下の区画と繋がっている
					if (map[x, y] == 1 && map[x, y + 1] == 1 && map[x, y + 2] == 1)
					{
						connectedRange = SearchRange(x, y + 2);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// 繋がっている区画群の下端まで調べる
						do
						{
							isLimited = true;
							y = connectedRange.End.Y;
							if (y >= mapSizeY)
							{
								break;
							}
							for (x = connectedRange.Start.X; x <= connectedRange.End.X; x++)
							{
								if (map[x, y] == 1 && map[x, y + 1] == 1)
								{
									isLimited = false;
									connectedRange = SearchRange(x, y + 2);
									if (!checkedRange.Contains(connectedRange))
									{
										checkedRange.Add(connectedRange);
									}
								}
							}
						} while (!isLimited);
						endY = connectedRange.End.Y;
						break;
					}
				}
			}

			// 繋がっている区画群の面積を計算
			area = (endX - startX) * (endY - startY);

			// 今の面積が前までの最大面積より大きければ更新
			if (area > maxTotalArea)
			{
				maxTotalArea = area;
				// 前回まで最大だった区画群を埋める
				if (prevstartX != -1)
                {
					for (x = prevstartX; x <= prevendX; x++)
					{
						for (y = prevstartY; y <= prevendY; y++)
						{
							if (map[x, y] == 1)
							{
								map[x, y] = 0;
							}
						}
					}
				}
				prevstartX = startX;
				prevstartY = startY;
				prevendX = endX;
				prevendY = endY;
			}
			// そうでなければ壁で埋める
			else
			{
				for (x = startX; x <= endX; x++)
				{
					for (y = startY; y <= endY; y++)
					{
						if (map[x, y] == 1)
						{
							map[x, y] = 0;
						}
					}
				}
			}
		}
	}

	private void PlaceObjects(ref int[,] map, int enemyNum)
    {
		PlacePlayer(ref map);
		PlaceStair(ref map);
		PlaceEnemy(ref map, enemyNum);
    }

	private void PlaceStair(ref int[,] map)
    {
		stairRoomIdx = Random.Range(0, roomList.Count);
		Range stairRoom = roomList[stairRoomIdx];
		int x = Random.Range(stairRoom.Start.X, stairRoom.End.X + 1);
		int y = Random.Range(stairRoom.Start.Y, stairRoom.End.Y + 1);

		map[x, y] = 2;
	}

	private void PlacePlayer(ref int[,] map)
	{
		do
		{
			playerRoomIdx = Random.Range(0, roomList.Count);
		}
		while (playerRoomIdx == stairRoomIdx);
		Range playerRoom = roomList[playerRoomIdx];
		int x = Random.Range(playerRoom.Start.X, playerRoom.End.X + 1);
		int y = Random.Range(playerRoom.Start.Y, playerRoom.End.Y + 1);

		map[x, y] = 3;
	}

	private void PlaceEnemy(ref int[,] map, int enemyNum)
	{
		int enemyRoomIdx;
		for (int i = 0; i < enemyNum; i++)
        {
			do
			{
				enemyRoomIdx = Random.Range(0, roomList.Count);
		}
			while (enemyRoomIdx == stairRoomIdx || enemyRoomIdx == playerRoomIdx);
			Range enemyRoom = roomList[enemyRoomIdx];
			int x = Random.Range(enemyRoom.Start.X, enemyRoom.End.X + 1);
			int y = Random.Range(enemyRoom.Start.Y, enemyRoom.End.Y + 1);

			map[x, y] = 4;
		}
	}
}