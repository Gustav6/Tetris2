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
        public static float tileSpace = 1.1f;
        public static float tileMapLocation = Data.tileSize * Data.tileSpace;

        public static Texture2D tileTexture;

        public static Block block;
        public static Point blockSpawnOffset = new Point(Data.gameWidth / 2 - 1, 2);

        public static Tile[,] tileMap;

        public static int score = 0;

        private static readonly Random random = new Random();

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
