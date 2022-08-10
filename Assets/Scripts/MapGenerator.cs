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

		// �����܂ł̌��ʂ���x�z��ɔ��f����
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
		// ���̃��X�g�̏����l�Ƃ��ă}�b�v�S�̂�����
		rangeList.Add(new Range(0, 0, mapSizeX - 1, mapSizeY - 1));

		bool isDevided;
		do
		{
			// �c �� �� �̏��Ԃŕ�������؂��Ă����B�����؂�Ȃ�������I��
			isDevided = DevideRange(false);
			isDevided = DevideRange(true) || isDevided;

			// �������͍ő��搔�𒴂�����I��
			if (rangeList.Count >= maxRoom)
			{
				break;
			}
		} while (isDevided);

	}

	public bool DevideRange(bool isVertical)
	{
		bool isDevided = false;

		// ��悲�Ƃɐ؂邩�ǂ������肷��
		List<Range> newRangeList = new List<Range>();
		foreach (Range range in rangeList)
		{
			// ����ȏ㕪���ł��Ȃ��ꍇ�̓X�L�b�v
			if (isVertical && range.GetWidthY() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}
			else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}

			System.Threading.Thread.Sleep(1);

			// 40���̊m���ŕ������Ȃ�
			// �������A���̐���1�̎��͕K����������
			if (rangeList.Count > 1 && RogueUtils.RandomJadge(0.4f))
			{
				continue;
			}

			// ��������ŏ��̋��T�C�Y2���������A�c�肩�烉���_���ŕ����ʒu�����߂�
			int length = isVertical ? range.GetWidthY() : range.GetWidthX();
			int margin = length - MINIMUM_RANGE_WIDTH * 2;
			int baseIndex = isVertical ? range.Start.Y : range.Start.X;
			int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + RogueUtils.GetRandomInt(1, margin) - 1;

			// �������ꂽ���̑傫����ύX���A�V��������ǉ����X�g�ɒǉ�����
			// �����ɁA�����������E��ʘH�Ƃ��ĕۑ����Ă���
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

			// �ǉ����X�g�ɐV��������ޔ�����B
			newRangeList.Add(newRange);

			isDevided = true;
		}

		// �ǉ����X�g�ɑޔ����Ă������V��������ǉ�����B
		rangeList.AddRange(newRangeList);

		return isDevided;
	}

	private void CreateRoom()
	{
		// �����̂Ȃ���悪�΂�Ȃ��悤�Ƀ��X�g���V���b�t������
		rangeList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

		// 1��悠����1����������Ă����B���Ȃ���������B
		foreach (Range range in rangeList)
		{
			System.Threading.Thread.Sleep(1);
			// 30���̊m���ŕ��������Ȃ�
			// �������A�ő啔�����̔����ɖ����Ȃ��ꍇ�͍��
			if (roomList.Count > maxRoom / 2 && RogueUtils.RandomJadge(0.3f))
			{
				continue;
			}

			// �P�\���v�Z
			int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
			int marginY = range.GetWidthY() - MINIMUM_RANGE_WIDTH + 1;

			// �J�n�ʒu������
			int randomX = RogueUtils.GetRandomInt(1, marginX);
			int randomY = RogueUtils.GetRandomInt(1, marginY);

			// ���W���Z�o
			int startX = range.Start.X + randomX;
			int endX = range.End.X - RogueUtils.GetRandomInt(0, (marginX - randomX)) - 1;
			int startY = range.Start.Y + randomY;
			int endY = range.End.Y - RogueUtils.GetRandomInt(0, (marginY - randomY)) - 1;

			// �������X�g�֒ǉ�
			Range room = new Range(startX, startY, endX, endY);
			roomList.Add(room);

			// �ʘH�����
			CreatePass(range, room);
		}
	}

	private void CreatePass(Range range, Range room)
	{
		List<int> directionList = new List<int>();
		if (range.Start.X != 0)
		{
			// X�}�C�i�X����
			directionList.Add(0);
		}
		if (range.End.X != mapSizeX - 1)
		{
			// X�v���X����
			directionList.Add(1);
		}
		if (range.Start.Y != 0)
		{
			// Y�}�C�i�X����
			directionList.Add(2);
		}
		if (range.End.Y != mapSizeY - 1)
		{
			// Y�v���X����
			directionList.Add(3);
		}

		// �ʘH�̗L�����΂�Ȃ��悤�A���X�g���V���b�t������
		directionList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

		bool isFirst = true;
		foreach (int direction in directionList)
		{
			System.Threading.Thread.Sleep(1);
			// 80%�̊m���ŒʘH�����Ȃ�
			// �������A�܂��ʘH���Ȃ��ꍇ�͕K�����
			if (!isFirst && RogueUtils.RandomJadge(0.8f))
			{
				continue;
			}
			else
			{
				isFirst = false;
			}

			// �����̔���
			int random;
			switch (direction)
			{
				case 0: // X�}�C�i�X����
					random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(range.Start.X, random, room.Start.X - 1, random));
					break;

				case 1: // X�v���X����
					random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(room.End.X + 1, random, range.End.X, random));
					break;

				case 2: // Y�}�C�i�X����
					random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, range.Start.Y, random, room.Start.Y - 1));
					break;

				case 3: // Y�v���X����
					random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, room.End.Y + 1, random, range.End.Y));
					break;
			}
		}

	}

	private void TrimPassList(ref int[,] map)
	{
		// �ǂ̕����ʘH������ڑ�����Ȃ������ʘH���폜����
		for (int i = passList.Count - 1; i >= 0; i--)
		{
			Range pass = passList[i];

			bool isVertical = pass.GetWidthY() > 1;

			// �ʘH�������ʘH����ڑ�����Ă��邩�`�F�b�N
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

			// �폜�ΏۂƂȂ����ʘH���폜����
			if (isTrimTarget)
			{
				passList.Remove(pass);

				// �}�b�v�z�񂩂���폜
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

		// �O���ɐڂ��Ă���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
		// �㉺�
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
		// ���E�
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
		// ���ɒ��ׂ����̃��X�g
		List<Range> checkedRange = new List<Range>();
		int maxTotalArea = 0;
		int prevstartX = -1;
		int prevstartY = -1;
		int prevendX = -1;
		int prevendY = -1;

		foreach (Range range in rangeList)
		{
			// ���ɒ��ׂ����Ȃ��΂�
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

			// ��ԍ��̕�
			x = startX;
			if (x > 0)
			{
				for (y = range.Start.Y; y <= range.End.Y; y++)
				{
					// ���̋��ƌq�����Ă���
					if (map[x, y] == 1 && map[x - 1, y] == 1 && map[x - 2, y] == 1)
					{
						connectedRange = SearchRange(x - 2, y);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// �q�����Ă�����Q�̍��[�܂Œ��ׂ�
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
								if (map[x, y] == 1 && map[x - 1, y] == 1 && map[x - 2, y] == 1)
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
			// ��ԉE�̕�
			x = endX;
			if (x < mapSizeX)
			{
				for (y = range.Start.Y; y <= range.End.Y; y++)
				{
					// �E�̋��ƌq�����Ă���
					if (map[x, y] == 1 && map[x + 1, y] == 1 && map[x + 2, y] == 1)
					{
						connectedRange = SearchRange(x + 2, y);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// �q�����Ă�����Q�̉E�[�܂Œ��ׂ�
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
								if (map[x, y] == 1 && map[x + 1, y] == 1 && map[x + 2, y] == 1)
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
			// ��ԏ�̕�
			y = startY;
			if (y > 0)
			{
				for (x = range.Start.X; x <= range.End.X; x++)
				{
					// ��̋��ƌq�����Ă���
					if (map[x, y] == 1 && map[x, y - 1] == 1 && map[x, y - 2] == 1)
					{
						connectedRange = SearchRange(x, y - 2);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// �q�����Ă�����Q�̏�[�܂Œ��ׂ�
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
								if (map[x, y] == 1 && map[x, y - 1] == 1 && map[x, y - 2] == 1)
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
			// ��ԉ��̕�
			y = endY;
			if (y > 0)
			{
				for (x = range.Start.X; x <= range.End.X; x++)
				{
					// ���̋��ƌq�����Ă���
					if (map[x, y] == 1 && map[x, y + 1] == 1 && map[x, y + 2] == 1)
					{
						connectedRange = SearchRange(x, y + 2);
						if (!checkedRange.Contains(connectedRange))
						{
							checkedRange.Add(connectedRange);
						}
						bool isLimited;
						// �q�����Ă�����Q�̉��[�܂Œ��ׂ�
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
								if (map[x, y] == 1 && map[x, y + 1] == 1 && map[x, y + 2] == 1)
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

			// �q�����Ă�����Q�̖ʐς��v�Z
			area = (endX - startX) * (endY - startY);

			// ���̖ʐς��O�܂ł̍ő�ʐς��傫����΍X�V
			if (area > maxTotalArea)
			{
				maxTotalArea = area;
				// �O��܂ōő傾�������Q�𖄂߂�
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
			// �����łȂ���ΕǂŖ��߂�
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