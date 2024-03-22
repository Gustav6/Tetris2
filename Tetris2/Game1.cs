using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Tetris2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool canClearRow;
        private int howManyRowsCleard;
        private SpriteFont font;
        private Texture2D endScreanTileTexture;
        private bool pause;

        public Game1()
        {
            _graphics = new(this);
            _graphics.PreferredBackBufferHeight = Data.bufferHeight;
            _graphics.PreferredBackBufferWidth = Data.bufferWidth;
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Data.tileMap = new Tile[Data.gameWidth, Data.gameHeight];
            font = Content.Load<SpriteFont>("Font");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            endScreanTileTexture = new Texture2D(GraphicsDevice, Data.bufferWidth, Data.bufferHeight);
            Color[] colorArray2 = new Color[Data.bufferWidth * Data.bufferHeight];
            for (int i = 0; i < colorArray2.Length; i++)
            {
                colorArray2[i] = Color.Black;
            }
            endScreanTileTexture.SetData(colorArray2);

            Data.tileTexture = new Texture2D(GraphicsDevice, Data.tileSize, Data.tileSize);
            Color[] colorArray = new Color[Data.tileSize * Data.tileSize];
            for (int i = 0; i < colorArray.Length; i++)
            {
                colorArray[i] = Color.White;
            }
            Data.tileTexture.SetData<Color>(colorArray);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.GetStateCall();

            if (Input.HasBeenPressed(Keys.Tab))
            {
                pause = !pause;
            }

            if (Data.block != null && !pause)
            {
                Data.block.Update(gameTime);

                if (!Data.block.IsBlockMoving(gameTime))
                {
                    Data.block = null;
                }
            }
            else if (Data.block == null && !GameOver() && !pause)
            {
                CheckTileMap(); 
                Data.CreateBlock();
            }

            base.Update(gameTime);
        }

        public bool GameOver()
        {
            for (int y = 0; y < 4; y++)
            {
                if (Data.tileMap[Data.blockSpawnOffset.X, y].isSolid)
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckTileMap()
        {
            for (int y = Data.gameHeight - 1; y >= 0; y--)
            {
                canClearRow = true;

                for (int x = 0; x < Data.gameWidth; x++)
                {
                    if (Data.tileMap[x, y].isSolid == false)
                    {
                        canClearRow = false;
                    }
                }

                if (howManyRowsCleard != 0 && y != 0)
                {
                    for (int x = 0; x < Data.gameWidth; x++)
                    {
                        Data.tileMap[x, y + howManyRowsCleard] = Data.tileMap[x, y];
                    }
                }

                if (canClearRow)
                {
                    for (int x = 0; x < Data.gameWidth; x++)
                    {
                        Data.tileMap[x, y].isSolid = false;
                    }

                    howManyRowsCleard++;
                }
            }

            if (howManyRowsCleard == 1)
            {
                Data.score += 100;
            }
            else if (howManyRowsCleard == 2)
            {
                Data.score += 300;
            }
            else if (howManyRowsCleard == 3)
            {
                Data.score += 500;
            }
            else if (howManyRowsCleard == 4)
            {
                Data.score += 800;
            }

            howManyRowsCleard = 0;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, "Score: " + Data.score.ToString(), new Vector2(500, 25), Color.LawnGreen, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            for (int x = 0; x < Data.gameWidth; x++)
            {
                for (int y = 0; y < Data.gameHeight; y++)
                {
                    _spriteBatch.Draw(Data.tileTexture, new Vector2((int)(x * Data.tileMapLocation + Data.tileMapOffset), (int)(y * Data.tileMapLocation + Data.tileMapOffset)), Data.tileMap[x, y].isSolid ? Data.tileMap[x, y].color : Color.DarkSlateGray);
                }
            }

            if (Data.block != null)
            {
                Data.block.Draw(_spriteBatch);
            }

            if (GameOver())
            {
                _spriteBatch.Draw(endScreanTileTexture, Vector2.Zero, Color.Black * 0.9f);
                _spriteBatch.DrawString(font, "Game Over", new Vector2(Data.bufferWidth / 2 - 150, Data.bufferHeight / 2 - 50), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            else if (pause)
            {
                _spriteBatch.Draw(endScreanTileTexture, Vector2.Zero, Color.Black * 0.9f);
                _spriteBatch.DrawString(font, "Paused", new Vector2(Data.bufferWidth / 2 - 100, Data.bufferHeight / 2 - 50), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}