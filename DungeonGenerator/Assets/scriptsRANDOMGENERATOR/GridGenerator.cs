using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator
{
    int width, length, halls, deadEnds;
    float spacing;
    Tile[,] tiles;
    public Stack<Tile> tileStack;

    System.Random rnd;

    public GridGenerator(int width, int lenght, int halls, int deadEnds, float spacing)
    {
        this.width = width;
        this.length = lenght;
        this.halls = halls;
        this.deadEnds = deadEnds;
        this.spacing = spacing;
        rnd = new System.Random();
        tiles = GenerateGrid();
    }

    public Tile[,] GenerateGrid()
    {
        tiles = new Tile[width, length];


        /////////////////////////fill in remainder of grid
        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                if (tiles[w, l] == null)
                {
                    tiles[w, l] = new Tile(w, l, w * spacing, l * spacing, 0, Tile.TileType.spawn, Color.white);
                }
            }
        }
        /////////////////////////

        /////////////////////////set the hall sections
        int placedhalls = 0;
        while (placedhalls < halls)
        {
            int w = Random.Range(1, width - 1);
            int l = Random.Range(1, length - 1);
            int placehall = 0;
            for (int wPos = -1; wPos <= 1; wPos++)
            {
                for (int lPos = -1; lPos <= 1; lPos++)
                {
                    if (wPos == lPos || wPos == -lPos) //skips unnecessary tiles
                    {
                        continue;
                    }

                    int nbW = w + wPos;
                    int nbL = l + lPos;
                    if (nbW >= 0 && nbL >= 0 && nbW <= width && nbL <= length)
                    {
                        placehall++;
                        SetTileOpenings(tiles[w, l], tiles[nbW, nbL]);
                    }
                }
            }
            if (placehall == 4)
            {
                tiles[w, l].SetCrossSection();
                tiles[w, l].Visited = true;
                placedhalls++;

            }
        }
        /////////////////////////

        /////////////////////////create the actual maze
        CreateMaze(Mathf.RoundToInt(Random.Range(0, width)), Mathf.RoundToInt(Random.Range(0, length)));
        /////////////////////////

        /////////////////////////sets correct gameobject
        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                tiles[w, l].SetSection();
            }
        }

        return tiles;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetLenght()
    {
        return length;
    }


    void CreateMaze(int w, int l)
    {
        tileStack = new Stack<Tile>();
        Tile currentTile = tiles[w, l];
        currentTile.Visited = true;
        tileStack.Push(currentTile);

        while (tileStack.Count > 0)
        {
            currentTile = CheckForNeighbours(currentTile.Width, currentTile.Length);
            currentTile.Visited = true;
        }

        for(int i = 0; i < deadEnds; i++)
        {
            RemoveDeadEnds();
        }
    }

    Tile CheckForNeighbours(int w, int l)
    {
        List<Tile> neighbours = new List<Tile>();
        for (int wPos = -1; wPos <= 1; wPos++)
        {
            for (int lPos = -1; lPos <= 1; lPos++)
            {
                if (wPos == lPos || wPos == -lPos) //skips unnecessary tiles
                {
                    continue;
                }

                int nbW = w + wPos;
                int nbL = l + lPos;
                if (nbW >= 0 && nbL >= 0 && nbW <= width - 1 && nbL <= length - 1)
                {
                    if (tiles[nbW, nbL].Visited == false)
                    {
                        neighbours.Add(tiles[nbW, nbL]);
                    }
                }
            }
        }

        if (neighbours.Count == 0)
        {
            return tileStack.Pop();
        }
        else
        {
            tileStack.Push(tiles[w, l]);
            int r = rnd.Next(neighbours.Count);
            SetTileOpenings(tiles[w, l], neighbours[r]);
            return neighbours[r];
        }
    }

    void SetTileOpenings(Tile currentTile, Tile newTile)
    {
        int OPwidth = currentTile.Width - newTile.Width;
        int OPlength = currentTile.Length - newTile.Length;

        currentTile.SetConnector(OPwidth, OPlength, true);
        newTile.SetConnector(-OPwidth, -OPlength, true);
    }

    void RemoveDeadEnds()
    {
        List<Tile> deadends = new List<Tile>();
        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                int openings = tiles[w, l].GetConnectionCount();
                if (openings == 1)
                {
                    deadends.Add(tiles[w, l]);
                }
            }
        }

        foreach (Tile t in deadends)
        {
            Vector2 opening = t.GetDeadEndOpening();

            int nbW = t.Width - (int)opening.x;
            int nbL = t.Length - (int)opening.y;
            if (nbW >= 0 && nbL >= 0 && nbW <= width - 1 && nbL <= length - 1)
            {
                t.SetConnector((int)opening.x, (int)opening.y, false);
                tiles[nbW, nbL].SetConnector((int)-opening.x, (int)-opening.y, false);
            }
        }
    }
}
