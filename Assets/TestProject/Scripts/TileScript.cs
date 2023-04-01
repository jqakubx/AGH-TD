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
    
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer SpriteRenderer;
    
    
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
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    public void Setup(Point gridPosition, Vector3 worldPos, Transform parent)
    {
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
            if (!IsEmpty)
            {
                ColorTile(fullColor);
                
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        
        tower.transform.SetParent(transform);

        IsEmpty = false;
        ColorTile(Color.white);
        GameManager.Instance.BuyTower();
    }

    private void ColorTile(Color newColor)
    {
        SpriteRenderer.color = newColor;
    }
}