using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graph : MonoBehaviour
{
    public Node prefab;
    public TextMeshPro textPrefab;
    public float forceRange;
    public int createCount;

    public float createPeriod = 0.1f;

    private bool init = false;

    List<Node> nodes = new();

    public void CreateNode()
    {
        Node node = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        node.AddRandForce(forceRange);        
        nodes.Add(node);
    }
    public void CreateNodes()
    {
        if (init)
            return;

        StartCoroutine(NodeCreateCoroutine());
        init = true;
    }
    IEnumerator NodeCreateCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(createPeriod);

        for(int i = 0; i < createCount; i++)
        {
            CreateNode();
            yield return wait;
        }
        
        yield break;
    }
    public void StopNodesMoving()
    {
        foreach (Node node in nodes)
        {
            node.Stop();
        }
    }
    public void MakeLines()
    {
        StopNodesMoving();
        nodes.Sort();        

        int nodeCount = nodes.Count;

        for(int i = 0; i < nodeCount; i++)
        {
            var text = Instantiate(textPrefab, nodes[i].transform.position, Quaternion.identity);
            text.text = i.ToString();
        }
    }

}
