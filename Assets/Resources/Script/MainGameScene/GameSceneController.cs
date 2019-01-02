using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour {
    [SerializeField]
    LabyrinthGenerater labyrinth;
    [SerializeField]
    Transform tileContainer;

    [Header("一時的 壁オブジェクト"), SerializeField]
    GameObject Wall;
    [Header("一時的 床オブジェクト"), SerializeField]
    GameObject Mat;

	// Use this for initialization
	void Start () {
        Generate();
    }
	
    void Generate()
    {
        foreach(Transform child in tileContainer.transform)
        {
            Destroy(child.gameObject);
        }

        Wall.gameObject.SetActive(true);
        Mat.gameObject.SetActive(true);

        var map = labyrinth.GenerateMap();

        for(var x = 0;x<labyrinth.width;x++)
        {
            for(var y = 0;y<labyrinth.height;y++)
            {
                var tile = map[x, y] == 1 ? Instantiate(Mat) : Instantiate(Wall);
                tile.transform.SetParent(tileContainer);
                tile.transform.localPosition = new Vector2(x, y);
            }
        }
        Wall.gameObject.SetActive(false);
        Mat.gameObject.SetActive(false);
    }
}
