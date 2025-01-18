using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GridItem : MonoBehaviour
{

    public int row;
    public int column;
    public Color ballColor;
  


    private void Start()
    {
        
        string name = gameObject.name;

        string pattern = @"\d+";
        MatchCollection matches = Regex.Matches(name, pattern);

        if(matches.Count > 0)
        {
            int firstNumber = int.Parse(matches[0].Value);
            int secondNumber = int.Parse(matches[1].Value);

            column = firstNumber;
            row = secondNumber;
        }
    }

    public void UpdateTileName(int newCol, int newRow)
    {
        column = newCol;
        row = newRow;
        gameObject.name = $"({column}, {row})";
        
    }

}
