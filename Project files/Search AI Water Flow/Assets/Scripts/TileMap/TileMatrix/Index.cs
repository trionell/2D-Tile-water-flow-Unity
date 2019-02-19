using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Index {

    public int i { get; private set; }
    public int j { get; private set; }
    public Tile tile { get; private set; }

    public bool visited = false;

    // Neighbor nodes index values
    public Vector2 north { get { return new Vector2(i, j + 1); } }
    public Vector2 south { get { return new Vector2(i, j - 1); }}
    public Vector2 west { get { return new Vector2(i - 1, j); } }
    public Vector2 east { get { return new Vector2(i + 1, j); } }

    //--- Slopes ---//
    public int northSlope { get; private set; }
    public int southSlope { get; private set; }
    public int westSlope { get; private set; }
    public int eastSlope { get; private set; }


    //--- Constructors ---//
    public Index(Tile tile)
    {
        j = 0;
        i = 0;
        this.tile = tile;
    }

    public Index(Tile tile, int i, int j)
    {
        this.i = i;
        this.j = j;
        this.tile = tile;
    }

    public Index(Index index)
    {
        this.i = index.i;
        this.j = index.j;
        this.tile = index.tile;
    }

    /// <summary>
    /// Generate slopes
    /// </summary>
    /// <param name="northTile"></param>
    /// <param name="southTile"></param>
    /// <param name="westTile"></param>
    /// <param name="eastTile"></param>
    public void GenerateSlopes(Index northTile, Index southTile, Index westTile, Index eastTile)
    {
        int centerHeight = tile.height;

        if (northTile != null)
            northSlope = northTile.tile.height - centerHeight;

        if (southTile != null)
            southSlope = southTile.tile.height - centerHeight;

        if (westTile != null)
            westSlope = westTile.tile.height - centerHeight;

        if (eastTile != null)
            eastSlope = eastTile.tile.height - centerHeight;
    }
}
