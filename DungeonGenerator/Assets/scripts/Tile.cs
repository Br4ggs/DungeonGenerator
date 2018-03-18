using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType
    {
        crossSec, tSec, corner, tunnel, spawn, hall, empty
    }

    TileType typeOfTile;
    GameObject instancedTile;
    float yRotation;
    int width, lenght;
    float xPos, yPos;
    bool top, left, right, bottom;

    bool visited;



    public Tile(int width, int length, float xPos, float yPos, float yRotation, TileType typeOfTile, Color test)
    {
        this.width = width;
        this.lenght = length;
        this.xPos = xPos;
        this.yPos = yPos;
        this.yRotation = yRotation;
        this.typeOfTile = typeOfTile;
        top = left = right = bottom = false;
        visited = false;

        /*GameObject tile = Resources.Load(typeOfTile.ToString(), typeof(GameObject)) as GameObject;
        instancedTile = GameObject.Instantiate(tile, new Vector3(xPos, 0, yPos), tile.transform.rotation);
        instancedTile.GetComponent<MeshRenderer>().material.color = test;
        */
    }

    public TileType TypeOfTile
    {
        get { return typeOfTile; }
        set { typeOfTile = value; } //might be handy to change mesh if enum value changes
    }

    public GameObject InstancedTile
    {
        get { return instancedTile; }
        set { instancedTile = value; }
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int Length
    {
        get { return lenght; }
        set { lenght = value; }
    }

    public float XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }

    public float YPos
    {
        get { return yPos; }
        set { yPos = value; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; }
    }

    // 0,-1 == top
    // -1,0 == right
    // 1,0 == left
    // 0,1 == bottom
    // this only counts for the current tile!!! the neighbouring tile is the reverse!!! //
    public bool GetConnector(int width, int lenght)
    {
        if(width == 0 && lenght == -1)
        {
            return top;
        }
        else if(width == -1 && lenght == 0)
        {
            return right;
        }
        else if(width == 1 && lenght == 0)
        {
            return left;
        }
        else if(width == 0 && lenght == 1)
        {
            return bottom;
        }
        else { return false; }
    }

    public void SetConnector(int width, int lenght , bool value)
    {
        if (width == 0 && lenght == -1)
        {
            top = value;
        }
        else if (width == -1 && lenght == 0)
        {
            right = value;
        }
        else if (width == 1 && lenght == 0)
        {
            left = value;
        }
        else if (width == 0 && lenght == 1)
        {
            bottom = value;
        }

    }

    public void SetColor(Color col)
    {
        instancedTile.GetComponent<MeshRenderer>().material.color = col;
    }

    public void SetSection()
    {
        //FOUR WAY
        if (top && right && left && bottom)
        {
            if (typeOfTile == TileType.crossSec)
            {
                GameObject model = Resources.Load("crossSec", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            }
            else
            {
                GameObject model = Resources.Load("plusSec", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            }
        }

        //TWO WAY
        else if (top && !right && !left && bottom || !top && right && left && !bottom)
        {
            GameObject model = Resources.Load("hallway", typeof(GameObject)) as GameObject;
            instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            if (top && bottom)
            {
                instancedTile.transform.Rotate(0, 0, 90);
            }
        }
        
        //CORNERS
        else if (top && right && !left && !bottom || top && !right && left && !bottom || !top && right && !left && bottom || !top && !right && left && bottom)
        {
            GameObject model = Resources.Load("corner", typeof(GameObject)) as GameObject;
            instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            if(top && left)
            {
                instancedTile.transform.Rotate(0, 0, 90);
            }
            else if(top && right )
            {
                instancedTile.transform.Rotate(0, 0, 180);
            }
            else if(bottom && right)
            {
                instancedTile.transform.Rotate(0, 0, 270);
            }
        }

        //THREE WAY
        else if (!top && bottom && right && left || !right && left && top && bottom || !left && right && bottom && top || !bottom && top && right && left)
        {
            GameObject model = Resources.Load("tSec", typeof(GameObject)) as GameObject;
            instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            if(!right)
            {
                instancedTile.transform.Rotate(0, 0, 90);
            }
            if (!bottom)
            {
                instancedTile.transform.Rotate(0, 0, 180);
            }
            if (!left)
            {
                instancedTile.transform.Rotate(0, 0, 270);
            }
        }
        else
        {
            //DEAD ENDS
            if (bottom)
            {
                GameObject model = Resources.Load("spawn", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
                instancedTile.transform.Rotate(0, 0, 90);
            }
            if (left)
            {
                GameObject model = Resources.Load("spawn", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
                instancedTile.transform.Rotate(0, 0, 180);
            }
            if (right)
            {
                GameObject model = Resources.Load("spawn", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
                instancedTile.transform.Rotate(0, 0, 0);
            }
            if (top)
            {
                GameObject model = Resources.Load("spawn", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
                instancedTile.transform.Rotate(0, 0, 270);
            }
            //EMPTY SPACE
            //dont instantiate any model
            /*if(!top && !bottom && !left && !right)
            {
                GameObject model = Resources.Load("empty", typeof(GameObject)) as GameObject;
                instancedTile = GameObject.Instantiate(model, new Vector3(xPos, 0, yPos), model.transform.rotation);
            }*/
        }
        //instancedTile.AddComponent<MeshCollider>();
    }

    public void SetCrossSection()
    {
        typeOfTile = TileType.crossSec;
    }

    public int GetConnectionCount()
    {
        bool[] values = new bool[4] { top, left, right, bottom };
        int openings = 0;
        for(int i = 0; i < values.Length; i++)
        {
            if (values[i])
            {
                openings++;
            }
        }
        return openings;
    }

    //tool for getting the opening in dead ends
    public Vector2 GetDeadEndOpening()
    {
        int openings = GetConnectionCount();
        if(openings == 1) //just to make sure its only executed on dead ends
        {
            if (top)
            {
                return new Vector2(0, -1);
            }
            else if (right)
            {
                return new Vector2(-1, 0);
            }
            else if (left)
            {
                return new Vector2(1, 0);
            }
            else if (bottom)
            {
                return new Vector2(0, 1);
            }
            else
            {
                return new Vector2(9, 9);
            }
        }

        else
        {
            return new Vector2(9, 9);
        }
    }
}
