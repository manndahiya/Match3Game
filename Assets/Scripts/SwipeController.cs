using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private GridItem grid;

    public Board board;
    public GameObject otherBall;
 
 
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private float swipeAngle = 0;

    private int width = 6;
    private int height = 8;
    private int previousRow;
    private int previousColumn;

    private bool isMatched = false;


    void Start()
    {
        board = FindFirstObjectByType<Board>();
        grid = GetComponent<GridItem>();

        previousRow = grid.row;
        previousColumn = grid.column;
       
    }


    void Update()
    {
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = Color.white;
        }
    }

    private void OnMouseDown()
    {

        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseUp()
    {
       
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        //converting to degrees
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        MovePieces();
    }

    private void MovePieces()
    {
        otherBall = null;

        //Checking for Right Swipe
        if (swipeAngle <= 45 && swipeAngle >= -45 && grid.column < width - 1)
        {
            otherBall = board.allBalls[grid.column + 1, grid.row];
          
            otherBall.GetComponent<GridItem>().column -= 1;
            grid.column++;
           
        }

        //Checking for Up Swipe
        else if (swipeAngle <= 135 && swipeAngle > 45 && grid.row < height - 1)
        {
            otherBall = board.allBalls[grid.column, grid.row + 1];
            
            otherBall.GetComponent<GridItem>().row -= 1;
            grid.row++;
        }

        //Checking for Left Swipe
        else if ((swipeAngle <= -135 || swipeAngle > 135) && grid.column > 0)
        {
            otherBall = board.allBalls[grid.column - 1, grid.row];
            

            otherBall.GetComponent<GridItem>().column += 1;
            grid.column--;

        }

        //Checking for Down Swipe
        else if (swipeAngle < -45 && swipeAngle >= -135 && grid.row > 0)
        {
            otherBall = board.allBalls[grid.column, grid.row - 1];
            
            otherBall.GetComponent<GridItem>().row += 1;
            grid.row--;
        }

       
        if (otherBall == null)
        {
          
            return;
        }

        //for updating tile names
        int otherBallCol = otherBall.GetComponent<GridItem>().column;
        int otherBallRow = otherBall.GetComponent<GridItem>().row;
        grid.UpdateTileName(grid.column, grid.row);
        otherBall.GetComponent<GridItem>().UpdateTileName(otherBallCol, otherBallRow);

        //Start moving the pieces
        StartCoroutine(LerpMovingPieces(this.gameObject, otherBall));

        // Update the board's array
        board.allBalls[grid.column, grid.row] = this.gameObject;
        board.allBalls[otherBallCol, otherBallRow] = otherBall;

        // Update tile names
        grid.UpdateTileName(grid.column, grid.row);
            


    }

    IEnumerator LerpMovingPieces(GameObject ball1, GameObject ball2)
    {
      
        Vector2 startPosition1 = ball1.transform.position;
        Vector2 targetPosition1 = ball2.transform.position;

        Vector2 startPosition2 = ball2.transform.position;
        Vector2 targetPosition2 = ball1.transform.position;

        float elapsedTime = 0f;
        float moveDuration = 0.5f;

        while (elapsedTime < moveDuration)
        {
            ball1.transform.position = Vector2.Lerp(startPosition1, targetPosition1, elapsedTime / moveDuration);
            ball2.transform.position = Vector2.Lerp(startPosition2, targetPosition2, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

          // Check for matches
        if (otherBall != null)
        {

            if (!isMatched && !otherBall.GetComponent<SwipeController>().isMatched)
            {
                // Reset elapsed time for smooth revert
                elapsedTime = 0f;


                // Lerp back to the original positions
                while (elapsedTime < moveDuration)
                {
                    ball1.transform.position = Vector2.Lerp(targetPosition1, startPosition1, elapsedTime / moveDuration);
                    ball2.transform.position = Vector2.Lerp(targetPosition2, startPosition2, elapsedTime / moveDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Ensure final positions after revert
                ball1.transform.position = startPosition1;
                ball2.transform.position = startPosition2;

                // Update grid positions
                otherBall.GetComponent<GridItem>().row = this.grid.row;
                otherBall.GetComponent<GridItem>().column = this.grid.column;
                grid.row = previousRow;
                grid.column = previousColumn;
            }

        }
      
       
    }

    private void FindMatches()
    {
        //Horizontal Matches
        if (grid.row > 0 && grid.row < height - 1)
        {
            GameObject upBall1 = board.allBalls[grid.column, grid.row + 1];
            GameObject downBall1 = board.allBalls[grid.column, grid.row - 1];

            if (upBall1.GetComponent<GridItem>().ballColor == this.grid.ballColor &&
                downBall1.GetComponent<GridItem>().ballColor == this.grid.ballColor)
            {
                upBall1.GetComponent<SwipeController>().isMatched = true;
                downBall1.GetComponent<SwipeController>().isMatched = true;
                this.isMatched = true;
            }
        }

        //Vertical Matches
        if (grid.column > 0 && grid.column < width - 1)
        {
            GameObject leftBall1 = board.allBalls[grid.column - 1, grid.row];
            GameObject rightBall1 = board.allBalls[grid.column + 1, grid.row];

            if (leftBall1.GetComponent<GridItem>().ballColor == this.grid.ballColor &&
                rightBall1.GetComponent<GridItem>().ballColor == this.grid.ballColor)
            {
                leftBall1.GetComponent<SwipeController>().isMatched = true;
                rightBall1.GetComponent<SwipeController>().isMatched = true;
                this.isMatched = true;
            }
        }
    }

    
    
}

