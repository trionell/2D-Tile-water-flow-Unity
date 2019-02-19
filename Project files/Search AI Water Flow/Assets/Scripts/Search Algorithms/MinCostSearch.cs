using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinCostSearch{

    private class IndexWithCost
    {
        public Index index;
        public int cost;
        public bool isStartNode = false;

        public IndexWithCost(Index index, int cost)
        {
            this.index = index;
            this.cost = cost;
        }
    }

    private Index startIndex;
    private int maxHeight;

    private List<IndexWithCost> openList = new List<IndexWithCost>();
    private List<IndexWithCost> closedList = new List<IndexWithCost>();

    private bool readyToSearch = false;
    private IndexWithCost currentNode;
    private bool searching = false;
    public bool mSearching { get { return searching; } }
    public int speed;

    //--- Constructor ---//
    public MinCostSearch(Index startIndex, out bool result)
    {
        if (!searching)
        {
            if (startIndex != null)
            {
                this.startIndex = startIndex;
                maxHeight = this.startIndex.tile.height;
                openList.Add(new IndexWithCost(this.startIndex, 0));
                openList[0].isStartNode = true;
                openList[0].index.tile.tileGameObject.GetComponent<SpriteRenderer>().color = Color.green;
                readyToSearch = true;
                result = true;
            }
            else
            {
                Debug.LogError("Cannot send a null object as start index");
                result = false;
            }
        }
        else
            result = false;
    }

    /// <summary>
    /// Sets a new start node and initializes the search
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public bool SelectNewStartNode(Index startIndex)
    {
        if (!searching)
        {
            if (startIndex != null)
            {
                this.startIndex = startIndex;
                maxHeight = this.startIndex.tile.height;

                // Reset lists before new search
                openList = new List<IndexWithCost>();
                closedList = new List<IndexWithCost>();

                openList.Add(new IndexWithCost(this.startIndex, 0));
                openList[0].isStartNode = true;
                openList[0].index.tile.tileGameObject.GetComponent<SpriteRenderer>().color = Color.green;
                readyToSearch = true;
                return true;
            }
            else
            {
                Debug.LogError("Cannot send a null object as start index");
                return false;
            }
        }
        else
            return false;
    }

    public void Reset()
    {
        readyToSearch = false;
    }

    /// <summary>
    /// Search loop
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator SearchLoop(MasterBehavior.voidDelegate callback)
    {
        searching = true;
        while (openList.Count > 0 && readyToSearch)
        {
            for (int i = 0; i < speed; i++) // Sets the speed. speed = how many itterations per frame (visualization only)
            {
                if (openList.Count > 0) // if the openlist is empty but the for loop still got itterations left, do nothing
                {
                    currentNode = openList[0];
                    openList.RemoveAt(0);

                    #region Check Neighbors and add them to openList if they exist

                    // Check north tile neighbor. If neighbor height is lower than max height, add it to openList
                    if (TileMapBehavior.instance.NodeValid(currentNode.index.north))
                    {
                        Index index = TileMapBehavior.instance.GetIndex(currentNode.index.north);
                        if (index.tile.height < maxHeight) // Water can not flow higher than the origin
                        {
                            int cost = currentNode.cost + currentNode.index.northSlope;
                            InsertNodeToOpenList(new IndexWithCost(index, cost));
                        }
                    }
                    // Check south tile neighbor. If neighbor height is lower than max height, add it to openList
                    if (TileMapBehavior.instance.NodeValid(currentNode.index.south))
                    {
                        Index index = TileMapBehavior.instance.GetIndex(currentNode.index.south);
                        if (index.tile.height < maxHeight) // Water can not flow higher than the origin
                        {
                            int cost = currentNode.cost + currentNode.index.southSlope;
                            InsertNodeToOpenList(new IndexWithCost(index, cost));
                        }
                    }
                    // Check west tile neighbor. If neighbor height is lower than max height, add it to openList
                    if (TileMapBehavior.instance.NodeValid(currentNode.index.west))
                    {
                        Index index = TileMapBehavior.instance.GetIndex(currentNode.index.west);
                        if (index.tile.height < maxHeight) // Water can not flow higher than the origin
                        {
                            int cost = currentNode.cost + currentNode.index.westSlope;
                            InsertNodeToOpenList(new IndexWithCost(index, cost));
                        }
                    }
                    // Check east tile neighbor. If neighbor height is lower than max height, add it to openList
                    if (TileMapBehavior.instance.NodeValid(currentNode.index.east))
                    {
                        Index index = TileMapBehavior.instance.GetIndex(currentNode.index.east);
                        if (index.tile.height < maxHeight) // Water can not flow higher than the origin
                        {
                            int cost = currentNode.cost + currentNode.index.eastSlope;
                            InsertNodeToOpenList(new IndexWithCost(index, cost));
                        }
                    }
                    #endregion

                    closedList.Add(currentNode); // Add current list to closed list

                    // Set all visited nodes to blue, except the start node
                    if (!currentNode.isStartNode)
                        currentNode.index.tile.tileGameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
            yield return null;
        }

        //yield return null;
        callback();
        searching = false;
    }

    /// <summary>
    /// Insert node to openList based on cost
    /// </summary>
    /// <param name="node"></param>
    private void InsertNodeToOpenList(IndexWithCost node)
    {
        node.index.tile.tileGameObject.GetComponent<SpriteRenderer>().color = Color.cyan; // Set tile color tint to cyan

        node.index.visited = true;
        for (int i = 0; i < openList.Count; i++)
        {
            if(openList[i].cost > node.cost)
            {
                openList.Insert(i, node);
                return;
            }
        }
        openList.Add(node);
    }
}
