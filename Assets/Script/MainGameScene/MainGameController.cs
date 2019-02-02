using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameController : MonoBehaviour {

    [SerializeField]
    private GameObject wall = null;
    [SerializeField]
    private GameObject wall_Down = null;
    [SerializeField]
    private GameObject road = null;
    [SerializeField]
    private Transform TileContainer = null;

    [SerializeField]
    private DungeonGenerator generator = null;

    // Use this for initialization
    void Start () {
		foreach(Transform child in TileContainer.transform)
        {
            Destroy(child.gameObject);
        }

        int[,] mapData = generator.Generater();

        for(int y=0;y<100;y++)
        {
            for(int x=0;x<100;x++)
            {
                var tileChip = mapData[y, x];
                GameObject chip;
                float width = 0;
                switch((DungeonGenerator.MAP_CHIP)tileChip)
                {
                    case DungeonGenerator.MAP_CHIP.WALL:
                        chip = Instantiate(wall);
                        chip.transform.SetParent(TileContainer);
                        width = chip.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                        chip.transform.localPosition = new Vector2(width * x, y);
                        break;
                    case DungeonGenerator.MAP_CHIP.WALLDOWN:
                        chip = Instantiate(wall_Down);
                        chip.transform.SetParent(TileContainer);
                        width = chip.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                        chip.transform.localPosition = new Vector2(width * x, y);
                        break;
                    case DungeonGenerator.MAP_CHIP.ROOM:
                        chip = Instantiate(road);
                        chip.transform.SetParent(TileContainer);
                        width = chip.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                        chip.transform.localPosition = new Vector2(width * x, y);
                        break;
                    case DungeonGenerator.MAP_CHIP.ROAD:
                        chip = Instantiate(road);
                        chip.transform.SetParent(TileContainer);
                        width = chip.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                        chip.transform.localPosition = new Vector2(width * x, y);
                        break;
                    case DungeonGenerator.MAP_CHIP.NO_DESTORY_WALL:
                        chip = Instantiate(wall);
                        chip.transform.SetParent(TileContainer);
                        width = chip.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                        chip.transform.localPosition = new Vector2(width * x, y);
                        break;
                }
            }
        }//for
        wall.gameObject.SetActive(false);
        road.gameObject.SetActive(false);
        wall_Down.SetActive(false);
    }
	
}
