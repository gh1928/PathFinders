using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, System.IComparable<Node>
{
    public Rigidbody2D rb;
    public int Idx;
    public bool Visitied = false;    

    public float Distance;

    private List<Node> linkedNodes;    
    public void AddRandForce(float forceRange)
    {
        float xForce = Random.Range(0, forceRange);
        float yForce = Random.Range(-forceRange, forceRange);
        rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);        
    }

    public void SetDistance(Vector3 startPoint)
    {
        Distance = Vector3.Distance(startPoint, transform.position);
    }

    public int CompareTo(Node other)
    {
        return Distance.CompareTo(other.Distance);
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
    }

    public void SetLinkedNodeList(List<Node> linkedNodes)
    {
        this.linkedNodes = linkedNodes;
    }

    public List<Node> GetSortedLinkedNodes()
    {
        foreach (Node node in linkedNodes)
        {
            node.SetDistance(transform.position);
        }

        linkedNodes.Sort();

        return linkedNodes;
    }

    public void MakeLine(Node node)
    {
        linkedNodes.Add(node);        
    }
}
