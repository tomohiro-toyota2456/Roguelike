using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LabyrinthArea {

    public readonly int x;
    public readonly int y;
    public readonly int width;
    public readonly int height;
    public readonly LabyrinthSettings labySettings;
    public readonly Room room;

    public LabyrinthArea()
    {

    }
    public LabyrinthArea(int x,int y,int width,int heght,LabyrinthSettings settings)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = heght;
        this.labySettings = settings;
        room = Generater();
    }

    /// <summary>
    /// エリア幅
    /// </summary>
    int MinWidth { get { return labySettings.minWidth + 2; } }
    /// <summary>
    /// エリア高さ
    /// </summary>
    int MinHeight { get { return labySettings.minHeight + 2; } }

    /// <summary>
    /// エリアを横に分割できるか
    /// </summary>
    /// <value><c>true</c> if is split horizontal; otherwise, <c>false</c>.</value>
    bool IsSplitHorizontal{get{ return MinWidth * 2 <= width; } }
    /// <summary>
    /// エリアを縦に分割できるか
    /// </summary>
    /// <value><c>true</c> if is split vertucal; otherwise, <c>false</c>.</value>
    bool IsSplitVertucal { get { return MinHeight * 2 <= height; } }
    /// <summary>
    /// そもそも分割できるか
    /// </summary>
    /// <value><c>true</c> if is split area; otherwise, <c>false</c>.</value>
    bool isSplitArea{get { return IsSplitHorizontal || IsSplitVertucal; }}

    /// <summary>
    /// エリア分割
    /// </summary>
    /// <returns>The split.</returns>
    public LabyrinthArea[] Split()
    {
        //分割可能数
        LabyrinthArea[] splittableArea = new LabyrinthArea[] { this };
        //分割数
        List<LabyrinthArea> splitArea = new List<LabyrinthArea>();
        //一定数
        List<LabyrinthArea> constantArea = new List<LabyrinthArea>();

        //分割開始
        while (true)
        {
            constantArea.AddRange(splittableArea.Where(x => !x.isSplitArea));
            if (splittableArea.Length == 0)
            {
                break;
            }
            splitArea.Clear();

            foreach (var area in splittableArea.Where(x => x.isSplitArea))
            {
                if(UnityEngine.Random.Range(0,100)<labySettings.roomRate)
                {
                    constantArea.Add(area);
                }
                else
                {
                    splitArea.AddRange(area.SplitOnce());
                }
            }
            splittableArea = splitArea.ToArray();
        }
        return constantArea.ToArray();
    }

    /// <summary>
    /// マップの書き込み
    /// </summary>
    /// <returns>The to map.</returns>
    /// <param name="map">Map.</param>
    public int[,] WriteToMap(int[,] map)
    {
        for(int ix = room.x;ix<room.x+room.width;ix++)
        {
            for(int iy = room.y;iy<room.y+room.height;iy++)
            {
                map[ix, iy] = 1;
            }
        }
        return map;
    }

    /// <summary>
    /// エリアに部屋を生成
    /// </summary>
    /// <returns>The generater.</returns>
    private Room Generater()
    {
        int left  = UnityEngine.Random.Range(1, Math.Min(1 + labySettings.maxWallThickness, width - labySettings.minWidth));
        int right = UnityEngine.Random.Range(Math.Max(width - labySettings.maxWallThickness, left + labySettings.minWidth),width-1);
        int bottom = UnityEngine.Random.Range(1, Math.Min(1 + labySettings.maxWallThickness, height - labySettings.minHeight));
        int top = UnityEngine.Random.Range(Math.Max(height - labySettings.maxWallThickness, bottom + labySettings.minHeight), height - 1);

        return new Room(x + left, y + bottom, right - left, top - bottom);
    }

    /// <summary>
    /// 一度だけ分割
    /// </summary>
    /// <returns>The once.</returns>
    LabyrinthArea[] SplitOnce()
    {
        if(IsSplitHorizontal && IsSplitVertucal && UnityEngine.Random.Range(0,2)==0
           ||IsSplitHorizontal && !IsSplitVertucal)
        {
            int splitPosX = UnityEngine.Random.Range(x + MinWidth, x + width - MinWidth + 1);
            return new LabyrinthArea[]
            {
                new LabyrinthArea(x,y,splitPosX-x,height,labySettings),
                new LabyrinthArea(splitPosX,y,width-(splitPosX-x),height,labySettings)
            };
        }
        else if(IsSplitVertucal)
        {
            int splitPosY = UnityEngine.Random.Range(y + MinHeight, y + height - MinHeight + 1);
            return new LabyrinthArea[]
            {
                new LabyrinthArea(x,y,width,splitPosY-y,labySettings),
                new LabyrinthArea(x,splitPosY,width,height-(splitPosY-y),labySettings)
            };
        }
        else
        {
            return new LabyrinthArea[] { this };
        }
    }

}




