using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int width, lenght, halls, deadEnds;
    public float spacing;
    GridGenerator gridGen;
    GameObject[,] GOgrid;

	void Start ()
    {
        gridGen = new GridGenerator(width, lenght, halls, deadEnds, spacing);
    }
	
	void Update ()
    {
		
	}
}
