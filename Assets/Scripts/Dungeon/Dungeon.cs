using System;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon {

    /// <summary>
    /// 地图数据，value部分之后定义
    /// </summary>
    public Dictionary<Position, BaseTile> DungeonMap{
        get {
            return dungeonMap;
        }
    }
    private Dictionary<Position, BaseTile> dungeonMap;

    /// <summary>
    /// 误差
    /// </summary>
    private const int gap = 5;

    public int Size {
        get;
        private set;
    }

    public int RoomCount {
        get;
        private set;
    }

    public int RoomSize {
        get;
        private set;
    }

    public int Diffculty {
        get;
        private set;
    }

    public Position StartPoint {
        get;
        private set;
    }

    public Position EndPoint {
        get;
        private set;
    }

    public Dungeon(int size, int roomCount, int roomSize, int diffculty) {
        this.Size = size;
        this.RoomCount = roomCount;
        this.RoomSize = roomSize;
        this.Diffculty = diffculty;
        StartPoint = new Position(-1, -1);
        EndPoint = new Position(-1, -1);
    }

    public void GenerateDungeon() {
        dungeonMap = GenerateAllRoom();
    }

    public void DrawDungeon(Transform container) {
        Dictionary<Position, BaseTile>.Enumerator itor = dungeonMap.GetEnumerator();

        while (itor.MoveNext()) {
            Utils.DrawTile(itor.Current.Key, itor.Current.Value, container);
        }
    }

    public bool CanMove(CharacterDirction dirction, Position position) {
        switch (dirction) {
            case CharacterDirction.UP:
                Position top = position.Top();
                if (dungeonMap.ContainsKey(top)) {
                    return dungeonMap[top].reach;
                }
                break;
            case CharacterDirction.DOWN:
                Position bottom = position.Bottom();
                if (dungeonMap.ContainsKey(bottom)) {
                    return dungeonMap[bottom].reach;
                }
                break;
            case CharacterDirction.LEFT:
                Position left = position.Left();
                if (dungeonMap.ContainsKey(left)) {
                    return dungeonMap[left].reach;
                }
                break;
            case CharacterDirction.RIGHT:
                Position right = position.Right();
                if (dungeonMap.ContainsKey(right)) {
                    return dungeonMap[right].reach;
                }
                break;
        }

        return false;
    }

    public bool IsInPath(Position position) {
        Position top = position.Top();
        Position left = position.Left();
        Position right = position.Right();
        Position bottom = position.Bottom();

        Position topLeft = top.Left();
        Position topRight = top.Right();
        Position bottomLeft = bottom.Left();
        Position bottomRight = bottom.Right();

        if (CanReach(top)) {
            if (CanReach(topRight) && CanReach(right)) {
                return false;
            }
        }

        if (CanReach(right)) {
            if (CanReach(bottomRight) && CanReach(bottom)) {
                return false;
            }
        }

        if (CanReach(bottom)) {
            if (CanReach(bottomLeft) && CanReach(left)) {
                return false;
            }
        }

        if (CanReach(left)) {
            if (CanReach(topLeft) && CanReach(top)) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 判断一个positon在map中是否存在，如果存在，是否reach
    /// </summary>
    /// <returns><c>true</c>, if reach was caned, <c>false</c> otherwise.</returns>
    private bool CanReach(Position position) {
        return dungeonMap.ContainsKey(position) && dungeonMap[position].reach;
    }

    /// <summary>
    /// 1.递归生成一个一个room
    /// 2.检查该room是否与总地图有重合部分（相邻也不行）
    /// 3.生成该room与上一个room的路径
    /// 4.将该room加入到总地图中
    /// 5.生成所有的room和路径
    /// 6.填补room中的缝隙（填补生成瑕疵）
    /// 7.添加总地图的包围区域
    /// </summary>
    /// <returns>The all room.</returns>
    private Dictionary<Position, BaseTile> GenerateAllRoom() {
        Dictionary<Position, BaseTile> map = new Dictionary<Position, BaseTile>();

        Dictionary<Position, BaseTile> oldMap = null;
        for (int i = 0; i < RoomCount;) {
            Position startPos = GenerateRandomPosition();

            if (i == 0) {
                StartPoint = startPos;
            }
            if (i == RoomCount - 1) {
                EndPoint = startPos;
            }

            Dictionary<Position, BaseTile> newMap = GenerateRoom(startPos, RoomSize);
            //SetUpSlot(newMap);

            if (!isLap(map, newMap) && newMap.Count >= RoomSize - gap) {
                SetPath(map, oldMap, newMap);
                MergeMap(map, newMap);
                oldMap = newMap;
                i++;
            }
        }

        Debug.LogWarning("EndPoint: " + EndPoint.row + " " + EndPoint.col);
        SetUpSlot(map);
        GenerateUnreach(map);

        return map;
    }

    private Dictionary<Position, BaseTile> GenerateRoom(Position startPos, int step) {
        Dictionary<Position, BaseTile> map = new Dictionary<Position, BaseTile>();
        GenerateStep(map, startPos, ref step);
        return map;
    }

    private void GenerateStep(Dictionary<Position, BaseTile> map, Position pos, ref int step) {
        if (step <= 0 || !IsValidPosition(pos) || map.ContainsKey(pos)) {
            return;
        }

        if (EndPoint == pos) {
            map.Add(pos, new DekuchiTile());
        } else {
            map.Add(pos, new ReachTile());
        }
        step--;

        bool isSet = false;
        if (UnityEngine.Random.Range(0f, 1f) > .5f) {
            GenerateStep(map, new Position(pos.row - 1, pos.col), ref step);
            isSet = true;
        }

        if (UnityEngine.Random.Range(0f, 1f) > .5f) {
            GenerateStep(map, new Position(pos.row, pos.col - 1), ref step);
            isSet = true;
        }

        if (UnityEngine.Random.Range(0f, 1f) > .5f) {
            GenerateStep(map, new Position(pos.row + 1, pos.col), ref step);
            isSet = true;
        }

        if (!isSet || UnityEngine.Random.Range(0f, 1f) > .5f) {
            GenerateStep(map, new Position(pos.row, pos.col + 1), ref step);
        }
    }

    private bool isLap(Dictionary<Position, BaseTile> map, Dictionary<Position, BaseTile> newMap) {
        Dictionary<Position, BaseTile>.Enumerator itor = newMap.GetEnumerator();
        while (itor.MoveNext()) {
            if (map.ContainsKey(itor.Current.Key)) {
                return true;
            }

            if (map.ContainsKey(itor.Current.Key.Left())) {
                return true;
            }

            if (map.ContainsKey(itor.Current.Key.Right())) {
                return true;
            }

            if (map.ContainsKey(itor.Current.Key.Bottom())) {
                return true;
            }

            if (map.ContainsKey(itor.Current.Key.Top())) {
                return true;
            }
        }

        return false;
    }

    private Position GenerateRandomPosition() {
        return new Position(UnityEngine.Random.Range(0, Size), UnityEngine.Random.Range(0, Size));
    }

    public bool IsValidPosition(Position positon) {
        return positon.row >= 0 && positon.row < Size && positon.col >= 0 && positon.col < Size;
    }

    /// <summary>
    /// 填补生成的map的缝隙
    /// </summary>
    /// <param name="map">Map.</param>
    private void SetUpSlot(Dictionary<Position, BaseTile> map) {
        Queue<Position> PosQue = new Queue<Position>(map.Keys);
        while (PosQue.Count > 0) {
            Position pos = PosQue.Dequeue();

            Position left = pos.Left();
            if (IsValidPosition(left) && !map.ContainsKey(left)) {
                if (IsSurround(left, map)) {
                    map.Add(left, new ReachTile());
                    PosQue.Enqueue(left);
                }
            }

            Position right = pos.Right();
            if (IsValidPosition(right) && !map.ContainsKey(right)) {
                if (IsSurround(right, map)) {
                    map.Add(right, new ReachTile());
                    PosQue.Enqueue(right);
                }
            }

            Position bottom = pos.Bottom();
            if (IsValidPosition(bottom) && !map.ContainsKey(bottom)) {
                if (IsSurround(bottom, map)) {
                    map.Add(bottom, new ReachTile());
                    PosQue.Enqueue(bottom);
                }
            }

            Position top = pos.Top();
            if (IsValidPosition(top) && !map.ContainsKey(top)) {
                if (IsSurround(top, map)) {
                    map.Add(top, new ReachTile());
                    PosQue.Enqueue(top);
                }
            }
        }
    }

    /// <summary>
    /// 判断当前格子是否被包围，（至少3个格子）
    /// </summary>
    /// <returns><c>true</c>, if surround was ised, <c>false</c> otherwise.</returns>
    /// <param name="pos">Position.</param>
    /// <param name="map">Map.</param>
    private bool IsSurround(Position pos, Dictionary<Position, BaseTile> map) {

        return SurroundCount(pos, map) > 2;
    }

    private int SurroundCount(Position pos, Dictionary<Position, BaseTile> map,  bool checkEightDir = false, bool checkReach = false) {
        int count = 0;

        Position left = pos.Left();
        if (map.ContainsKey(left)) {
            if (!checkReach || map[left].reach)
                count++;
        }

        Position right = pos.Right();
        if (map.ContainsKey(right)) {
            if (!checkReach || map[right].reach)
                count++;
        }

        Position bottom = pos.Bottom();
        if (map.ContainsKey(bottom)) {
            if (!checkReach || map[bottom].reach)
                count++;
        }

        Position top = pos.Top();
        if (map.ContainsKey(top)) {
            if (!checkReach || map[top].reach)
                count++;
        }

        if (checkEightDir) {
            Position topRight = top.Right();
            if (map.ContainsKey(topRight)) {
                if (!checkReach || map[topRight].reach)
                    count++;
            }

            Position topLeft = top.Left();
            if (map.ContainsKey(topLeft)) {
                if (!checkReach || map[topLeft].reach)
                    count++;
            }

            Position bottomRight = bottom.Right();
            if (map.ContainsKey(bottomRight)) {
                if (!checkReach || map[bottomRight].reach)
                    count++;
            }

            Position bottomLeft = bottom.Left();
            if (map.ContainsKey(bottomLeft)) {
                if (!checkReach || map[bottomLeft].reach)
                    count++;
            }
        }

        return count;
    }

    /// <summary>
    /// 进行merge操作
    /// </summary>
    /// <param name="map">Map.</param>
    /// <param name="newMap">New map.</param>
    private void MergeMap(Dictionary<Position, BaseTile> map, Dictionary<Position, BaseTile> newMap) {
        Dictionary<Position, BaseTile>.Enumerator itor = newMap.GetEnumerator();
        while (itor.MoveNext()) {
            if (!map.ContainsKey(itor.Current.Key)) {
                map.Add(itor.Current.Key, itor.Current.Value);
            }
        }
    }

    /// <summary>
    /// 添加链接两个room的路径
    /// </summary>
    /// <param name="map">Map.</param>
    /// <param name="oldMap">Old map.</param>
    /// <param name="newMap">New map.</param>
    private void SetPath(Dictionary<Position, BaseTile> map, Dictionary<Position, BaseTile> oldMap, Dictionary<Position, BaseTile> newMap) {

        if (oldMap == null) {
            return;
        }

        int startIndex = UnityEngine.Random.Range(0, oldMap.Count);
        Position startPos = new List<Position>(oldMap.Keys)[startIndex];

        int endIndex = UnityEngine.Random.Range(0, newMap.Count);
        Position endPos = new List<Position>(newMap.Keys)[endIndex];

        int currentCol = startPos.col;
        while (currentCol != endPos.col) {
            Position pos = new Position(startPos.row, currentCol);
            if (!map.ContainsKey(pos) && !newMap.ContainsKey(pos)) {
                map.Add(pos, new ReachTile());
            }

            currentCol = currentCol + (currentCol < endPos.col ? 1 : -1);
        }
        Position colPos = new Position(startPos.row, currentCol);
        if (!map.ContainsKey(colPos) && !newMap.ContainsKey(colPos)) {
            map.Add(colPos, new ReachTile());
        }

        int currentRow = endPos.row;
        while (currentRow != startPos.row) {
            Position pos = new Position(currentRow, endPos.col);
            if (!map.ContainsKey(pos) && !newMap.ContainsKey(pos)) {
                map.Add(pos, new ReachTile());
            }
            currentRow = currentRow + (currentRow < startPos.row ? 1 : -1);
        }
        Position rowPos = new Position(currentRow, endPos.col);
        if (!map.ContainsKey(rowPos) && !newMap.ContainsKey(rowPos)) {
            map.Add(rowPos, new ReachTile());
        }
    }

    /// <summary>
    /// 为地格创建包围区域，不可进入
    /// </summary>
    private void GenerateUnreach(Dictionary<Position, BaseTile> map) {
        List<Position> posList = new List<Position>(map.Keys);
        foreach (Position pos in posList) {
            Position left = pos.Left();
            if (!map.ContainsKey(left)) {
                map.Add(left, new UnReach(ImageDirction.RIGHT));
            }

            Position right = pos.Right();
            if (!map.ContainsKey(right)) {
                map.Add(right, new UnReach(ImageDirction.LEFT));
            }

            Position top = pos.Top();
            if (!map.ContainsKey(top)) {
                map.Add(top, new UnReach(ImageDirction.DOWN));
            }

            Position bottom = pos.Bottom();
            if (!map.ContainsKey(bottom)) {
                map.Add(bottom, new UnReach(ImageDirction.UP));
            }
        }
    }
}