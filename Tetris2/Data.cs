using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Tetris2
{
    public static class Data
    {
        public static int bufferHeight = 1080;
        public static int bufferWidth = 1920;

        public static int tileSize = 42;
        public static int gameWidth = 10;
        public static int gameHeight = 22;
        public static int tileMapOffset = 20;
        public static float tileSpace = 1.14f;
        public static float tileMapLocation = Data.tileSize * Data.tileSpace;

        public static Texture2D tileTexture;

        public static float lowerTimer;
        public static Block block;
        public static Point blockSpawnOffset = new(Data.gameWidth / 2 - 1, 2);

        public static Tile[,] tileMap;

        public static int score = 0;

        private static readonly Random random = new();
        
        public static void CreateBlock()
        {
            Data.block = new Block();

            int rnd = Data.RandomNumber(0, 7);

            block.BlockLayout(rnd);
            for (int i = 0; i < block.position.Length; i++)
            {
                block.position[i] += Data.blockSpawnOffset;
            }
        }
        public static bool InBounds(int x, int y)
        {
            return 0 <= y && y < Data.tileMap.GetLength(1) && 0 <= x && x < Data.tileMap.GetLength(0);
        }

        public static void AddBlockToTileMap()
        {
            for (int i = 0; i < block.position.Length; i++)
            {
                Data.tileMap[block.position[i].X, block.position[i].Y] = new Tile { color = block.color, isSolid = true };
            }
        }

        public static int RandomNumber(int min, int max)
        {
            lock (random)
            {
                return random.Next(min, max);
            }
        }
    }
}
