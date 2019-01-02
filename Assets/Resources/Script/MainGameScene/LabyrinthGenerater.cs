using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LabyrinthGenerater : MonoBehaviour {

    [SerializeField]
    LabyrinthSettings labyrinthSettings;
    [Header(""), Range(4, 100)]
    public int width;
    [Header(""), Range(4, 100)]
    public int height;

    [SerializeField]
    public int[,] GenerateMap()
    {
        int[,] mapData = new int[width, height];
        LabyrinthArea baseArea = new LabyrinthArea(0, 0, width, height, labyrinthSettings);
        var splitArea = baseArea.Split();

        foreach(var area in splitArea)
        {
            mapData = area.WriteToMap(mapData);
        }
        var road = GenerateRoadArea(splitArea);
        foreach(var roadArea in road)
        {
            mapData = roadArea.WriteMap(mapData);
        }
        return mapData;
    }

    /// <summary>
    /// 道の生成
    /// </summary>
    /// <returns>The road area.</returns>
    /// <param name="area">Area.</param>
    Road[] GenerateRoadArea(LabyrinthArea[] area)
    {
        List<Road> road = new List<Road>();
        foreach(var roadA in area)
        {
            foreach(var roadB in area)
            {
                if(roadA == roadB || !IsAdjacently(roadA,roadB))
                { 
                    continue; 
                }
                road.Add(new Road(roadA, roadB));
            }
        }
        //不要
        List<Road> constantRoad = new List<Road>();
        while (road.Count > 0)
        {
            int targetIndex = UnityEngine.Random.Range(0, road.Count);
            Road targetRoad = road[targetIndex];
            road.RemoveAt(targetIndex);

            if (!IsConnectArea(area.ToList(), road.ToArray(), constantRoad.ToArray()))
            {
                constantRoad.Add(targetRoad);
            }

        }
        return constantRoad.ToArray();
    }

    bool IsAdjacently(LabyrinthArea areaA,LabyrinthArea areaB)
    {
        var left = areaA.x < areaB.x ? areaA : areaB;
        var right = areaA.x > areaB.x ? areaA : areaB;
        var top = areaA.y < areaB.y ? areaA : areaB;
        var bottom = areaA.y < areaB.y ? areaA : areaB;

        if(left != null && right != null
            && (left.x+left.width)==right.x
            && (left.y <= right.y && right.y < (left.y + left.height) || right.y <= left.y && left.y <(right.y+right.height)))
            {
                return true;
            }

        if (top != null && bottom != null
            && (bottom.y + bottom.height) == top.y
            && (bottom.x <= top.x && top.x < (bottom.x + bottom.width) || top.x <= bottom.x && bottom.x < (top.x + top.width)))
            {
                return true;
            }
        return false;
    }

    bool IsConnectArea(List<LabyrinthArea> area, Road[] roadA, Road[] roadB)
    {
        if(area.Count<=1)
        {
            return true;
        }

        List<Road> road = new List<Road>();
        road.AddRange(roadA);
        road.AddRange(roadB);

        List<LabyrinthArea> checkArea = new List<LabyrinthArea>() { area[0] };
        area.RemoveAt(0);
        List<LabyrinthArea> checkedArea = new List<LabyrinthArea>() { };

        while(checkArea.Count>0)
        {
            List<LabyrinthArea> nextArea = new List<LabyrinthArea>() { };
            foreach(var checkTarget in checkArea)
            {
                foreach(var roadMap in road.Where(x=>x.labyArea.Contains(checkTarget)))
                {
                    var pairedArea = roadMap.labyArea.First(x => x != checkTarget);
                    if(!checkedArea.Contains(pairedArea) && !checkArea.Contains(pairedArea) && !nextArea.Contains(pairedArea))
                    {
                        area.Remove(pairedArea);
                        nextArea.Add(pairedArea);
                    }
                }
            }
            checkedArea.AddRange(checkArea);
            checkArea = nextArea;
        }
        return area.Count == 0;
    }

}
