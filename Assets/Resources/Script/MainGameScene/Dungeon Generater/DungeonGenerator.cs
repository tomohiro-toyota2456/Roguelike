using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public const int WIDTH = 100;
    public const int HIGHT = 100;
    //private const int RANDOM_RANGE = 10;
    public enum MAP_CHIP
    {
        WALL = 0,
        ROOM = 1,
        ROAD = 2,
        NO_DESTORY_WALL = 9
    }

    //部屋の最低サイズ
    public const int MIN_ROOM_SIZE = 3;
    //部屋の最大サイズ
    public const int MAX_ROOM_SIZE = 10;

    //余白
    public const int OUTER_MERGIN = 3;

    public const int MAX_ROOM = 10;

    //部屋配置に余白サイズ
    public const int POS_MERGIN = 3;

    /// <summary>
    /// 区画リスト
    /// </summary>
    List<LayerArea> areaList = null;

    // Use this for initialization
    void Start()
    {
        // Generater();

    }

    public int[,] Generater(int Width = WIDTH, int Hight = HIGHT)
    {
        int[,] map = new int[Width, Hight];
        for (int y = 0; y < Hight; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map[y, x] = (int)MAP_CHIP.NO_DESTORY_WALL; //破壊不能オブジェクトIDで初期化
            }
        }
        //区画のリスト
        areaList = new List<LayerArea>();
        //外周は破壊不能の壁にするため１回り小さく
        CreateArea(1, Width - 1, 1, Hight - 1);
        //縦・横どちらかに分割
        bool isVertical = (Random.Range(0, 2) == 0);
        SplitArea(isVertical);

        CreateRoom(map);
        ConnectRooms(map);
        return map;
    }

    /// <summary>
    /// 初回の区画を作成
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Right.</param>
    /// <param name="top">Top.</param>
    /// <param name="bottom">Bottom.</param>
    void CreateArea(int left, int right, int top, int bottom)
    {
        LayerArea area = new LayerArea();
        //大きな区画（全体マップ情報の保存）
        area.outer.Set(left, right, top, bottom);
        areaList.Add(area);
    }

    void SplitArea(bool isVertical, int areaNum = 7)
    {
        if (areaNum <= 0)
        {
            return;
        }
        LayerArea parent = areaList[areaList.Count - 1];
        areaList.Remove(parent);

        LayerArea child = new LayerArea();

        //縦方向に分割
        if (isVertical)
        {
            //高さを見て分割するか
            if (CheckSplitSize(parent.outer.Hight) == false)
            {
                areaList.Add(parent);
                return;
            }
            //分割のポイントを設定
            //外周のポイントに最小部屋サイズと壁との差を加える
            int pointA = parent.outer.areaTop + (MIN_ROOM_SIZE + OUTER_MERGIN);
            //外周のポイントに最大部屋サイズと壁との差を加える
            int pointB = parent.outer.areaBottom - (MAX_ROOM_SIZE + OUTER_MERGIN);
            // A,Bの間から分割ポイントをランダムで選択する
            int p = Random.Range(pointA, pointB);

            //分割したエリアを子区画として設定
            child.outer.Set(parent.outer.areaLeft, parent.outer.areaRight, p, parent.outer.areaBottom);

            //親の下をpに設定
            parent.outer.areaBottom = child.outer.areaTop;
        }
        //横方向に分割
        else
        {
            //幅を見て分割するか
            if (CheckSplitSize(parent.outer.Width) == false)
            {
                areaList.Add(parent);
                return;
            }
            //分割のポイントを設定
            //外周のポイントに最小部屋サイズと壁との差を加える
            int pointA = parent.outer.areaLeft + (MIN_ROOM_SIZE + OUTER_MERGIN);
            int pointB = parent.outer.areaRight - (MAX_ROOM_SIZE + OUTER_MERGIN);
            int p = Random.Range(pointA, pointB);

            //分割したエリアを子区画として設定
            child.outer.Set(p, parent.outer.areaRight, parent.outer.areaTop, parent.outer.areaBottom);

            //親の下をpに設定
            parent.outer.areaRight = child.outer.areaLeft;
        }

        if (Random.Range(0, 2) == 0)
        {
            //子供を分割
            areaList.Add(parent);
            areaList.Add(child);
        }
        else
        {
            //親を分割
            areaList.Add(child);
            areaList.Add(parent);
        }
        areaNum--;
        SplitArea(!isVertical, areaNum);
    }//SplitArea

    //掘れるかチェック
    bool CheckSplitSize(int size)
    {
        //+1 = 通路用
        int min = (MIN_ROOM_SIZE + OUTER_MERGIN) * 2 + 1;
        return size >= min;
    }

    void CreateRoom(int[,] map)
    {
        foreach (LayerArea div in areaList)
        {
            //　基準サイズ 部屋の大きさ
            // 部屋の最大サイズ＋余白分を確保する
            int dw = div.outer.Width - (OUTER_MERGIN + MAX_ROOM_SIZE);
            int dh = div.outer.Hight - (OUTER_MERGIN + MAX_ROOM_SIZE);
            //万が一最小部屋サイズより小さかった場合最小サイズにする
            dw = Mathf.Max(MIN_ROOM_SIZE, dw);
            dh = Mathf.Max(MIN_ROOM_SIZE, dh);
            // 大きさをランダムに決める
            int sw = Random.Range(MIN_ROOM_SIZE, dw);
            int sh = Random.Range(MIN_ROOM_SIZE, dh);

            //最大サイズを超えないようにする(ここあとで変える)
            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            //空きサイズを計算（区画-部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);

            //部屋の左上位置を決める
            int rx = Random.Range(0, rw) + POS_MERGIN;
            int ry = Random.Range(0, rh) + POS_MERGIN;

            int left = div.outer.areaLeft + rx;
            int right = left + sw;
            int top = div.outer.areaTop + ry;
            int bottom = top + sh;

            // 部屋サイズを設定
            div.room.Set(left, right, top, bottom);

            // 部屋を生成
            for (int topS = top; topS < bottom; topS++)
            {
                for (int leftS = left; leftS < right; leftS++)
                {
                    map[topS, leftS] = (int)MAP_CHIP.ROOM;
                }
            }//部屋生成終了

        }
    }// CreateRoom

    void ConnectRooms(int[,] map)
    {
        for (int i = 0; i < areaList.Count - 1; i++)
        {
            LayerArea a = areaList[i];
            LayerArea b = areaList[i + 1];
            //２つの部屋を繋ぐ
            CreateRoad(a, b, false, map);
            //孫と通路を繋げる
            //if(Random.Range(0,2) == 0)
           // {
                for (int j = i + 2; j < areaList.Count; j++)
                {
                    LayerArea c = areaList[j];
                    if (CreateRoad(a, c, true, map))
                    {
                        break;
                    }
                }
           // }//*/
        }// for
        /*最初の部屋と最後の部屋をランダムで繋げる*/
        if (Random.Range(0, 2) == 0)
        {
            LayerArea First = areaList[0];
            LayerArea Last = areaList[areaList.Count - 1];
           // CreateRoad(First, Last, map);
        }

    }//connect rooms

    bool CreateRoad(LayerArea roomA, LayerArea roomB, bool isGrandChild, int[,] map)
    {

        if (roomA.outer.areaBottom == roomB.outer.areaTop
            || roomA.outer.areaTop == roomB.outer.areaBottom)
        {
            int x1 = Random.Range(roomA.room.areaLeft, roomA.room.areaRight);
            int x2 = Random.Range(roomB.room.areaLeft, roomB.room.areaRight);
            int y = 0;
            if (isGrandChild)
            {
                if (roomA.HasRoad()) { x1 = roomA.Road.areaLeft; }
                if (roomB.HasRoad()) { x2 = roomB.Road.areaLeft; }
            }

            if (roomA.outer.areaTop > roomB.outer.areaTop)
            {
                // B - A (Bが上側)
                y = roomA.outer.areaTop;
                roomA.CreateRoad(x1, x1 + 1, y + 1, roomA.room.areaTop);
                roomB.CreateRoad(x2, x2 + 1, roomB.room.areaBottom, y);
            }
            else
            {
                //A - B(Aが上側)
                y = roomB.outer.areaTop;
                roomA.CreateRoad(x1, x1 + 1, roomA.room.areaBottom, y);
                roomB.CreateRoad(x2, x2 + 1, y + 1, roomB.room.areaTop);
            }

            /*mapに書き込み*/
            //部屋Aの通路
            for (int ray = roomA.Road.areaTop; ray < roomA.Road.areaBottom; ray++)
            {
                for (int rax = roomA.Road.areaLeft; rax < roomA.Road.areaRight; rax++)
                {
                    MapWriteCheck(ray, rax, map);
                }
            }
            //部屋Bの通路
            for (int rby = roomB.Road.areaTop; rby < roomB.Road.areaBottom; rby++)
            {
                for (int rbx = roomB.Road.areaLeft; rbx < roomB.Road.areaRight; rbx++)
                {
                    MapWriteCheck(rby, rbx, map);
                }
            }
            //通路同士を繋げる
            //左右が逆になっていないかチェック
            if (x1 > x2)
            {
                int x3 = x1;
                x1 = x2;
                x2 = x3;
            }
            //通路同士の書き込み
            for (int rcy = y; rcy < y + 1; rcy++)
            {
                for (int rcx = x1; rcx < x2 + 1; rcx++)
                {
                    MapWriteCheck(rcy, rcx, map);
                }
            }
            return true;
        }
        if (roomA.outer.areaLeft == roomB.outer.areaRight
         || roomA.outer.areaRight == roomB.outer.areaLeft)
        {
            int y1 = Random.Range(roomA.room.areaTop, roomA.room.areaBottom);
            int y2 = Random.Range(roomB.room.areaTop, roomB.room.areaBottom);
            int x = 0;
            if (isGrandChild)
            {
                if (roomA.HasRoad()) { y1 = roomA.Road.areaTop; }
                if (roomB.HasRoad()) { y2 = roomB.Road.areaTop; }
            }
            if (roomA.outer.areaLeft > roomB.outer.areaLeft)
            {
                // B - A(Bが左側)
                x = roomA.outer.areaLeft;
                roomB.CreateRoad(roomB.room.areaRight, x, y2, y2 + 1);
                roomA.CreateRoad(x + 1, roomA.room.areaLeft, y1, y1 + 1);
            }
            else
            {
                //A - B (Aが左側)
                x = roomB.outer.areaLeft;
                roomA.CreateRoad(roomA.room.areaRight, x, y1, y1 + 1);
                roomB.CreateRoad(x, roomB.room.areaLeft, y2, y2 + 1);
            }

            /*mapに書き込み*/
            //部屋Aの通路
            for (int ray = roomA.Road.areaTop; ray < roomA.Road.areaBottom; ray++)
            {
                for (int rax = roomA.Road.areaLeft; rax < roomA.Road.areaRight; rax++)
                {
                    MapWriteCheck(ray, rax, map);
                }
            }
            //部屋Bの通路
            for (int rby = roomB.Road.areaTop; rby < roomB.Road.areaBottom; rby++)
            {
                for (int rbx = roomB.Road.areaLeft; rbx < roomB.Road.areaRight; rbx++)
                {
                    MapWriteCheck(rby, rbx, map);
                }
            }
            //通路同士を繋げる
            //左右が逆になっていないかチェック
            if (y1 > y2)
            {
                int y3 = y1;
                y1 = y2;
                y2 = y3;
            }
            //通路同士の書き込み
            for (int rcy = y1; rcy < y2 + 1; rcy++)
            {
                for (int rcx = x; rcx < x + 1; rcx++)
                {
                    MapWriteCheck(rcy, rcx, map);
                }
            }
            return true;
        }
        return false;
    }// CreateRoad

    //最初の部屋と最後の部屋を区画を無視して繋げる
    void CreateRoad(LayerArea roomA, LayerArea roomB, int[,] map)
    {
        int startX = 0;
        int endX = 0;
        int startY = 0;
        int endY = 0;

        if (roomA.room.areaLeft > roomB.room.areaLeft)
        {
            startX = roomB.room.areaRight;
            endX = roomA.room.areaLeft;
            if (roomA.room.areaTop > roomB.room.areaTop)
            {
                startY = roomB.room.areaTop;
                endY = roomA.room.areaBottom;
            }
            else
            {
                startY = roomA.room.areaTop;
                endY = roomB.room.areaBottom;
            }
        }
        else
        {
            startX = roomA.room.areaRight;
            endX = roomB.room.areaLeft;
            if (roomA.room.areaTop > roomB.room.areaTop)
            {
                startY = roomB.room.areaTop;
                endY = roomA.room.areaBottom;
            }
            else
            {
                startY = roomA.room.areaTop;
                endY = roomB.room.areaBottom;
            }
        }

        for(int ry = startY;ry<endY+1;ry++)
        {
            for(int rx = startX;rx < endX+1;rx++)
            {
                map[ry, rx] = (int)MAP_CHIP.ROAD;
            }
        }

    }//CreateRoad

    void MapWriteCheck(int y,int x,int[,]map)
    {
        //通路が部屋を貫通していなければ
        if(map[y, x]!= (int)MAP_CHIP.ROOM)
        {
            map[y, x] = (int)MAP_CHIP.ROAD;
        }

    }
}
