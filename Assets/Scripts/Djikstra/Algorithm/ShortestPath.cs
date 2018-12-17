using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    private GameObject[] nodes;

    // Finding the shortest path and return in a List
    public List<Transform> findShortestPath(Transform start, Transform end)
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");

        List<Transform> result = new List<Transform>();
        Transform node = DijkstrasAlgorithm(start, end);

        // If previous node still available search.
        while (node != null)
        {
            result.Add(node);
            Nodes currentNode = node.GetComponent<Nodes>();
            node = currentNode.getParentNode();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    // Dijkstra Algorithm to find the shortest path
    // The end node
    private Transform DijkstrasAlgorithm(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

        // Nodes that are unexplored
        List<Transform> unexplored = new List<Transform>();

        // Adding all the nodes we found into unexplored.
        foreach (GameObject obj in nodes)
        {
            Nodes n = obj.GetComponent<Nodes>();
            if (n.isWalkable())
            {
                n.resetNode();
                unexplored.Add(obj.transform);
            }
        }

        // Set the starting node weight to 0;
        Nodes startNode = start.GetComponent<Nodes>();
        startNode.setWeight(0);

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Nodes>().getWeight().CompareTo(y.GetComponent<Nodes>().getWeight()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            unexplored.Remove(current);

            Nodes currentNode = current.GetComponent<Nodes>();
            List<Transform> neighbours = currentNode.getNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                Nodes node = neighNode.GetComponent<Nodes>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.isWalkable())
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(neighNode.position, current.position);
                    distance = currentNode.getWeight() + distance;

                    // If the added distance is less than the current weight.
                    if (distance < node.getWeight())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.setWeight(distance);
                        node.setParentNode(current);
                    }
                }
            }

        }

        return end;
    }

}
