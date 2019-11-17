using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Prefabs")]
    public Transform tilePrefab;
    public Tank lightTankPrefab;
    public Tank heavyTankPrefab;

    [Header("Dimensions")]
    public int rows = 5;
    public int cols = 5;
    public int heavyTankCount = 2;
    public int lightTankCount = 3;

    [Header("Parents")]
    public Transform tanksParent;


    //PRIVATE MEMBERS
    private List<Transform> tiles;
    private List<Tank> tanks;

    //Boundaries for the tanks and tiles.
    private int minX;
    private int maxX;
    private int minY;
    private int maxY;

    public int playerTurnLeft = 10;

    public int playerNumber = 0;

    /// <summary>
    /// Assigns the events when the gameobject is enabled.
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnTankTouched += OnTankTouched;
        EventManager.OnTurnSwitched += OnTurnSwitched;
    }

    /// <summary>
    /// Places the tiles and Spawn the tanks
    /// Hides all the ships if not the turn of the player.
    /// </summary>
    void Awake()
    {
        PlaceTiles();
        SpawnTanks();

        if (GameManager.instance.playerTurn != playerNumber)
        {
            HideAllShips();
        }
    }

    /// <summary>
    /// Handles the shoot mechanism and movement code.
    /// </summary>
    private void Update()
    {
        if (playerNumber != GameManager.instance.playerTurn || playerTurnLeft <= 0)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int activeTankTileIndex = GetActiveTankTileIndex();
            if (activeTankTileIndex > -1)
            {
                int otherPlayerNumber = (playerNumber == 1) ? 1 : 0;
                Tank selectedTank = GetSelectedTank();
                if (selectedTank.isDead)
                {
                    return;
                }
                selectedTank.transform.position = new Vector3((int)selectedTank.transform.position.x, selectedTank.transform.position.y, (int)selectedTank.transform.position.z);
                Manager otherPlayerManager = GameManager.instance.playerManagers[otherPlayerNumber];
                Transform otherPlayerTile = otherPlayerManager.tiles[activeTankTileIndex];
                Transform tankTile = tiles[activeTankTileIndex];
                Tank targetTank = otherPlayerManager.GetTankAtPosition(otherPlayerTile);
                GetSelectedTank().transform.rotation = Quaternion.LookRotation(otherPlayerTile .position- tankTile.position, Vector3.up);
                FireMissile(tankTile.position, otherPlayerTile.position, targetTank);
                playerTurnLeft--;
            }
        }
    }

    /// <summary>
    /// Deregisters the events on disable.
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnTankTouched -= OnTankTouched;
    }


    /// <summary>
    /// Places all the tiles
    /// </summary>
    public void PlaceTiles()
    {
        minX = -(cols / 2) + (int)transform.position.x;
        minY = -(rows / 2) + (int)transform.position.z;
        maxX = (cols / 2) + (int)transform.position.x;
        maxY = (rows / 2) + (int)transform.position.z;

        if (tiles == null)
            tiles = new List<Transform>();


        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                Transform tile = Instantiate(tilePrefab, transform);
                Vector3 tilePos = new Vector3(i, tile.position.y, j);
                tile.position = tilePos;
                tiles.Add(tile);
            }
        }
    }

    /// <summary>
    /// Spawns all the heavy and light tanks
    /// </summary>
    public void SpawnTanks()
    {
        List<Transform> tempTiles = new List<Transform>(tiles);
        Debug.Log(tempTiles.Count);
        if (tanks == null)
            tanks = new List<Tank>();
        for (int i = 0; i < heavyTankCount; i++)
        {
            Tank tank = Instantiate(heavyTankPrefab, tanksParent);
            int randomTileIndex = Random.Range(0, tempTiles.Count);
            Transform randomTile = tiles[randomTileIndex];
            Vector3 tankPos = new Vector3(randomTile.position.x, tank.transform.position.y, randomTile.position.z);
            tank.transform.position = tankPos;
            tank.SetBounds(minX, maxX, minY, maxY);
            tanks.Add(tank);
            tempTiles.RemoveAt(randomTileIndex);
        }

        for (int i = 0; i < lightTankCount; i++)
        {
            Tank tank = Instantiate(lightTankPrefab, tanksParent);
            int randomTileIndex = Random.Range(0, tempTiles.Count);
            Transform randomTile = tiles[randomTileIndex];
            Vector3 tankPos = new Vector3(randomTile.position.x, tank.transform.position.y, randomTile.position.z);
            tank.transform.position = tankPos;
            tank.SetBounds(minX, maxX, minY, maxY);
            tanks.Add(tank);
            tempTiles.RemoveAt(randomTileIndex);
        }
    }

    /// <summary>
    /// This method is called when the tanktouched event is called.
    /// </summary>
    /// <param name="tank">Tank touched</param>
    private void OnTankTouched(Tank tank)
    {
        if (GameManager.instance.playerTurn != playerNumber)
            return;

        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks[i] != tank)
            {
                tanks[i].Deactivate();
            }
            else
            {
                tank.Activate();
            }
        }
    }

    /// <summary>
    /// On player turn change
    /// </summary>
    /// <param name="turn">Player number.</param>
    private void OnTurnSwitched(int turn)
    {

        if (turn != playerNumber)
        {
            DisableAllTanks();
            HideAllShips();
        }
        else
        {
            ShowAllShips();
        }
    }

    /// <summary>
    /// Disables the movement of all the tanks
    /// </summary>
    private void DisableAllTanks()
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            tanks[i].Deactivate();
        }
    }

    /// <summary>
    /// Returns the tank at the provided position.
    /// </summary>
    /// <param name="tile">Tile on which the tank is placed.</param>
    /// <returns></returns>
    public Tank GetTankAtPosition(Transform tile)
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            if ((int)tanks[i].transform.position.x == (int)tile.position.x && (int)tanks[i].transform.position.z == (int)tile.position.z)
            {
                return tanks[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the currently selected tank.
    /// </summary>
    /// <returns></returns>
    private Tank GetSelectedTank()
    {
        Tank tank = null;
        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks[i].isActive)
            {
                return tanks[i];
            }
        }
        return tank;
    }

    /// <summary>
    /// Returns the index of the tile on which the selected tank is currently placed.
    /// </summary>
    /// <returns></returns>
    private int GetActiveTankTileIndex()
    {
        Tank selectedTank = GetSelectedTank();
        for (int i = 0; i < tiles.Count; i++)
        {
            Transform tankTransform = selectedTank.transform;
            if ((int)tankTransform.position.x == (int)tiles[i].position.x && (int)tankTransform.position.z == (int)tiles[i].position.z)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Hides all the tanks
    /// </summary>
    private void HideAllShips()
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            tanks[i].SetActive(false);
        }
    }

    /// <summary>
    /// Unhides all the ships.
    /// </summary>
    private void ShowAllShips()
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            tanks[i].SetActive(true);
        }
    }

    /// <summary>
    /// Tanks left of the player.
    /// </summary>
    /// <returns></returns>
    public int TanksLeft()
    {
        int left = 0;
        for (int i = 0; i < tanks.Count; i++)
        {
            if (tanks[i].isDead == false)
            {
                left++;
            }
        }

        return left;
    }

    /// <summary>
    /// Fires a missile on the enemy territory
    /// </summary>
    /// <param name="initMissilePosition">initial position of the missile</param>
    /// <param name="targetPosition">target position of the missile</param>
    /// <param name="targetTank">target tank</param>
    private void FireMissile(Vector3 initMissilePosition, Vector3 targetPosition, Tank targetTank)
    {
        int otherPlayerNumber = (playerNumber == 1) ? 1 : 0;
        CameraManager.instance.SetTarget(GameManager.instance.playerManagers[otherPlayerNumber].transform);
        GameManager.instance.bullet.DamageTank(initMissilePosition, targetPosition, targetTank);
        DisableAllTanks();
    }
}
