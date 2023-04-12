using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Stack<Node> path;
    
    public Point GridPosition { get; set; }

    private Vector3 destination;

    public bool IsActive { get; set; }

    private Animator myAnimator;
    
    private void Update()
    {
        Move();
    }

    public void Spawn()
    {
        transform.position = LevelManager.Instance.FirstPortal.transform.position;

        myAnimator = GetComponent<Animator>();
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1.0f, 1.0f), false));
        
        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;
        IsActive = true;
        if (remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;
            
            Animate(GridPosition, path.Peek().GridPosition);
            
            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }

    private void Animate(Point currentPos, Point newPos)
    {
        if (currentPos.Y > newPos.Y)
        {
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
            //Moving Down
        }
        else if (currentPos.Y < newPos.Y)
        {
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
            //Moving up
        }
        else if (currentPos.Y == newPos.Y)
        {
            if (currentPos.X > newPos.X)
            {
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
                //Moving left
            }
            else if (currentPos.X < newPos.X)
            {
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
                //Moving right
            }
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "SecondPortal")
        {
            StartCoroutine(Scale(new Vector3(1.0f, 1.0f), new Vector3(0.1f, 0.1f), true));
            col.GetComponent<Portal>().Open();
            GameManager.Instance.Lives--;
        }
    }

    private void Release()
    {
        IsActive = false;
        GridPosition = LevelManager.Instance.FirstSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveEnemy(this);
    }
}
