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
				for (int x = 0; x < mapSizeX; x++)
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

	private int SearchRange(int x, int y)
	{
		int index = -1;

		for (int i = 0; i < rangeList.Count; i++)
		{
			if (x >= rangeList[i].Start.X && x <= rangeList[i].End.X && y >= rangeList[i].Start.Y && y <= rangeList[i].End.Y)
			{
				index = i;
				break;
			}
		}

		return index;
	}

	private void CheckRangeConnection(ref int[,] map)
	{
		// 既に調べた区画のリスト
		List<int> checkedRangeList = new List<int>();
		int maxTotalArea = 0;
		List<int> prevRangeList = new List<int>();
		List<Position> prevPassPositionList = new List<Position>();
		int index = -1;
		foreach (Range range in rangeList)
		{
			index++;
			// 既に調べた区画なら飛ばす
			if (checkedRangeList.Contains(index))
			{
				continue;
			}

			checkedRangeList.Add(index);

			List<int> nowRangeList = new List<int>();
			Range connectedRange;
			int connectedRangeIndex;
			int x, y;
			int nowX = -1, nowY = -1;
			int nowDirection = -1;
			int dx = 0, dy = 0;
			int startX;
			int startY;

			// スタート地点の区画をリストに入れる
			nowRangeList.Add(index);

			// スタート地点の区画の面積をareaの初期値にする
			int area = (range.End.X - range.Start.X) * (range.End.Y - range.Start.Y);

			/*
			 * 左手法で探索する
			 * 地面でありかつ左側が壁に接している地点からスタート
			 * 現在の向きnowDirection:0→上，1→右，2→下，3→左
			*/
			for (x = range.Start.X; x <= range.End.X; x++)
            {
				for (y = range.Start.Y; y <= range.End.Y; y++)
                {
					if (x > 0 && x < mapSizeX && y > 0 && y < mapSizeY)
                    {
						if (map[x, y] == 1)
                        {
							// 上が壁の場合
							if (map[x, y - 1] == 0)
                            {
								nowX = x;
								nowY = y;
								nowDirection = 1;
								break;
                            }
							// 右が壁の場合
							if (map[x + 1, y] == 0)
							{
								nowX = x;
								nowY = y;
								nowDirection = 2;
								break;
							}
							// 下が壁の場合
							if (map[x, y + 1] == 0)
							{
								nowX = x;
								nowY = y;
								nowDirection = 3;
								break;
							}
							// 左が壁の場合
							if (map[x, y - 1] == 0)
							{
								nowX = x;
								nowY = y;
								nowDirection = 0;
								break;
							}
						}

					}
                }
				if (nowX != -1 && nowY != -1)
				{
					break;
				}
			}

			// 区画が全て壁だったらスキップする
			if (nowX == -1 && nowY == -1)
            {
				continue;
            }

			// スタート地点を保存する
			startX = nowX;
			startY = nowY;

			/*
			 * 前に進めていく
			 * どこかの区画(まだcheckedRangeに入っていない区画)に到達するごとに，その区画をcheckedRangeに入れ，その区画の面積をareaに足す
			 * スタート地点に2回戻ってきたら終わり
			*/
			int startCount = -1;
			int initialStep = 0;
			List<int> nowPassPositionX = new List<int>();
			List<int> nowPassPositionY = new List<int>();
			bool isPassed;
			while (true)
            {
				// 現在地がスタート地点か確認する
				if (nowX == startX && nowY == startY)
                {
					startCount++;
					if (startCount >= 2)
                    {
						break;
                    }
                }
				else
                {
					initialStep = 0;
                }

				// 現在の向きから移動差分を求める
				if (nowDirection == 0)
                {
					dx = 0;
					dy = -1;
                }
				if (nowDirection == 1)
				{
					dx = 1;
					dy = 0;

				}
				if (nowDirection == 2)
				{
					dx = 0;
					dy = 1;
				}
				if (nowDirection == 3)
				{
					dx = -1;
					dy = 0;
				}

				// 前に壁がある場合は右を向く
				if (map[nowX + dx, nowY + dy] == 0)
                {
					nowDirection++;
					if (nowDirection >= 4)
                    {
						nowDirection = 0;
                    }
					// スタート地点で4回足踏みしたらstartCountを1にして次のループで終了する
					if (nowX == startX && nowY == startY)
                    {
						startCount -= 1;
						initialStep += 1;
						if (initialStep >= 4)
                        {
							startCount = 1;
                        }
                    }
					continue;
                }

				// 現在地点を登録
				isPassed = false;
				for (int i = 0; i < nowPassPositionX.Count; i++)
                {
					if (nowPassPositionX[i] == nowX && nowPassPositionX[i] == nowY)
                    {
						isPassed = true;
					}
                }
				if (isPassed == false)
                {
					nowPassPositionX.Add(nowX);
					nowPassPositionY.Add(nowY);
				}

				// 移動する
				nowX = nowX + dx;
				nowY = nowY + dy;

				// 左が空いている場合は左を向く
				if (nowDirection == 0 && map[nowX - 1, nowY] == 1)
				{
					nowDirection = 3;
				}
				else if (nowDirection == 1 && map[nowX, nowY - 1] == 1)
				{
					nowDirection = 0;

				}
				else if (nowDirection == 2 && map[nowX + 1, nowY] == 1)
				{
					nowDirection = 1;
				}
				else if (nowDirection == 3 && map[nowX, nowY + 1] == 1)
				{
					nowDirection = 2;
				}

				// 移動先の区画を確認する
				connectedRangeIndex = SearchRange(nowX, nowY);
				if (connectedRangeIndex != -1)
				{
					connectedRange = rangeList[connectedRangeIndex];
					if (!checkedRangeList.Contains(connectedRangeIndex))
					{
						checkedRangeList.Add(connectedRangeIndex);
						nowRangeList.Add(connectedRangeIndex);
						area += (connectedRange.End.X - connectedRange.Start.X) * (connectedRange.End.Y - connectedRange.Start.Y);
					}
				}
			}

			/*
			 * areaがmaxTotalAreaより小さいならnowRangeListにある区画と部屋(roomList参照)を全て埋め，prevRangeListをnowRangeListで上書きする
			 * index > 0かつareaがmaxTotalAreaより大きいならprevRangeListにある区画と部屋(roomList参照)を全て埋める
			*/
			if (area > maxTotalArea)
			{
				maxTotalArea = area;
				// 前回まで最大だった区画群を埋める
				if (index > 0)
				{
					foreach (int prevRangeIndex in prevRangeList)
                    {
						for (x = rangeList[prevRangeIndex].Start.X; x <= rangeList[prevRangeIndex].End.X; x++)
						{
							for (y = rangeList[prevRangeIndex].Start.Y; y <= rangeList[prevRangeIndex].End.Y; y++)
							{
								if (map[x, y] == 1)
								{
									map[x, y] = 0;
								}
							}
						}
						List<int> removeList = new List<int>();
						for (int i = 0; i < roomList.Count; i++)
						{
							if (roomList[i].Start.X >= rangeList[prevRangeIndex].Start.X && roomList[i].End.X <= rangeList[prevRangeIndex].End.X && roomList[i].Start.Y >= rangeList[prevRangeIndex].Start.Y && roomList[i].End.Y <= rangeList[prevRangeIndex].End.Y)
							{
								removeList.Add(i);
							}
						}
						foreach (int removeIndex in removeList)
						{
							roomList.RemoveAt(removeIndex);
						}
					}
					foreach (Position prevPassPosition in prevPassPositionList)
                    {
						if (map[prevPassPosition.X, prevPassPosition.Y] == 1)
                        {
							map[prevPassPosition.X, prevPassPosition.Y] = 0;

						}
                    }
				}
				prevRangeList.Clear();
				foreach (int nowRangeIndex in nowRangeList)
                {
					prevRangeList.Add(nowRangeIndex);
				}
				prevPassPositionList.Clear();
				for (int i = 0; i < nowPassPositionX.Count; i++)
                {
					prevPassPositionList.Add(new Position(nowPassPositionX[i], nowPassPositionY[i]));
                }

			}
			// そうでなければ壁で埋める
			else
			{
				foreach (int nowRangeIndex in nowRangeList)
				{
					for (x = rangeList[nowRangeIndex].Start.X; x <= rangeList[nowRangeIndex].End.X; x++)
					{
						for (y = rangeList[nowRangeIndex].Start.Y; y <= rangeList[nowRangeIndex].End.Y; y++)
						{
							if (map[x, y] == 1)
							{
								map[x, y] = 0;
							}
						}
					}
					List<int> removeList = new List<int>();
					for (int i = 0; i < roomList.Count; i++)
					{
						if (roomList[i].Start.X >= rangeList[nowRangeIndex].Start.X && roomList[i].End.X <= rangeList[nowRangeIndex].End.X && roomList[i].Start.Y >= rangeList[nowRangeIndex].Start.Y && roomList[i].End.Y <= rangeList[nowRangeIndex].End.Y)
						{
							removeList.Add(i);
						}
					}
					foreach (int removeIndex in removeList)
					{
						roomList.RemoveAt(removeIndex);
					}
				}
				for (int i = 0; i < nowPassPositionX.Count; i++)
				{
					if (map[nowPassPositionX[i], nowPassPositionY[i]] == 1)
					{
						map[nowPassPositionX[i], nowPassPositionY[i]] = 0;
					}
				}
			}
		}
	}

	private void PlaceObjects(ref int[,] map, int enemyNum)
    {
		PlaceStair(ref map);
		PlacePlayer(ref map);
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