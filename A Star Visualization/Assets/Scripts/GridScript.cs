using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridScript : MonoBehaviour {

    private List<List<GameObject>> blockGrid;
    private List<List<Block.States>> statesGrid;
    private Camera camera;

    private AStarPathfinder AStar;

    private List<List<Block.States>> foundPath;

    private const int GRID_WIDTH = 20;
    private const int GRID_HEIGHT = 20;

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
                        break;
                }
                GameObject go = Instantiate(Resources.Load("Prefabs/BlockPrefab", typeof(GameObject))) as GameObject;
                go.GetComponent<Transform>().position = new Vector3(j, -i, 0);
                goRow.Add(go);
                stateRow.Add(currentState);
            }
            statesGrid.Add(stateRow);
            blockGrid.Add(goRow);
        }
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
}
