using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapBehavior : MonoBehaviour {

    [Range(5, 64)] public int mapSize = 48; // basically irrelevant
    [SerializeField] private Sprite defaultHeightMap;

    [HideInInspector] public Sprite heightMap;
    public bool invertHeightMap = false;

    [SerializeField] private GameObject tilePrefab;

    private TileMatrix tileMatrix;

    // Singelton
    public static TileMapBehavior instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;

            // Generate new tilemap
            tileMatrix = new TileMatrix(mapSize);
            heightMap = defaultHeightMap;
            GenerateMap();
        }
    }

    /// <summary>
    /// Generate tile map
    /// </summary>
    private void GenerateMap()
    {
        if(heightMap != null)
        {
            if(heightMap.texture.width >= mapSize && heightMap.texture.height >= mapSize)
            {
                GameObject go;
                
                // Generate map loop
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        go = null;
                        go = Instantiate(tilePrefab, transform.position + new Vector3(i, j, 0), Quaternion.identity);
                        go.transform.parent = transform;
                        tileMatrix.Insert(new Index(new Tile(GetHeightFromMap(i, j), go), i, j), i, j);
                    }
                }
                tileMatrix.CalculateSlopes();
                tileMatrix.DisplayHeight();
            }
            else
            {
                Debug.LogError("Heightmap smaller than map size. Change map size to match or smaller than heightmap resolution or change to higher resolution heightmap");
            }
        }
        else
        {
            Debug.LogError("Missing heightmap. Please asign a heightmap");
        }
    }

    /// <summary>
    /// Get a value from 0 to 100 from the heightmap
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    private int GetHeightFromMap(int i, int j)
    {
        if(invertHeightMap)
            return (int)(heightMap.texture.GetPixel(i, j).b * 100);
        else
            return (100 - (int)(heightMap.texture.GetPixel(i, j).b * 100));
    }

    /// <summary>
    /// Recalculates slopes and reseting when new map is loaded
    /// </summary>
    /// <param name="heightMap"></param>
    public void ChangeHightMap(Sprite heightMap)
    {
        this.heightMap = heightMap; // Set new heightmap

        // Itterate over all tiles and set each tile height to new height from new heightmap
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tileMatrix.GetIndex(i, j).tile.height = GetHeightFromMap(i, j);
            }
        }
        ResetTilemap();
        tileMatrix.CalculateSlopes();
    }

    /// <summary>
    /// sets new heights and recalculates Slopes. Used when map should invert
    /// </summary>
    public void ReCalculateHeightMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tileMatrix.GetIndex(i, j).tile.height = GetHeightFromMap(i, j);
            }
        }
        ResetTilemap();
        tileMatrix.CalculateSlopes();
    }

    /// <summary>
    ///  Only resets visual and state
    /// </summary>
    public void ResetTilemap()
    {
        tileMatrix.ResetVisited();
        tileMatrix.DisplayHeight();
    }
    
    /// <summary>
    /// Get tile index from coords
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public Index GetIndex(int i, int j)
    {
        return tileMatrix.GetIndex(i, j);
    }

    /// <summary>
    /// Get tile index from coords
    /// </summary>
    public Index GetIndex(Vector2 coord)
    {
        return tileMatrix.GetIndex((int)coord.x, (int)coord.y);
    }

    /// <summary>
    ///  Check if node is inside the map
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public bool NodeValid(int i, int j)
    {
        if (i >= 0 && i < mapSize && j >= 0 && j < mapSize)
        {
            if (GetIndex(i, j).visited)
                return false;
            else
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  Check if node is inside the map
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    public bool NodeValid(Vector2 coord)
    {
        if (coord.x >= 0 && coord.x < mapSize && coord.y >= 0 && coord.y < mapSize)
        {
            if (GetIndex(coord).visited)
                return false;
            else
            {
                return true;
            }
        }
        return false;
    }

}
