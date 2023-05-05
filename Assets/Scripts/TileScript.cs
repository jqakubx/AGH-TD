using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    private static readonly Color32 DEFAULT_COLOR = Color.white;

    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; set; }

    private Tower myTower;

    public TileAttributes Attrs { get; private set; }

    private SpriteRenderer spriteRenderer;
    
    private GameObject tower;

    public bool Walkable { get => Attrs.Walkable && IsEmpty; }
    
 
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2,
                transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2);
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    public void Setup(TileAttributes attributes, Point gridPosition, Vector3 worldPos, Transform parent)
    {
        this.Attrs = attributes;
        this.GridPosition = gridPosition;
        IsEmpty = true;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPosition, this);;
    }

    private void OnMouseOver()
    {
        if (Attrs.Buildable && !EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            ColorTile(IsEmpty ? Attrs.EmptyColor : Attrs.FullColor);
            
            if (IsEmpty && Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(DEFAULT_COLOR);
    }

    private void PlaceTower()
    {
        IsEmpty = false;
        if (AStar.GetPath(LevelManager.Instance.FirstSpawn, LevelManager.Instance.SecondSpawn) == null)
        {
            IsEmpty = true;
            return;
        }

        tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        IsEmpty = false;
        ColorTile(DEFAULT_COLOR);

        GameManager.Instance.BuyTower();
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;

        if (!IsEmpty)
        {
            tower.GetComponent<SpriteRenderer>().color = newColor;
        }
    }
}