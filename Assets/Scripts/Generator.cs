
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; //0 - top; 1 - down; 2 - left; 3 - right;
    }

    [SerializeField] private Vector2Int size;
    [SerializeField] private int initPosition = 0;
    [SerializeField] GameObject room;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private int nRooms;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    private void MazeGenerator()
    {
        board = new List<Cell>();
        
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = initPosition;

        Stack<int> path = new Stack<int>();// Pila

        int num = 0;

        while(num < nRooms)
        {
            num++;
            board[num].visited = true;

            // Comprobar celdas vecinas
            List<int> neighbourgs = CheckNeighbours(currentCell);

            //Comprobamos si no hay vecinos
            if(neighbourgs.Count == 0)
            {
                // Si no hay mas caminos que probar
                if(path.Count == 0)
                {
                    break;
                }
                else //Retrocedemos un camino
                {
                    currentCell = path.Pop(); // Pop saca 1 elemento de la pila
                }

            }
            else // si hay vecinos
            {
                path.Push(currentCell);
                int newCell = neighbourgs[Random.Range(0, neighbourgs.Count)]; //asignamos la nueva celda aleatoriamente

                // Vecino derecha o abajo
                if(newCell > currentCell)
                {
                    if(newCell - 1 == currentCell)//derecha
                    {
                        board[currentCell].status[3] = true; // a la casilla actual le abrimos la derecha y a la nueva la izquierda
                        board[newCell].status[2] = true;
                    }
                    else // Abajo
                    {
                        board[currentCell].status[0] = true;
                        board[newCell].status[1] = true;
                    }
                }
                else // Vecino izquierda o arriba
                {
                    if (newCell + 1 == currentCell)//izquierda
                    {
                        board[currentCell].status[2] = true; // a la casilla actual le abrimos la derecha y a la nueva la izquierda
                        board[newCell].status[3] = true;
                    }
                    else //arriba
                    {
                        board[currentCell].status[1] = true;
                        board[newCell].status[0] = true;
                    }
                }

                currentCell = newCell;
            }
        }

        DungeonGenerator();
    }

    private void DungeonGenerator()
    {
        for(int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var newRoom = Instantiate(room, new Vector3(i * roomSize.x, 0, j * roomSize.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateSala(board[i + j * size.x].status);
            }
        }
    }

    public List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        if(cell - size.x > 0 && !board[cell - size.x].visited)// Comprobamos top
        {
            neighbours.Add(cell - size.x);
        }

        if (cell + size.x < board.Count && !board[cell + size.x].visited)// Comprobamos down
        {
            neighbours.Add(cell + size.x);
        }

        if (cell % size.x != 0 && !board[cell - 1].visited)// Comprobamos left
        {
            neighbours.Add(cell - 1);
        }

        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)// Comprobamos rigth
        {
            neighbours.Add(cell + 1);
        }

        return neighbours;
    }
}
