using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static readonly float PROJECTILE_CLOSE_THRESHOLD = 0.001f;

    private EnemyShip target;

    private Vector3 lastTargetPostion;

    private Tower parent;

    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        lastTargetPostion = this.target.transform.position;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            flyTowards(target.transform.position);
            lastTargetPostion = target.transform.position;
        }
        else if (!target.IsActive && !isCloseToLastTargetPosition())
        {
            flyTowards(lastTargetPostion);
        }
        else
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage);

                myAnimator.SetTrigger("Impact");
            }
        }
    }

    private void flyTowards(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * parent.ProjectileSpeed);

        Vector2 dir = targetPos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private bool isCloseToLastTargetPosition()
    {
        return lastTargetPostion != null && (lastTargetPostion - transform.position).magnitude < PROJECTILE_CLOSE_THRESHOLD;
    }
}
