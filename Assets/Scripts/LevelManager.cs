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

    public Portal FirstPortal { get; set; }
    
    private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public Dictionary<Point, TileScript> Tiles { get; set; }
    
    
    public float TileSize => tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

    public Point FirstSpawn { get => firstSpawn; private set => firstSpawn = value; }
    public Point SecondSpawn { get => secondSpawn; private set => secondSpawn = value; }

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

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

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
        TileAttributes attributes = TileAttributes.FromStringRepr(tileType);
        TileScript newTile = Instantiate(tilePrefabs[attributes.PrefabIdx]).GetComponent<TileScript>();

        newTile.Setup(
            attributes,
            new Point(x, y),
            new Vector3(worldStartPosition.x + (TileSize*x),
            worldStartPosition.y - (TileSize*y)), map
        );
    }

    private string[] ReadLevelTest()
    {
        TextAsset bindData = Resources.Load(LevelStateController.level) as TextAsset;

        string data = bindData!.text.Replace(Environment.NewLine, string.Empty);

        return data.Split("-");
    }

    private void SpawnPortals()
    {
        FirstSpawn = new Point(3, 3);
        GameObject tmp = (GameObject) Instantiate(firstPortalPrefab, Tiles[FirstSpawn].GetComponent<TileScript>().WorldPosition,Quaternion.identity);
        FirstPortal = tmp.GetComponent<Portal>();
        FirstPortal.name = "FirstPortal";
        
        SecondSpawn = new Point(9, 5);
        Instantiate(secondPortalPrefab, Tiles[secondSpawn].GetComponent<TileScript>().WorldPosition,Quaternion.identity);
    }
        
    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(FirstSpawn, secondSpawn);
    }
}
