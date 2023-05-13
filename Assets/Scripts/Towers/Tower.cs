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

    [SerializeField]
    private float range;

    public float Range { get => range;
        private set
        {
            range = value;
            rangeGameObject.transform.localScale = new Vector3(range, range, 1);
        }
    }

    public int Price { get; set; }
    
    private SpriteRenderer mySpriteRenderer;

    private GameObject rangeGameObject;

    private EnemyShip target;

    public int Level { get; protected set; }

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
        rangeGameObject = transform.gameObject;
        Upgrades = new TowerUpgrade[0];
        Range = range;
        Level = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        Attack();
    }

    public TowerUpgrade NextUpgrade
    {
        get {
            if (Upgrades.Length > Level - 1)
            {
                return Upgrades[Level - 1];
            }

            return null;
        }
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateUpgradeTip();
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
                if (transform.position.x > target.transform.position.x)
                {
                    myAnimator.SetInteger("Horizontal", 0);
                }
                else
                {
                    myAnimator.SetInteger("Horizontal", 1);
                }
                
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

    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamageee: {1} <color=#00ff00ff> +{2}</color>", Level, damage, NextUpgrade.Damage);
        }

        return string.Format("\nLevel: {0} \nDamage: {1}", Level, damage);
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

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        damage += NextUpgrade.Damage;
        attackCooldown -= NextUpgrade.Cooldown;
        Range *= NextUpgrade.RangeMultiplier;
        Level++;
        GameManager.Instance.UpdateUpgradeTip();
    }
}
