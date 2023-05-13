using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : Building
{
    [SerializeField]
    private string projectileType;

    [SerializeField]
    private string upgradeInfoColorHex = "#00ff00ff";

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

    public float AttackCooldown { get => attackCooldown; }
    
    public TowerUpgrade[] Upgrades { get; protected set; }

    public void Awake()
    {
        mySpriteRenderer = transform.GetComponent<SpriteRenderer>();
        myAnimator = transform.GetComponent<Animator>();
        rangeGameObject = transform.GetChild(0).gameObject;
    }

    public void Start()
    {
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

    public void ToggleSelection()
    {
        SpriteRenderer rangeSpriteRenderer = rangeGameObject.GetComponent<SpriteRenderer>();
        rangeSpriteRenderer.enabled = !rangeSpriteRenderer.enabled;
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
        string shipName = Util.FormatShipStatName(GetTowerName());
        string damageDelta = null, rangeDelta = null, cooldownDelta = null;

        if (NextUpgrade != null)
        {
            damageDelta = NextUpgrade.Damage.ToString();
            rangeDelta = (Range * NextUpgrade.RangeMultiplier - Range).ToString("0.00");
            cooldownDelta = NextUpgrade.Cooldown.ToString("0.00");
        }

        return string.Format(
            "{0}" +
            "\nLevel: {1}" +
            "\nDamage: {2}" +
            "\nRange: {3}" +
            "\nCooldown: {4}",
            shipName, Level,
            Util.FormatStat(Damage.ToString(), upgradeInfoColorHex, damageDelta),
            Util.FormatStat(Range.ToString("0.00"), upgradeInfoColorHex, rangeDelta),
            Util.FormatStat(AttackCooldown.ToString("0.00"), upgradeInfoColorHex, cooldownDelta, "-", "s")
        );
    }

    public string GetTooltipInfo()
    {
        return string.Format("{0}" +
                        "\nDamage: {1}" +
                        "\nRange: {2}" +
                        "\nCooldown: {3}s" +
                        "{4}" +
                        "\n{5}",
                        Util.FormatShipStatName(GetTowerName()),
                        Damage, Range, AttackCooldown.ToString("0.00"), GetExtraTooltipInfo(), GetDescription());
    }

    protected abstract string GetTowerName();
    protected abstract string GetDescription();
    protected virtual string GetExtraTooltipInfo()
    {
        return "";
    }
    public override void OnClick()
    {
        GameManager.Instance.SelectTower(this);
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

    public override bool CanBeBuiltOn(TileScript tile)
    {
        return tile.Attrs.Type != TileAttributes.TileType.WATER;
    }

    public override bool CanBeBuiltDuringWave()
    {
        return false;
    }

    public override void SetColor(Color newColor)
    {
        mySpriteRenderer.color = newColor;
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
