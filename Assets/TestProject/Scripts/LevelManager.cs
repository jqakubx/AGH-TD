using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField] private Transform map;
    
    private Point firstSpawn, secondSpawn;

    [SerializeField]
    private GameObject firstPortalPrefab, secondPortalPrefab;
    
    public Dictionary<Point, TileScript> Tiles { get; set; }
    
    public float TileSize => tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }


    private void Update()
    {
        
    }

    // Create our level
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        
        string[] mapData = ReadLevelTest();
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        Vector3 maxTile = Vector3.zero;
        
        Vector3 worldStart = Camera.main!.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
        
        maxTile = Tiles[new Point(mapXSize - 1, mapYSize - 1)].transform.position;
            
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
        
        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y , Vector3 worldStartPosition)
    {
        int tileIndex = int.Parse(tileType);
        
        //Create a a new tile and makes a reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Uses the new tile variable to change the position of the tile
        
        newTile.Setup(new Point(x, y), new Vector3(worldStartPosition.x + (TileSize*x), worldStartPosition.y - (TileSize*y)), map);
    }

    private string[] ReadLevelTest()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData!.text.Replace(Environment.NewLine, string.Empty);

        return data.Split("-");
    }

    private void SpawnPortals()
    {
        firstSpawn = new Point(0, 0);

        Instantiate(firstPortalPrefab, Tiles[firstSpawn].GetComponent<TileScript>().WorldPosition,Quaternion.identity);
        
        secondSpawn = new Point(11, 6);

        Instantiate(secondPortalPrefab, Tiles[secondSpawn].GetComponent<TileScript>().WorldPosition,Quaternion.identity);
    }
        
}