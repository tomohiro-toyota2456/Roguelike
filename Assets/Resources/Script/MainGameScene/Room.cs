using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    public readonly int x;
    public readonly int y;
    public readonly int width;
    public readonly int height;

    public Room(int x,int y, int width,int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

}
