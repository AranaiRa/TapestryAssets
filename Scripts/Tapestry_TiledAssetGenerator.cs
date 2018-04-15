using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_TiledAssetGenerator : MonoBehaviour {

    public GameObject tile;
    public Vector2Int arraySize;
    public Vector2 tileSize;
    public bool 
        randomlyRotateTiles = true,
        rotateOnX, rotateOnY = true, rotateOnZ,
        outline;
    public Tapestry_TileGeneratorModes mode;

    [SerializeField]
    private List<GameObject> tiles;

    private void Reset()
    {
        tiles = new List<GameObject>();
    }

    void Start () {
        Destroy(this);
	}

    public void Generate()
    {
        Vector2 offset = GetOffset();

        for (int x = 0; x < arraySize.x; x++) 
        {
            for (int y = 0; y < arraySize.y; y++) 
            {
                if(outline)
                {
                    if (!(x == arraySize.x - 1 || x == 0 || y == arraySize.y - 1 || y == 0))
                            continue;
                }

                GameObject go = Instantiate(tile);
                go.name = tile.name;
                go.transform.SetParent(this.transform);
                go.transform.localPosition = new Vector3(x * offset.x, 0, y * offset.y);
                if(randomlyRotateTiles)
                {
                    GameObject fix = new GameObject();
                    fix.transform.SetParent(go.transform);
                    fix.transform.localPosition = Vector3.zero;
                    fix.transform.SetParent(this.transform);
                    fix.transform.localRotation = Quaternion.identity;
                    go.transform.SetParent(fix.transform);
                    if (rotateOnX)
                    {
                        int rot = Random.Range(0, 4) * 90;
                        fix.transform.localRotation = Quaternion.Euler(rot, 0, 0);
                    }
                    if (rotateOnY)
                    {
                        int rot = Random.Range(0, 4) * 90;
                        fix.transform.localRotation = Quaternion.Euler(0, rot, 0);
                    }
                    if (rotateOnZ)
                    {
                        int rot = Random.Range(0, 4) * 90;
                        fix.transform.localRotation = Quaternion.Euler(0, 0, rot);
                    }
                    go.transform.SetParent(this.transform);
                    DestroyImmediate(fix);
                }

                tiles.Add(go);
            }
        }
    }

    public void Clear()
    {
        if (tiles == null)
            tiles = new List<GameObject>();
        else
        {
            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(tiles[i]);
            }
            tiles = new List<GameObject>();
        }
    }

    private Vector2 GetOffset()
    {
        Vector2 offset = new Vector2();
        if(mode == Tapestry_TileGeneratorModes.XMinusZMinus)
        {
            offset.x = -tileSize.x;
            offset.y = -tileSize.y;
        }
        else if (mode == Tapestry_TileGeneratorModes.XPlusZMinus)
        {
            offset.x = tileSize.x;
            offset.y = -tileSize.y;
        }
        else if (mode == Tapestry_TileGeneratorModes.XMinusZPlus)
        {
            offset.x = -tileSize.x;
            offset.y = tileSize.y;
        }
        else if (mode == Tapestry_TileGeneratorModes.XPlusZPlus)
        {
            offset.x = tileSize.x;
            offset.y = tileSize.y;
        }
        return offset;
    }
}

public enum Tapestry_TileGeneratorModes
{
    XPlusZPlus, XPlusZMinus, XMinusZPlus, XMinusZMinus
}