using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graph : MonoBehaviour
{
    public Node nodePrefab;
    public TextMeshPro textPrefab;
    public LineRenderer linePrefab;

    public float forceRange;
    public int createCount;

    public float createPeriod = 0.1f;

    private bool init = false;
    private bool lineMaked = false;

    List<Node> nodes = new();

    public void CreateNode()
    {
        Node node = Instantiate(nodePrefab, transform.position, Quaternion.identity, transform);
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
    public void SetNumbers()
    {
        nodes.Sort();

        int nodeCount = nodes.Count;

        for (int i = 0; i < nodeCount; i++)
        {
            var text = Instantiate(textPrefab, nodes[i].transform.position, Quaternion.identity);
            text.text = i.ToString();
        }
    }

    public void SetGoalLine()
    {
        int nodeCount = nodes.Count;

        int range = Random.Range(0, nodeCount);

        //for(int i = 0; i < range; i++)
        //{
        //    var line = Instantiate(linePrefab);


        //}

        var line = Instantiate(linePrefab);

        line.SetPosition(0, nodes[0].transform.position);
        line.SetPosition(1, nodes[nodeCount - 1].transform.position);
    }

    public void MakeLines()
    {
        if (!init)
            return;

        if (lineMaked)
            return;

        lineMaked = true;

        StopNodesMoving();
        SetNumbers();
        SetGoalLine();
    }
}
