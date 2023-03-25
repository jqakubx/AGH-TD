using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int hp = 100;
    [SerializeField]
    private int movementSpeed = 2;

    private int targetIdx = 0;

    public void destroy()
    {
        Destroy(transform.gameObject);
    }

    public int getCurrentTargetIdx()
    {
        return targetIdx;
    }

    public void setCurrentTargetIdx(int targetIdx)
    {
        this.targetIdx = targetIdx;
    }

    public void move(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
    }

    public bool hasReachedTarget(GameObject target)
    {
        // TODO taken from tutorial but i don't rly like that
        return (transform.position - target.transform.position).magnitude < 0.001f;
    }

    public void takeDamage(int dmg)
    {
        hp = Mathf.Max(0, hp - dmg);
    }

    public bool isDead()
    {
        return hp == 0;
    }
}
