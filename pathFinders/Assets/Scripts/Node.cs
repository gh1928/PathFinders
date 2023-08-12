using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, System.IComparable<Node>
{
    public Rigidbody2D rb;    
    public void AddRandForce(float forceRange)
    {
        float xForce = Random.Range(0, forceRange);
        float yForce = Random.Range(-forceRange, forceRange);
        rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);        
    }
    public int CompareTo(Node other)
    {
        return transform.position.x.CompareTo((other.transform.position.x));
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
    }
}