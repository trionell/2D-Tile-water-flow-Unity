using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMatrix{

    public int mapSize;
    private Index[,] tiles;

    //--- Constructor ---//
    public TileMatrix(int mapSize)
    {
        this.mapSize = mapSize;
        tiles = new Index[mapSize, mapSize];
    }

    //--- Insert index to matrix ---//
    public void Insert(Index index, int i, int j)
    {
        // If index coords is not == to i & j, create a new index with i & j as coords
        if (index.i != i || index.j != j)
            index = new Index(index.tile, i, j);
        tiles[i, j] = index;
    }

    /// <summary>
    /// Get index by coords
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public Index GetIndex(int i, int j)
    {
        return tiles[i, j];
    }

    /// <summary>
    /// Calculate the slopes rellative to each tile
    /// </summary>
    public void CalculateSlopes()
    {
        Index north = null, south = null, west = null, east = null;

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                #region Assign neighbors if valid
                // North
                if (!(i+1 >= 0 && i+1 < mapSize && j >= 0 && j < mapSize))
                    east = null;
                else
                    east = tiles[i + 1, j];

                // south
                if (!(i-1 >= 0 && i-1 < mapSize && j >= 0 && j < mapSize))
                    west = null;
                else
                    west = tiles[i - 1, j];

                // west
                if (!(i >= 0 && i < mapSize && j-1 >= 0 && j-1 < mapSize))
                    south = null;
                else
                    south = tiles[i, j - 1];

                // east
                if (!(i >= 0 && i < mapSize && j+1 >= 0 && j+1 < mapSize))
                    north = null;
                else
                    north = tiles[i, j + 1];
                #endregion

                tiles[i, j].GenerateSlopes(north, south, west, east);
            }
        }
    }

    /// <summary>
    /// Set each tile color to a color based on its height. Black is heighest, white is lowest
    /// </summary>
    public void DisplayHeight()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tiles[i, j].tile.RepresentHeight();
            }
        }
    }

    /// <summary>
    /// Sets all tiles to not visited
    /// </summary>
    public void ResetVisited()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tiles[i, j].visited = false;
            }
        }
    }
}
