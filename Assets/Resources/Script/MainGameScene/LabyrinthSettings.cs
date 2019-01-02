using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョン生成用設定クラス
/// </summary>
[SerializeField]
public class LabyrinthSettings : MonoBehaviour {

    [Header(""),Range(2, 10)]
    public int minWidth;
    [Header(""), Range(2, 10)]
    public int minHeight;
    [Header(""), Range(0, 100)]
    public int roomRate;
    [Header(""), Range(1, 10)]
    public int maxWallThickness;
}
