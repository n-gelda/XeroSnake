﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BusinessLayer
{
    public class AI
    {
        private Direction currentDirection;
        private int X;
        private int Y;
        Random randomNumber = new Random();
        public int XCoordinate
        {
            get
            {
                return X;
            }
        }

        public int YCoordinate
        {
            get
            {
                return Y;
            }
        }

        public AI()
        {
            X = -1;
            Y = -1;
        }

        public bool SpawnAI(int XBorder, int YBorder)
        {
            if (XBorder < 0 || YBorder < 0)
            {
                return false;
            }

            this.X = randomNumber.Next(XBorder - 1);
            this.Y = randomNumber.Next(YBorder - 1);

            return true;
        }

        public void MoveAI(int previousX, int previousY)
        {
            X = previousX;
            Y = previousY;
            bool Opposite = true;
            Direction newDirection = Direction.Right;

            while (Opposite)
            {
                newDirection = (Direction)randomNumber.Next(0,4);

                if (newDirection == Direction.Right && currentDirection == Direction.Left ||
                    newDirection == Direction.Left && currentDirection == Direction.Right ||
                    newDirection == Direction.Up && currentDirection == Direction.Down ||
                    newDirection == Direction.Down && currentDirection == Direction.Up
                    )
                {
                    Opposite = true;
                    break;
                } 
                else
                {
                    Opposite = false;
                }

            }
           

            switch (newDirection)
            {
                case Direction.Right:
                    Y = Y + 1;
                    break;
                case Direction.Left:
                    Y = Y - 1;
                    break;
                case Direction.Up:
                    X = X - 1;
                    break;
                case Direction.Down:
                    X = X + 1;
                    break;
            }

            currentDirection = newDirection;
        }
    }
}