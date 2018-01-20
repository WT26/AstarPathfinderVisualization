using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridScript : MonoBehaviour {

    private List<List<GameObject>> blockGrid;
    private List<List<Block.States>> statesGrid;
    private List<List<Block.States>> initialGrid;
    private Camera camera;

    private AStarPathfinder AStar;

    private List<List<Block.States>> foundPath;

    private const int GRID_WIDTH = 20;
    private const int GRID_HEIGHT = 20;

    private int goal_i;
    private int goal_j;

    void Start () {
        blockGrid = new List<List<GameObject>>();
        statesGrid = new List<List<Block.States>>();

        AStar = new AStarPathfinder();
        foundPath = new List<List<Block.States>>();

        camera = Camera.main;

        parseMaze();
        setCamera();
        AStar.initializeMaze(statesGrid);
        foundPath = AStar.algorithm();
        updateMaze(foundPath);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                statesGrid = initialGrid;
                int tempi = hit.transform.GetComponent<Block>().getCoordI();
                int tempj = hit.transform.GetComponent<Block>().getCoordJ();
                if (!((tempi == goal_i) && (tempj == goal_j)))
                {
                    updateBlock(tempi, tempj, Block.States.END);
                    statesGrid[tempi][tempj] = Block.States.END;

                    updateBlock(goal_i, goal_j, Block.States.NOT_VISITED);
                    statesGrid[goal_i][goal_j] = Block.States.NOT_VISITED;

                    goal_i = tempi;
                    goal_j = tempj;


                    updateMaze(statesGrid);

                    AStar.initializeMaze(statesGrid);
                    foundPath = AStar.algorithm();
                    updateMaze(foundPath);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        { 
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                statesGrid = initialGrid;
                int tempi = hit.transform.GetComponent<Block>().getCoordI();
                int tempj = hit.transform.GetComponent<Block>().getCoordJ();

                if (statesGrid[tempi][tempj] == Block.States.WALL)
                {
                    updateBlock(tempi, tempj, Block.States.NOT_VISITED);
                    statesGrid[tempi][tempj] = Block.States.NOT_VISITED;
                }
                else if ((statesGrid[tempi][tempj] == Block.States.START) || (statesGrid[tempi][tempj] == Block.States.END))
                {
                    //
                }
                else
                {
                    updateBlock(tempi, tempj, Block.States.WALL);
                    statesGrid[tempi][tempj] = Block.States.WALL;
                }

                updateMaze(statesGrid);

                AStar.initializeMaze(statesGrid);
                foundPath = AStar.algorithm();
                if (foundPath.Count == 0)
                {
                    Debug.Log("Cannot reach goal.");
                }
                else
                {
                    updateMaze(foundPath);
                }
            }
        }
    }

    // Set camera in the center and pointing towards the grid.
    private void setCamera()
    {
        int armLength = 0;
        if(GRID_HEIGHT > GRID_WIDTH)
        {
            armLength = GRID_HEIGHT;
        }
        else
        {
            armLength = GRID_WIDTH;
        }
        camera.transform.position = 
            new Vector3(blockGrid[GRID_HEIGHT/2][GRID_WIDTH/2].transform.position.x,
                        blockGrid[GRID_HEIGHT / 2][GRID_WIDTH / 2].transform.position.y,
                        -armLength);
    }

    // Parses txt file and creates blockGrid and statesGrid
    private void parseMaze()
    {
        string path = "Assets/Resources/TextFiles/maze20x20.txt";
        StreamReader reader = new StreamReader(path);

        for (int i = 0; i < GRID_WIDTH; i++)
        {

            string line = reader.ReadLine();
            List<Block.States> stateRow = new List<Block.States>();
            List<GameObject> goRow = new List<GameObject>();
            for (int j = 0; j < GRID_HEIGHT; j++)
            {
                Block.States currentState = Block.States.NOT_VISITED;

                switch (line[j]) {
                    case 'O':
                        currentState = Block.States.NOT_VISITED;
                        break;
                    case '#':
                        currentState = Block.States.WALL;
                        break;
                    case 'S':
                        currentState = Block.States.START;
                        break;
                    case 'E':
                        currentState = Block.States.END;
                        goal_i = i;
                        goal_j = j;
                        break;
                }
                GameObject go = Instantiate(Resources.Load("Prefabs/BlockPrefab", typeof(GameObject))) as GameObject;
                go.GetComponent<Block>().setCoordinates(i, j);
                go.GetComponent<Transform>().position = new Vector3(j, -i, 0);
                goRow.Add(go);
                stateRow.Add(currentState);
            }
            statesGrid.Add(stateRow);
            blockGrid.Add(goRow);
        }
        initialGrid = statesGrid;
    }

    private void updateMaze(List<List<Block.States>> newMaze)
    {
        for (int i = 0; i < GRID_WIDTH; i++)
        {
            for (int j = 0; j < GRID_HEIGHT; j++)
            {
                blockGrid[i][j].GetComponent<Block>().switchState(newMaze[i][j]);
                blockGrid[i][j].GetComponent<Block>().checkState();
            }
        }
    }

    public void updateBlock(int i, int j, Block.States state)
    {
        blockGrid[i][j].GetComponent<Block>().switchState(state);
        blockGrid[i][j].GetComponent<Block>().checkState();
    }
}
