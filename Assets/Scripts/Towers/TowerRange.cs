using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    private Tower parent;

    public void Start()
    {
        parent = transform.parent.GetComponent<Tower>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        parent.OnTriggerEnter2D(other);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        parent.OnTriggerExit2D(other);
    }
}
