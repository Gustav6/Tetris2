﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris2
{
    public class Block
    {
        public Point[] position = new Point[4];
        public Color color;

        private int RotationIndex = 0;
        private float timer = 1;
        private float moveDownTimer = 1f;
        private float addTimer = 0.5f; 

        public void Update(GameTime gameTime)
        {
            CalculateShadowPos();
            MoveBlockDown(gameTime);
            Controls();
        }

        public void BlockLayout(int whatBlock)
        {
            switch (whatBlock)
            {
                case 0: // IBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(-1, 0);
                    position[2] = new Point(1, 0);
                    position[3] = new Point(2, 0);  
                    color = Color.CornflowerBlue;
                    break; 
                case 1: // LBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(-1, 0);
                    position[2] = new Point(1, 0);
                    position[3] = new Point(1, -1);
                    color = Color.Orange;
                    break; 
                case 2: // JBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(-1, 0);
                    position[2] = new Point(-1, -1);
                    position[3] = new Point(1, 0);
                    color = Color.Blue;
                    break;
                case 3: // OBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(0, -1);
                    position[2] = new Point(1, 0);
                    position[3] = new Point(1, -1);
                    color = Color.Yellow;
                    break;
                case 4: // TBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(-1, 0);
                    position[2] = new Point(1, 0);
                    position[3] = new Point(0, -1);
                    color = Color.Purple;
                    break;
                case 5: // ZBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(1, 0);
                    position[2] = new Point(0, -1);
                    position[3] = new Point(-1, -1);
                    color = Color.Red;
                    break;
                case 6: // SBlock
                    position[0] = new Point(0, 0);
                    position[1] = new Point(-1, 0);
                    position[2] = new Point(0, -1);
                    position[3] = new Point(1, -1);
                    color = Color.LawnGreen;
                    break;
                default: break;
            }
        }

        public void Rotate()
        {
            if (color != Color.Yellow)
            {
                for (int i = 0; i < position.Length; i++)
                {
                    Point testRotation = position[i];

                    testRotation -= position[0];

                    testRotation = new Point(-testRotation.Y, testRotation.X);

                    testRotation += position[0];

                    if (testRotation.X >= Data.gameWidth && color != Color.CornflowerBlue || testRotation.X >= Data.gameWidth - 1 && color == Color.CornflowerBlue)
                    {
                        return;
                    }

                    if (testRotation.X <= -1 && color != Color.CornflowerBlue || testRotation.X <= -2 && color == Color.CornflowerBlue)
                    {
                        return;
                    }

                    if (testRotation.Y >= Data.gameHeight || testRotation.Y >= Data.gameHeight - 1 && color == Color.CornflowerBlue)
                    {
                        return;
                    }

                    if (color != Color.CornflowerBlue && Data.tileMap[testRotation.X, testRotation.Y].isSolid)
                    {
                        return;
                    }
                    
                    if (color == Color.CornflowerBlue && testRotation.X !< Data.gameWidth && testRotation.X !> -1 && Data.tileMap[testRotation.X + 1, testRotation.Y].isSolid)
                    {
                        return;
                    }
                    else if (color == Color.CornflowerBlue && testRotation.X !< Data.gameWidth && testRotation.X !> -1 && testRotation.Y < Data.gameHeight && Data.tileMap[testRotation.X, testRotation.Y + 1].isSolid)
                    {
                        return;
                    }
                }

                if (RotationIndex == 0)
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        Point point = position[i];

                        point -= position[0];

                        point = new Point(-point.Y, point.X);

                        point += position[0];

                        position[i] = point;
                    }

                    if (color == Color.CadetBlue)
                    {
                        RotationIndex++;
                    }
                }
                else if (RotationIndex == 1)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        Point point = position[i];

                        point -= position[0];

                        point = new Point(point.Y, -point.X);

                        point += position[0];

                        position[i] = point;
                    }

                    RotationIndex = 0;
                }
            }
        }

        public void Controls()
        {
            if (Input.HasBeenPressed(Keys.Left) && CanMoveLeft())
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i].X -= 1;
                }
            }

            if(Input.HasBeenPressed(Keys.Right) && CanMoveRight())
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i].X += 1;
                }
            }

            if (Input.IsPressed(Keys.Down))
            {
                moveDownTimer = 0.1f;
            }
            else 
            {
                moveDownTimer = 0.5f;
            }

            if (Input.HasBeenPressed(Keys.Up))
            {
                Rotate();
            }

            if (Input.HasBeenPressed(Keys.Space))
            {
                while (CanMoveDown())
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        position[i].Y += 1;
                    }
                }

                addTimer = 0;
            }
        }

        public void MoveBlockDown(GameTime gameTime)
        {
            if (timer <= 0 && CanMoveDown())
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i].Y += 1;
                }

                timer = moveDownTimer;
            }
            else
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public bool CanMoveDown()
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i].Y == Data.gameHeight - 1 || Data.tileMap[position[i].X, position[i].Y + 1].isSolid)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanMoveLeft()
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i].X == 0 || Data.tileMap[position[i].X - 1, position[i].Y].isSolid)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanMoveRight()
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i].X == Data.gameWidth - 1 || Data.tileMap[position[i].X + 1, position[i].Y].isSolid)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsBlockMoving(GameTime gameTime)
        {
            if (!CanMoveDown() && addTimer <= 0)
            {
                Data.lowerTimer = 0.1f;
                Data.AddBlockToTileMap();
                return false;
            }
            else if (!CanMoveDown())
            {
                addTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            return true;
        }

        public List<Point> CalculateShadowPos()
        {
            List<Point> shadowPos = new();

            for (int y = 0; y < Data.tileMap.GetLength(1); y++)
            {
                bool hasFound = false;

                for (int i = 0; i < position.Length; i++)
                {
                    Point point = position[i];

                    point -= position[0];

                    if (y == Data.tileMap.GetLength(1) - 1 - point.Y || Data.InBounds(position[i].X, y + 1 + point.Y) && Data.tileMap[position[i].X, y + 1 + point.Y].isSolid)
                    {
                        hasFound = true;
                    }
                }

                if (hasFound)
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        Point point = position[i];

                        point -= position[0];

                        Point newPoint = new(position[i].X, y);

                        newPoint.Y += point.Y;

                        shadowPos.Add(newPoint);
                    }
                    break;
                }
            }

            return shadowPos;
        }

        public void DrawShadow(SpriteBatch spriteBatch)
        {
            List<Point> temp = CalculateShadowPos();

            for (int i = 0; i < temp.Count; i++)
            {
                Vector2 pos = new((int)(temp[i].X * Data.tileMapLocation + Data.tileMapOffset), (int)(temp[i].Y * Data.tileMapLocation + Data.tileMapOffset));
                spriteBatch.Draw(Data.tileTexture, pos, Color.DarkGray * 0.5f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            for (int i = 0; i < position.Length; i++)
            {
                spriteBatch.Draw(Data.tileTexture, new Vector2((int)(position[i].X * Data.tileMapLocation + Data.tileMapOffset), (int)(position[i].Y * Data.tileMapLocation + Data.tileMapOffset)), color);
            }
        }
    }
}
