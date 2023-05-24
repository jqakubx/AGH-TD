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

    private Building myBuilding;

    public TileAttributes Attrs { get; private set; }

    private SpriteRenderer spriteRenderer;
    
    private GameObject building;

    public Building PlacedBuilding { get => myBuilding; }

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
        BuildingButton clickedButton = GameManager.Instance.ClickedBtn;

        if (!EventSystem.current.IsPointerOverGameObject() && clickedButton != null && clickedButton.BuildingTemplate.CanBeBuiltOn(this))
        {
            ColorTile(IsEmpty ? Attrs.EmptyColor : Attrs.FullColor);
            
            if (IsEmpty && Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {  
            if (myBuilding != null)
            {
                myBuilding.OnClick();
            }
            else
            {
                GameManager.Instance.DeselectSelection();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(DEFAULT_COLOR);
    }

    private void PlaceBuilding()
    {
        IsEmpty = false;
        if (AStar.GetPath(LevelManager.Instance.FirstSpawn, LevelManager.Instance.SecondSpawn) == null)
        {
            IsEmpty = true;
            return;
        }

        building = Instantiate(GameManager.Instance.ClickedBtn.BuildingPrefab, transform.position, Quaternion.identity);
        building.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        building.transform.SetParent(transform);

        myBuilding = building.transform.GetComponent<Building>();
        myBuilding.Price = GameManager.Instance.ClickedBtn.Price;

        ColorTile(DEFAULT_COLOR);
        IsEmpty = false;

        GameManager.Instance.BuyBuilding(myBuilding);
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;

        if (!IsEmpty)
        {
            myBuilding.SetColor(newColor);
        }
    }
}