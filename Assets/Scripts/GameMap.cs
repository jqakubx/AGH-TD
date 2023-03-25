using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMap : MonoBehaviour
{
    public GameObject mapTile;

    [SerializeField]
    private Color pathColor = Color.grey;
    [SerializeField]
    private Color startColor = Color.cyan;
    [SerializeField]
    private Color endColor = new Color(0.1f, 0.4f, 0.9f);
    [SerializeField]
    private int mapWidth = 8;
    [SerializeField]
    private int mapHeight = 8;

    private List<GameObject> mapTiles = new List<GameObject>();
    private List<GameObject> pathTiles = new List<GameObject>();

    void Start()
    {
        generateMap();
        buildPath();
        paintPath();
    }

    public List<GameObject> getEnemyPath()
    {
        return pathTiles;
    }

    public GameObject getSpawnTile()
    {
        return pathTiles[0];
    }

    private void generateMap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var newTile = Instantiate(mapTile);
                newTile.transform.position = new Vector2(x, y);
                mapTiles.Add(newTile);
            }
        }
    }

    private void buildPath()
    {
        // TODO something more interesting
        var from = mapTiles[Random.Range((mapHeight - 1) * mapWidth + 1, mapHeight * mapWidth - 2)];
        var to = mapTiles[Random.Range(1, mapWidth - 2)];
        var cur = from;

        int dx = (int)to.transform.position.x - (int)from.transform.position.x;
        int xDir = (int)Mathf.Sign(dx);

        for (int i = 0; i < Mathf.Abs(dx); i++)
        {
            pathTiles.Add(cur);
            int nextX = (int)cur.transform.position.x + xDir;
            cur = mapTiles[(mapHeight - 1) * mapWidth + nextX];
        }

        int dy = (int)to.transform.position.y - (int)from.transform.position.y;
        int yDir = (int)Mathf.Sign(dy);

        for (int i = 0; i < Mathf.Abs(dy); i++)
        {
            pathTiles.Add(cur);
            int nextY = (int)cur.transform.position.y + yDir;
            cur = mapTiles[nextY * mapWidth + (int)cur.transform.position.x];
        }

        pathTiles.Add(cur);
    }

    private void paintPath()
    {
        pathTiles.ForEach(pathElem => pathElem.GetComponent<SpriteRenderer>().color = pathColor);
        pathTiles.First().GetComponent<SpriteRenderer>().color = startColor;
        pathTiles.Last().GetComponent<SpriteRenderer>().color = endColor;
    }
}
