using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        ImportMapImage();

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
        
        SpawnPortals();
    }

    private void ImportMapImage()
    {
        Vector3 worldStart = Camera.main!.ScreenToWorldPoint(new Vector3(0, Screen.height));
        string levelImagePath = "Level" + LevelStateController.level + "_map";
        Texture2D  tex = Resources.Load(levelImagePath) as Texture2D;
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.0f,0.0f), 25.0f);
        
        GameObject imageObject = new GameObject("MapImage");
        SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = -4;
        
        // This value is different for free aspect and fullHD
        Vector3 bottomLeftPosition = new Vector3(0f, -tex.height - 620f, 10f);
        imageObject.transform.position = Camera.main.ScreenToWorldPoint(bottomLeftPosition);
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
        string level = "Level" + LevelStateController.level;
        TextAsset bindData = Resources.Load(level) as TextAsset;

        string data = bindData!.text.Replace(Environment.NewLine, string.Empty);

        return data.Split("-");
    }

    private void SpawnPortals()
    {
        FirstSpawn = new Point(2, 2);
        GameObject tmp = (GameObject) Instantiate(firstPortalPrefab, Tiles[FirstSpawn].GetComponent<TileScript>().WorldPosition,Quaternion.identity);
        FirstPortal = tmp.GetComponent<Portal>();
        FirstPortal.name = "FirstPortal";
        
        SecondSpawn = new Point(18, 9);
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

    public List<TileScript> GetNondiagonalNeighboursOf(TileScript tile)
    {
        Point tilePoint = tile.GridPosition;
        List<TileScript> neighbours = new List<TileScript>();

        Point[] cords = {
            new Point(tilePoint.X + 1, tilePoint.Y), new Point(tilePoint.X - 1, tilePoint.Y),
            new Point(tilePoint.X, tilePoint.Y + 1), new Point(tilePoint.X, tilePoint.Y - 1),
        };

        foreach(Point cord in cords)
        {
            if (Tiles.ContainsKey(cord))
            {
                neighbours.Add(Tiles[cord]);
            }
        }

        return neighbours;
    }
}
