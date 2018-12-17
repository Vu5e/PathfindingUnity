using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour 
{

    private Transform node;
    private Transform startNode;
    private Transform endNode;
    private List<Transform> blockPath = new List<Transform>();
    
	void Update ()
    {
        mouseInput();
    }
    
    // Mouse click.
    private void mouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            // Update colors for every mouse clicked.
            this.colorBlockPath();
            this.updateNodeColor();

            // Get the raycast from the mouse position from screen.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
            {
                //unmark previous
                Renderer rend;
                if (node != null)
                {
                    rend = node.GetComponent<Renderer>();
					rend.material.color = Color.yellow;
                }

                // We now update the selected node.
                node = hit.transform;

                // Mark it
                rend = node.GetComponent<Renderer>();
                rend.material.color = Color.green;

            }
        }
    }

    // Button for Set Starting node.
    public void btnStartNode()
    {
        if (node != null)
        {
            Nodes n = node.GetComponent<Nodes>();

            // Making sure only walkable node are able to set as start.
            if (n.isWalkable())
            {
                // If this is a new start node, we will just set it to blue.
                if (startNode == null)
                {
                    Renderer rend = node.GetComponent<Renderer>();
                    rend.material.color = Color.blue;
                }
                else
                {
                    // Reverse the color of the previous node
                    Renderer rend = startNode.GetComponent<Renderer>();
					rend.material.color = Color.yellow;

                    // Set the new node as blue.
                    rend = node.GetComponent<Renderer>();
                    rend.material.color = Color.blue;
                }

                startNode = node;
                node = null;
            }
        }
    }

    // Button for Set End node.
    public void btnEndNode()
    {
        if (node != null)
        {
            Nodes n = node.GetComponent<Nodes>();

            // Making sure only walkable node are able to set as end.
            if (n.isWalkable())
            {
                // If this is a new end node, we will just set it to cyan.
                if (endNode == null)
                {
                    Renderer rend = node.GetComponent<Renderer>();
                    rend.material.color = Color.cyan;
                }
                else
                {
                    // Reverse the color of the previous node
                    Renderer rend = endNode.GetComponent<Renderer>();
					rend.material.color = Color.yellow;

                    // Set the new node as cyan.
                    rend = node.GetComponent<Renderer>();
                    rend.material.color = Color.cyan;
                }

                endNode = node;
                node = null;
            }
        }
    }

    // Button for find path.
    public void btnFindPath()
    {   
        // Only find if there are start and end node.
        if (startNode != null && endNode != null)
        {
            // Execute Shortest Path.
            ShortestPath finder = gameObject.GetComponent<ShortestPath>();
            List<Transform> paths = finder.findShortestPath(startNode, endNode);

            // Colour the node red.
            foreach (Transform path in paths)
            {
                Renderer rend = path.GetComponent<Renderer>();
                rend.material.color = Color.red;
            }
        }
    }

    // Button for blocking a path.
    public void btnBlockPath()
    {
        if (node != null)
        {
            // Render the selected node to black.
            Renderer rend = node.GetComponent<Renderer>();
            rend.material.color = Color.black;

            // Set selected node to not walkable
            Nodes n = node.GetComponent<Nodes>();
            n.setWalkable(false);

            // Add the node to the block path list.
            blockPath.Add(node);

            // If the block path is start node, we remove start node.
            if (node == startNode)
            {
                startNode = null;
            }

            // If the block path is end node, we remove end node.
            if (node == endNode)
            {
                endNode = null;
            }

            node = null;
        }

        // For selection grid system.
        UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
        List<Transform> selected = selection.getSelectedObjects();

        foreach(Transform nd in selected)
        {
            // Render the selected node to black.
            Renderer rend = nd.GetComponent<Renderer>();
            rend.material.color = Color.black;

            // Set selected node to not walkable
            Nodes n = nd.GetComponent<Nodes>();
            n.setWalkable(false);

            // Add the node to the block path list.
            blockPath.Add(nd);

            // If the block path is start node, we remove start node.
            if (nd == startNode)
            {
                startNode = null;
            }

            // If the block path is end node, we remove end node.
            if (nd == endNode)
            {
                endNode = null;
            }
        }

        selection.clearSelections();
    }

    // Button to unblock a path.
    public void btnUnblockPath()
    {
        if (node != null)
        {
            // Set selected node to yellow.
            Renderer rend = node.GetComponent<Renderer>();
            rend.material.color = Color.yellow;

            // Set selected not to walkable.
            Nodes n = node.GetComponent<Nodes>();
            n.setWalkable(true);

            // Remove selected node from the block path list.
            blockPath.Remove(node);

            node = null;
        }

        // For selection grid system.
        UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
        List<Transform> selected = selection.getSelectedObjects();

        foreach (Transform nd in selected)
        {
            // Set selected node to yellow.
            Renderer rend = nd.GetComponent<Renderer>();
			rend.material.color = Color.yellow;

            // Set selected not to walkable.
            Nodes n = nd.GetComponent<Nodes>();
            n.setWalkable(true);

            // Remove selected node from the block path list.
            blockPath.Remove(nd);
        }

        selection.clearSelections();
    }

    // Clear all the block path.
    public void btnClearBlock()
    {   
        // For each blocked path in the list
        foreach(Transform path in blockPath)
        {   
            // Set walkable to true.
            Nodes n = path.GetComponent<Nodes>();
            n.setWalkable(true);

            // Set their color to yellow.
            Renderer rend = path.GetComponent<Renderer>();
			rend.material.color = Color.yellow;

        }
        // Clear the block path list and 
        blockPath.Clear();
    }

    // Button to restart level.
    public void btnRestart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    // Coloured unwalkable path to black.
    private void colorBlockPath()
    {
        foreach(Transform block in blockPath)
        {
            Renderer rend = block.GetComponent<Renderer>();
            rend.material.color = Color.black;
        }
    }

    // Refresh Update Nodes Color.
    private void updateNodeColor()
    {
        if (startNode != null)
        {
            Renderer rend = startNode.GetComponent<Renderer>();
            rend.material.color = Color.blue;
        }

        if (endNode != null)
        {
            Renderer rend = endNode.GetComponent<Renderer>();
            rend.material.color = Color.cyan;
        }
    }

}
