using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graph : MonoBehaviour
{
    public Node nodePrefab;
    public TextMeshPro textPrefab;
    public LineRenderer linePrefab;
    private GameObject holder;    

    public GameObject nodeInfoUIsHolder;
    public NodeInfoPrefab nodeInfoUIPrefab;    

    public float forceRange;
    public int nodeCreateCount;
    public int minLineMakerCount = 2;
    public int maxLineMakerCount = 3;

    private LineRenderer[,] lineRenderers;

    public float createPeriod = 0.1f;

    private bool init = false;
    private bool lineCreated = false;

    List<Node> nodes;
    List<NodeInfoPrefab> nodeInfoUIs;

    public int[] dp;
    public List<int>[] pathRecord;    

    private bool doDp = false;
    public GameObject doDpButton;
    public GameObject searchNextButton;

    private Coroutine dpCoroutine;
    public void CreateNode()
    {
        Node node = Instantiate(nodePrefab, transform.position, Quaternion.identity, holder.transform);
        node.AddRandForce(forceRange);
        node.SetLinkedNodeList(new List<Node>(maxLineMakerCount));
        
        nodes.Add(node);        
    }
    public void CreateNodes()
    {
        if (init)
            return;

        nodes = new(nodeCreateCount);
        nodeInfoUIs = new(nodeCreateCount);
        lineRenderers = new LineRenderer[nodeCreateCount, nodeCreateCount];
        dp = new int[nodeCreateCount];
        pathRecord = new List<int>[nodeCreateCount];        

        for(int i = 0; i < nodeCreateCount; i++)
        {
            pathRecord[i] = new List<int>(nodeCreateCount);
        }

        holder = new GameObject("Holder");

        StartCoroutine(NodeCreateCoroutine());        
    }
    IEnumerator NodeCreateCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(createPeriod);

        for(int i = 0; i < nodeCreateCount; i++)
        {
            CreateNode();
            yield return wait;
        }

        init = true;
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

        for (int i = 0; i < nodeCreateCount; i++)
        {
            nodes[i].Idx = i;
            nodes[i].name = "Node" + i;

            var text = Instantiate(textPrefab, nodes[i].transform.position, Quaternion.identity, holder.transform);
            text.text = i.ToString();
        }
    }

    public void MakeLinesAndInfoUI()
    {   
        int infoWidth = 800 / nodeCreateCount;

        for (int i = 0; i < nodeCreateCount; i++)
        {
            var info = Instantiate(nodeInfoUIPrefab, nodeInfoUIsHolder.transform);
            info.SetData(i, int.MaxValue);
            info.SetWidth(infoWidth);     
            nodeInfoUIs.Add(info);            

            int lineCount = Random.Range(minLineMakerCount, maxLineMakerCount + 1);

            for(int j = 1; j <= lineCount; j++)
            {
                if (i + j >= nodeCreateCount)
                    break;
                
                MakeLine(nodes[i], nodes[i + j]);
            }            
        }

        nodeInfoUIs[0].SetDist(0);
    }
    private void MakeLine(Node a, Node b)
    {
        a.MakeLine(b);

        var line = Instantiate(linePrefab, holder.transform);

        line.SetPosition(0, a.transform.position);
        line.SetPosition(1, b.transform.position);

        lineRenderers[a.Idx,b.Idx] = line;
    }

    public void MakeGraph()
    {
        if (!init)
            return;

        if (lineCreated)
            return;

        lineCreated = true;

        StopNodesMoving();
        SetNumbers();
        MakeLinesAndInfoUI();

        doDpButton.SetActive(true);
    }

    public void ClearGraph()
    {
        if (!init)
            return;

        init = false;
        lineCreated = false;

        foreach (var infoUI in nodeInfoUIs)
        {   
            Destroy(infoUI.gameObject);
        }

        Destroy(holder);
        
        if(dpCoroutine != null)
            StopCoroutine(dpCoroutine);

        searchNextButton.SetActive(false);
        doDpButton.SetActive(false);
    }

    public void StartSearch()
    {
        doDpButton.SetActive(false);
        searchNextButton.SetActive(true);
        dpCoroutine = StartCoroutine(DoDp());
    }

    public void SearchNext()
    {
        doDp = true;
    }

    IEnumerator DoDp()
    {
        System.Array.Fill(dp, int.MaxValue);
        dp[0] = 0;

        WaitUntil waitUntildoDpTrue = new WaitUntil(() => doDp);

        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(nodes[0]);
        pathRecord[0].Add(0);

        while (queue.Count > 0)
        {
            var dequeued = queue.Dequeue();

            foreach(var linked in dequeued.GetLinkedNodes())
            {
                int currIdx = dequeued.Idx;
                int destIdx = linked.Idx;
                int dist = (int)Vector2.Distance(dequeued.transform.position, nodes[destIdx].transform.position);

                int newDist = dp[currIdx] + dist;

                if (dp[destIdx] == int.MaxValue || newDist < dp[destIdx])
                {
                    pathRecord[destIdx].Clear();
                    pathRecord[destIdx].AddRange(pathRecord[currIdx]);
                    pathRecord[destIdx].Add(destIdx);

                    dp[destIdx] = newDist;
                    nodeInfoUIs[destIdx].SetDist(newDist);
                }

                if(!linked.Visitied)
                {
                    queue.Enqueue(linked);
                    linked.Visitied = true;
                }
            }

            doDp = false;
            yield return waitUntildoDpTrue;
        }

        var result = pathRecord[nodeCreateCount - 1];

        for (int i = 0; i < result.Count - 1; i++)
        {
            var line = lineRenderers[result[i], result[i + 1]];
            line.startColor = Color.green;
            line.endColor = Color.green;

            line.sortingOrder++;
        }
    }    
}
