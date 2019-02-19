using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile{

    public int height;
    public GameObject tileGameObject;

    //--- Constructors ---//
    public Tile(GameObject gameObject)
    {
        height = 0;
        tileGameObject = gameObject;
    }

    public Tile(int height, GameObject gameObject)
    {
        this.height = height;
        tileGameObject = gameObject;
    }

    // Set color of tile based on height assosiated with tile
    public void RepresentHeight()
    {
        SpriteRenderer spriteRenderer = tileGameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Texture2D gradient = Resources.Load("BlackWhiteGradient") as Texture2D;
            spriteRenderer.color = gradient.GetPixel(height, 1);
        }
        else
            Debug.LogError("Attached gameObject to tile does not have a sprite renderer");
    }

}
