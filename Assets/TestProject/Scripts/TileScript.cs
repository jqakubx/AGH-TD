using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; private set; }

    private Tower myTower;
    
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    public bool Walkable { get; set; }
    
    private GameObject tower;
    
    
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

    public void Setup(Point gridPosition, Vector3 worldPos, Transform parent)
    {
        Walkable = true;
        IsEmpty = true;
        this.GridPosition = gridPosition;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPosition, this);;
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty)
            {
                ColorTile(emptyColor);
            }
            else
            {
                ColorTile(fullColor);
            }
            
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
                GameManager.Instance.DeselectTower(myTower);
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        
        Walkable = false;
        IsEmpty = false;
        ColorTile(Color.white);
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