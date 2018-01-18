using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Node
    {
        public int i;
        public int j;
        public float gScore;
        public float fScore;
        public float hScore;
        public Block.States state;

        public List<Node> neighbours;
        public Node previous;

        private int cols;
        private int rows;

        public Node(int columnCount, int rowCount)
        {
            i = new int();
            j = new int();
            gScore = 0.0f;
            fScore = 0.0f;
            hScore = 0.0f;
            state = Block.States.NOT_VISITED;

            neighbours = new List<Node>();
            previous = null;
            cols = columnCount;
            rows = rowCount;
        }

        public void addNeighbours(List<List<Node>> nodeGrid)
        {
            // Right
            if (i < cols - 1) { neighbours.Add(nodeGrid[i + 1][j]); }

            // Left
            if (i > 0) { neighbours.Add(nodeGrid[i - 1][j]); }

            // Bottom
            if (j < rows - 1) { neighbours.Add(nodeGrid[i][j + 1]); }

            // Top
            if (j > 0) { neighbours.Add(nodeGrid[i][j - 1]); }


            // DIAGONAL

            // Top Left
            if ((i > 0) && (j > 0)) { neighbours.Add(nodeGrid[i - 1][j - 1]); }

            // Top Right
            if ((i < cols - 1) && (j > 0)) { neighbours.Add(nodeGrid[i + 1][j - 1]); }

            // Bottom Left
            if ((i > 0) && (j < rows - 1)) { neighbours.Add(nodeGrid[i - 1][j + 1]); }

            // Bottom Right
            if ((i < cols - 1) && (j < rows - 1)) { neighbours.Add(nodeGrid[i + 1][j + 1]); }
        }
    }
}
