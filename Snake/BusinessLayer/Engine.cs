﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Engine
    {
        private const int snakeInitialLength = 4;
        private const int step = 1;
        private const int snakeHitsMaze = 5;
        private int mazeLength { get; set; }
        private int mazeWidth { get; set; }
        private int[,] mazeArray { get; set; }

        private maze gameMaze;

        private GameSnake gameSnake1;
        // For future use, 2 player game mode
        //private GameSnake gameSnake2;
        private List<Food> foodList = new List<Food>();


        private enum gameMode
        {
            basic,
        }
        public enum snakeAction
        {
            eat,
            die,
            move
        }
        

        gameMode currentMode = gameMode.basic;


        public Engine(int length = 20,  int width = 70,  int mode = 1)
        {
            mazeLength = length;
            mazeWidth = width;
            Score.resetScore();
        }

  

        public int[,] initializeGame()
        {

            switch (currentMode)
            {

                case gameMode.basic:


                    // Create a New Maze and initialize it
                    gameMaze = new maze(mazeWidth, mazeLength);
                    mazeArray = gameMaze.CreateMaze();

                    // Add the Snake
                    gameSnake1 = new GameSnake();
                    //List<Point> snakeBody = new List<Point>();
                    List<Point> snakeCurrentBody = gameSnake1.createFirstSnake(mazeLength, mazeWidth, snakeInitialLength);

                    // Make the whole snake as body first
                    foreach (Point value in snakeCurrentBody)
                    {
                        mazeArray[value.returnX(), value.returnY()] = (int)Elements.snakeBody;
                    }
                    // Identify snake head
                    Point head = snakeCurrentBody[0];
                    mazeArray[head.returnX(), head.returnY()] = (int)Elements.snakeHead;

                    // Add the Food
                    foodList.Add(new Food());

                    foreach (Food value in foodList)
                    {
                        bool isValid = true;
                        do
                        {
                            value.generateFood(mazeLength, mazeWidth);
                            isValid = validateNewFoodLocation(value);
                        } while (!isValid);
                        mazeArray[value.getXLocation(), value.getyLocation()] = (int)Elements.food;
                    }

                    break;

                default:
                    // Invalid gameMode
                    throw new System.Exception("Invalid Game Mode!");
            }

            return mazeArray;
        }

        public int[,] updateGame(Direction snakeDirection)
        {
            if (snakeDirection == Direction.Unchanged)
            {
                snakeDirection = gameSnake1.directionFacing;
            }

            Point newSnakeHead = getNewHead(snakeDirection);
            List<Point> snakesNewLocation;
            List<Point> SnakeCurrentPosition;
            switch (mazeArray[newSnakeHead.returnX(), newSnakeHead.returnY()])
            {

                case (int)Elements.mazeBody: 
                    if (Score.getScore() > Score.getHighScore())
                    {
                        Score.setHighScore(Score.getScore());
                    }
                    mazeArray[0, 0] = snakeHitsMaze;
                    return mazeArray;


                case (int)Elements.food:
                    SnakeCurrentPosition = gameSnake1.returnCurrentSnakePosition();
                    //mazeArray[SnakeCurrentPosition.Last().returnX(), SnakeCurrentPosition.Last().returnY()] = (int)Elements.blank;                  
                    mazeArray[SnakeCurrentPosition.First().returnX(), SnakeCurrentPosition.First().returnY()] = (int)Elements.snakeBody;
                    snakesNewLocation = gameSnake1.snakeMove(snakeDirection, true);        
                    mazeArray[snakesNewLocation.First().returnX(), snakesNewLocation.First().returnY()] = (int)Elements.snakeHead;

                    int foodToRemove = 0;
                    foreach (Food value in foodList)
                    {
                        if ((newSnakeHead.returnX() == value.getXLocation()) && (newSnakeHead.returnY() == value.getyLocation()))
                        {
                            Score.incrementScore(value.getFoodType());
                            foodToRemove = foodList.IndexOf(value);
                        }
                    }
                    foodList.RemoveAt(foodToRemove);



                    Food newFood = new Food();
                    bool isValid = true;
                    do
                    {
                        newFood.generateFood(mazeLength, mazeWidth);
                        isValid = validateNewFoodLocation(newFood);
                        if (isValid)
                        {
                            foodList.Add(newFood);
                            mazeArray[newFood.getXLocation(), newFood.getyLocation()] = (int)Elements.food;
                        }
                    } while (!isValid);


                    break;

                default:   // snake moves
                    SnakeCurrentPosition = gameSnake1.returnCurrentSnakePosition();
                    mazeArray[SnakeCurrentPosition.Last().returnX(), SnakeCurrentPosition.Last().returnY()] = (int)Elements.blank;
                    mazeArray[SnakeCurrentPosition.First().returnX(), SnakeCurrentPosition.First().returnY()] = (int)Elements.snakeBody;
                    snakesNewLocation = gameSnake1.snakeMove(snakeDirection, false);
                    mazeArray[snakesNewLocation.First().returnX(), snakesNewLocation.First().returnY()] = (int)Elements.snakeHead;

                    foreach (Food value in foodList)
                    {
                        mazeArray[value.getXLocation(), value.getyLocation()] = (int)Elements.food;
                    }


                    break;
            }
            return mazeArray;
        }

        private Point getNewHead(Direction snakeDirection)
        {
            List<Point> snakeBody = gameSnake1.returnCurrentSnakePosition();

            // Check current snake head location.
            Point snakeHead = snakeBody.First();

            // Cross check with new snake head location.
            Point newSnakeHead;
            int x = snakeHead.returnX();
            int y = snakeHead.returnY();
            switch (snakeDirection)
            {

                case Direction.Right:
                    newSnakeHead = new Point(x, y + step);
                    break;

                case Direction.Left:
                    newSnakeHead = new Point(x, y - step);
                    break;

                case Direction.Up:
                    newSnakeHead = new Point(x - step, y);
                    break;

                case Direction.Down:
                    newSnakeHead = new Point(x + step, y);
                    break;

                default:
                    throw new System.Exception("Invalid direction.");
            }
            return newSnakeHead;
        }
        public bool validateNewFoodLocation(Food newFood)
        {
            int x = newFood.getXLocation();
            int y = newFood.getyLocation();

            if ((x >= mazeLength) || (y >= mazeWidth))
            {
                return false;
            }
            if (mazeArray[x, y] == (int)Elements.mazeBody)
            {
                return false;
            }
            if (mazeArray[x, y] == (int)Elements.snakeBody)
            {
                return false;
            }
            if (mazeArray[x, y] == (int)Elements.snakeHead)
            {
                return false;
            }
            return true;
        }
    }
}