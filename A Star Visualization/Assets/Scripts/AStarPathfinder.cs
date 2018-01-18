using System.Collections.Generic;
using UnityEngine;
using Assets;

public class AStarPathfinder : MonoBehaviour {

    private List<List<Node>> nodeGrid;
    private List<List<Block.States>> statesGrid;

    private List<Node> closedSet;
    private List<Node> openSet;
    private List<Node> path;

    private Node endNode;

    private const int columnCount = 20;
    private const int rowCount = 20;

    public AStarPathfinder()
    {
        nodeGrid = new List<List<Node>>();
        statesGrid = new List<List<Block.States>>();

        openSet = new List<Node>();
        closedSet = new List<Node>();
        endNode = new Node(columnCount, rowCount);

        path = new List<Node>();
    }

    public List<List<Block.States>> algorithm()
    {
        Node current;
        while (openSet.Count > 0)
        {
            // Sort List and take the most optimal Node.
            openSet.Sort((n1, n2) => n1.fScore.CompareTo(n2.fScore));
            current = openSet[0];

            // Found the end.
            if (current.state == Block.States.END)
            {
                // Build Path From end to start and create grid of states to return for GridScript.
                createPath();
                createStatesGrid();
                return statesGrid;
            }


            closedSet.Add(current);
            openSet.Remove(current);

            for (int i = 0; i < current.neighbours.Count; i++)
            {
                // Ignore if already in closed set or wall.
                if (!closedSet.Contains(current.neighbours[i]) && (current.neighbours[i].state != Block.States.WALL))
                {

                    // Count gScore. This might need improving.
                    float tempGScore = current.gScore + (heuristic(current.neighbours[i], endNode) / 3);
                    //float tempGScore = current.gScore + heuristic(current.neighbours[i], endNode;
                    //float tempGScore = current.gScore + 0.5f;

                    // If not in OpenSet, add it there.
                    if (!openSet.Contains(current.neighbours[i]))
                    {
                        openSet.Add(current.neighbours[i]);
                        if (current.neighbours[i].state != Block.States.END)
                        {
                            current.neighbours[i].state = Block.States.OPEN_SET;
                        }
                    }
                    else if (tempGScore >= current.neighbours[i].gScore)
                    {
                        continue;
                    }

                    // Update neighbour scores.
                    current.neighbours[i].gScore = tempGScore;
                    current.neighbours[i].hScore = heuristic(current.neighbours[i], endNode);
                    current.neighbours[i].fScore = current.neighbours[i].gScore + current.neighbours[i].hScore;
                    //Debug.Log("f:" + current.neighbours[i].fScore + "   g:" + current.neighbours[i].gScore + "   h:" + current.neighbours[i].hScore);
                    current.neighbours[i].previous = current;
                }
            }
        }
        return statesGrid;
    }

    // Initialize all Nodes and find their neighbours.
    public void initializeMaze(List<List<Block.States>> statesGrid)
    {
        for (int i = 0; i < statesGrid.Count; i++)
        {
            List<Node> rowOfNodes = new List<Node>();
            for (int j = 0; j < statesGrid[i].Count; j++)
            {
                Node newNode = new Node(columnCount, rowCount);

                switch (statesGrid[i][j])
                {
                    case Block.States.START:
                        openSet.Add(newNode);
                        break;
                    case Block.States.END:
                        endNode = newNode;
                        break;
                }
                newNode.i = i;
                newNode.j = j;
                newNode.state = statesGrid[i][j];
                rowOfNodes.Add(newNode);
            }
            nodeGrid.Add(rowOfNodes);
        }

        findNeighbours();
    }


    // Heuristic function. Using euclidean distance.
    private float heuristic(Node currentNode, Node endNode)
    {
        //int d = manhattanDist(currentNode.i, currentNode.j, endNode.i, endNode.j);
        float d = euclideanDist(currentNode.i, currentNode.j, endNode.i, endNode.j);
        //float d = chebyshev(currentNode.i, currentNode.j, endNode.i, endNode.j);
        //float d = euclideanDistWithSqrt(currentNode.i, currentNode.j, endNode.i, endNode.j);
        return d;
    }

    // Different functions to be used in heuristic scoring.
    private int euclideanDist(int x1, int y1, int x2, int y2)
    {
        return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }

    private float euclideanDistWithSqrt(int x1, int y1, int x2, int y2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    private int manhattanDist(int x1, int y1, int x2, int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    private float chebyshev(int x1, int y1, int x2, int y2)
    {
        int dx = Mathf.Abs(x1 - x2);
        int dy = Mathf.Abs(y1 - y2);
        int D = 1;
        int D2 = 1;
        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }

    private void findNeighbours()
    {
        for (int i = 0; i < nodeGrid.Count; i++)
        {
            for (int j = 0; j < nodeGrid[i].Count; j++)
            {
                nodeGrid[i][j].addNeighbours(nodeGrid);
            }
        }
    }

    private void createPath()
    {
        Node tempNode = endNode;
        path.Add(tempNode);
        while (tempNode.previous != null)
        {
            //tempNode.isPath = true;
            if (tempNode.state != Block.States.END)
            {
                tempNode.state = Block.States.PATH;
            }
            path.Add(tempNode.previous);
            tempNode = tempNode.previous;
        }
    }

    private void createStatesGrid()
    {
        for (int i = 0; i < columnCount; i++)
        {
            List<Block.States> stateRow = new List<Block.States>();
            for (int j = 0; j < rowCount; j++)
            {
                Block.States currentState = nodeGrid[i][j].state;
                stateRow.Add(currentState);
            }
            statesGrid.Add(stateRow);
        }
    }
}
