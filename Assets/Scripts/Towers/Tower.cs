using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
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

    [SerializeField]
    private int damage;

    public int Price { get; set; }
    
    private SpriteRenderer mySpriteRenderer;

    private EnemyShip target;

    public EnemyShip Target
    {
        get { return target; }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    private Queue<EnemyShip> enemies = new Queue<EnemyShip>();

    private bool canAttack = true;

    private float attackTimer;
    
    [SerializeField]
    private float attackCooldown;
    
    public TowerUpgrade[] Upgrades { get; protected set; }
    
    public void Start()
    {
        myAnimator = transform.parent.GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {
        Attack();
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
        
        if (target == null && enemies.Count > 0 && enemies.Peek().IsActive)
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
        if (target != null && !target.Alive || target != null && !target.IsActive)
        {
            target = null;
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
