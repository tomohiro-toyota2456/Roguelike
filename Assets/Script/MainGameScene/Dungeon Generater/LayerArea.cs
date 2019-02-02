using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区画クラス
/// ダンジョン全体と同じ広さを設定
/// </summary>
public class LayerArea  {


    /// <summary>
    /// 外周
    /// </summary>
    public Area outer;
    /// <summary>
    /// 部屋情報
    /// </summary>
    public Area room;
    /// <summary>
    /// 通路情報
    /// </summary>
    public Area Road;

    /// <summary>
    /// マップ作成コンストラクタ
    /// </summary>
    public LayerArea()
    {
        outer = new Area();
        room = new Area();
        Road = null;
    }
    public void CreateRoad(int left, int right, int top, int bottom)
    {
        Road = new Area();
        Road.Set(left, right, top, bottom);
    }
    public bool HasRoad()
    {
        return Road != null;
    }

    public class Area
    {
        public int areaLeft = 0;
        public int areaRight = 0;
        public int areaTop = 0;
        public int areaBottom = 0;

        /// <summary>
        /// 値のセット
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <param name="top">Top.</param>
        /// <param name="bottom">Bottom.</param>
        public void Set(int left,int right,int top,int bottom)
        {
            areaLeft = left;
            areaRight = right;
            areaTop = top;
            areaBottom = bottom;
        }

        public int Width { get { return areaRight - areaLeft; } }
        public int Hight { get { return areaBottom - areaTop; } }
        /// <summary>
        /// 面積
        /// </summary>
        /// <value>The extent.</value>
        public int Extent { get { return Width * Hight; } }

    }//Area class
}// LayerArea class
