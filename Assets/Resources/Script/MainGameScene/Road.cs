using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Road {
    public readonly LabyrinthArea[] labyArea;

    public Road(LabyrinthArea AreaA, LabyrinthArea AreaB)
    {
        this.labyArea = new LabyrinthArea[] { AreaA, AreaB };
    }

    public int[,] WriteMap(int[,] map)
    {
        int fromX = UnityEngine.Random.Range(labyArea[0].room.x, labyArea[0].room.x + labyArea[0].room.width);
        int fromY = UnityEngine.Random.Range(labyArea[0].room.y, labyArea[0].room.y + labyArea[0].room.height);
        int toX = UnityEngine.Random.Range(labyArea[1].room.x, labyArea[1].room.x + labyArea[1].room.width);
        int toY = UnityEngine.Random.Range(labyArea[1].room.y, labyArea[1].room.y + labyArea[1].room.height);

        while(fromX != toX || fromY != toY)
        {
            map[fromX, fromY] = 1;
            if(fromX != toX && fromY != toY
               && UnityEngine.Random.Range(0,2) == 0||fromY == toY)
            {
                fromX += (toX - fromX) > 0 ? 1 : -1;
            }
            else
            {
                fromY += (toY - fromY) > 0 ? 1 : -1;
            }

        }
        return map;
    }
}
