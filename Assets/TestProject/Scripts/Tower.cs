using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private string projectileType;

    [SerializeField] 
    private float projectileSpeed;

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    private Animator myAnimator;
    
    private SpriteRenderer mySpriteRenderer;

    private EnemyShip target;

    public EnemyShip Target
    {
        get { return target; }
    }

    private Queue<EnemyShip> enemies = new Queue<EnemyShip>();

    private bool canAttack = true;

    private float attackTimer;
    
    [SerializeField]
    private float attackCooldown;
    
    void Start()
    {
        myAnimator = transform.parent.GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Debug.Log(target);
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        
        if (target == null && enemies.Count > 0)
        {
            target = enemies.Dequeue();
        }

        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                
                myAnimator.SetTrigger("Attack");
                
                canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        Projectile projectile = (Projectile) GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            enemies.Enqueue(other.GetComponent<EnemyShip>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (target != null)
        {
            if (other.tag == "enemy")
                target = null;
        }
    }
}
