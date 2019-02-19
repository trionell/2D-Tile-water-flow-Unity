using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MasterBehavior : MonoBehaviour {

    public Sprite map1;
    public Sprite map2;
    public Sprite map3;
    public Sprite customMap;

    public GameObject editMapButton;

    public List<Button> buttonsToDisableOnSearch = new List<Button>();
    public Camera mainCamera;

    private MinCostSearch minCostSearch;
    private int speed = 1;

    // Callback to know when search is done
    public delegate void voidDelegate();
    public voidDelegate searchDoneDelegate;

    // Singelton
    public static MasterBehavior instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            searchDoneDelegate = DoneSearching;
            DisableButtons();
            editMapButton.SetActive(false);
        }
    }

    // Selects a start node if there is no search running
    public void SelectStartTile(Index index)
    {
        if (minCostSearch == null)
        {
            bool indexExists;
            minCostSearch = new MinCostSearch(index, out indexExists);
            minCostSearch.speed = speed;

            // If the start node is valid
            if (indexExists)
            {
                EnableButtons();
            }
        }            
        // If not searching, reset map and select new start node
        else if(!minCostSearch.mSearching)
        {
            ResetTileMap();
            if (minCostSearch.SelectNewStartNode(index))
            {
                EnableButtons();
            }
        }
    }

    /// <summary>
    /// Start searching if ready
    /// </summary>
    public void StartSearch()
    {
        if (minCostSearch != null)
        {
            StartCoroutine(minCostSearch.SearchLoop(searchDoneDelegate));
            DisableButtons();
        }
    }

    /// <summary>
    /// Reset map
    /// </summary>
    public void ResetTileMap()
    {
        TileMapBehavior.instance.ResetTilemap();
        if (minCostSearch != null)
            minCostSearch.Reset();
        DisableButtons();
    }

    /// <summary>
    /// Change to another map and resets the search
    /// </summary>
    /// <param name="option"></param>
    public void ChangeMap(Dropdown option)
    {
        switch (option.value)
        {
            case 0:
                TileMapBehavior.instance.ChangeHightMap(map1);
                if (minCostSearch != null)
                    minCostSearch.Reset();
                HideEditMapButton();
                break;

            case 1:
                TileMapBehavior.instance.ChangeHightMap(map2);
                if (minCostSearch != null)
                    minCostSearch.Reset();
                HideEditMapButton();
                break;

            case 2:
                TileMapBehavior.instance.ChangeHightMap(map3);
                if (minCostSearch != null)
                    minCostSearch.Reset();
                HideEditMapButton();
                break;

            case 3:
                TileMapBehavior.instance.ChangeHightMap(customMap);
                if (minCostSearch != null)
                    minCostSearch.Reset();
                ShowEditMapButton();
                break;

            default:
                Debug.Log("Option not available");
                break;
        }
    }

    /// <summary>
    /// Invert the height map
    /// </summary>
    /// <param name="option"></param>
    public void InvertMap(Toggle option)
    {
        TileMapBehavior.instance.invertHeightMap = option.isOn;
        TileMapBehavior.instance.ReCalculateHeightMap();
    }

    /// <summary>
    /// Set search speed
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        if (minCostSearch != null)
            minCostSearch.speed = (int)speed;
        this.speed = (int)speed;
    }

    /// <summary>
    /// Called from the search loop when search is done. Not used atm
    /// </summary>
    public void DoneSearching()
    {
        //EnableButtons();
    }

    /// <summary>
    /// Enables all buttons in list
    /// </summary>
    private void EnableButtons()
    {
        foreach (Button button in buttonsToDisableOnSearch)
        {
            button.interactable = true;
        }
    }

    /// <summary>
    /// Disables all buttons in list
    /// </summary>
    private void DisableButtons()
    {
        foreach (Button button in buttonsToDisableOnSearch)
        {
            button.interactable = false;
        }
    }

    /// <summary>
    /// Set edit button to active
    /// </summary>
    private void ShowEditMapButton()
    {
        editMapButton.SetActive(true);
    }

    /// <summary>
    /// Set edit button to deactive
    /// </summary>
    private void HideEditMapButton()
    {
        editMapButton.SetActive(false);
    }

    /// <summary>
    /// Opens the map drawer scene
    /// </summary>
    public void OpenMapEditor()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        mainCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets heightmap to the custom one/ reloads the custom heightmap
    /// </summary>
    public void DoneEditingMap()
    {
        TileMapBehavior.instance.ChangeHightMap(customMap);
        if (minCostSearch != null)
            minCostSearch.Reset();

        mainCamera.gameObject.SetActive(true);
    }
}
