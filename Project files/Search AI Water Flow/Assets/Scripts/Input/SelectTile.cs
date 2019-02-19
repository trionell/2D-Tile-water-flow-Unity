using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectTile : MonoBehaviour {

    private Vector3 point;
    public GameObject UIPanel;

    private void Update()
    {
        // If right click is pressed and pointer is not over UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Translate point from screen to world point and round to integer
            point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), 0f);

            // If node is valid, select it as start node
            if (TileMapBehavior.instance.NodeValid((int)point.x, (int)point.y)) { 
                Index clickedIndex = TileMapBehavior.instance.GetIndex((int)point.x, (int)point.y);
                MasterBehavior.instance.SelectStartTile(clickedIndex);
            }
            else
                Debug.Log("Outside of tilemap. Wont select start node");
        }
    }
}
