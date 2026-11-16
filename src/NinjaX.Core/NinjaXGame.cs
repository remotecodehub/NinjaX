using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NinjaX.Core.Input;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input.Touch; 
using Microsoft.Xna.Framework.Content;
using System.Reflection;

namespace NinjaX.Core;
public class NinjaXGame : Game
{
    #region Private Fields

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private OrthographicCamera _camera;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private List<IInput> inputs;
    private SpriteFont _font;
    private Vector2 _cameraPosition;
    private VirtualInput virtualInput;
    #endregion

    public NinjaXGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        //#if ANDROID || IOS || WINRT || WINDOWS_PHONE
        //        // not sure if this is needed at all
        //        _graphics.IsFullScreen = true;
        //        _graphics.PreferredBackBufferWidth = 800;
        //        _graphics.PreferredBackBufferHeight = 480;
        //        _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        //#endif
    }

    #region Methods
    private List<IInput> GetPlatformInputs()
    {
        //virtualInput = new VirtualInput(TouchPanel.DisplayWidth, TouchPanel.DisplayHeight, _graphics.GraphicsDevice);
        virtualInput = new VirtualInput(TouchPanel.DisplayWidth/3, TouchPanel.DisplayHeight/3, _graphics.GraphicsDevice);
        var result = new List<IInput>();
#if WINDOWS || LINUX
            result.Add(new KeyboardInput());
            result.Add(new GamepadInput());
#endif
#if ANDROID || IOS || WINRT || WINDOWS_PHONE
        result.Add(virtualInput);
#endif
        return result;
    }

    private SpriteFont LoadFont(ContentManager content)
    {
#if ANDROID || IOS || WINRT || WINDOWS_PHONE
        var fontName = "fonts\\Pinkend";
#else
            var fontName = "font1";
#endif
        return content.Load<SpriteFont>(fontName);
    }

    private ActionRequest Merge(IEnumerable<ActionRequest> actions)
    {
        var merged = new ActionRequest
        {
            Left = 0,
            Right = 0,
            Stop = false,
            Activate = false,
            Exit = false
        };
        if (actions.All(a => a.Left == 0))
            merged.Left = 0;
        else
            merged.Left = actions.Select(a => a.Left).FirstOrDefault(x => x != 0);
        if (actions.All(a => a.Right == 0))
            merged.Right = 0;
        else
            merged.Right = actions.Select(a => a.Right).FirstOrDefault(x => x != 0);
        merged.Stop = actions.Any(a => a.Stop);
        merged.Activate = actions.Any(a => a.Activate);
        merged.Exit = actions.Any(a => a.Exit);

        return merged;
    }

    private Vector2 GetMovementDirection(VirtualGamepad gp)
    {
        var movementDirection = Vector2.Zero;
        var isLeft = gp.left.StartLocation.X > gp.left.StickPos.X;
        var isUp = gp.left.StartLocation.Y > gp.left.StickPos.Y;
        if (isUp && (gp.left.StartLocation.Y != gp.left.StickPos.Y))
        {
            movementDirection -= Vector2.UnitY;
        }
        if ((gp.left.StartLocation.Y != gp.left.StickPos.Y))
        {
            movementDirection += Vector2.UnitY;
        }
        if (isLeft && (gp.left.StartLocation.X != gp.left.StickPos.X))
        {
            movementDirection -= Vector2.UnitX;
        }
        if (!isLeft && gp.left.StartLocation.X != gp.left.StickPos.X)
        {
            movementDirection += Vector2.UnitX;
        }

        // Can't normalize the zero vector so test for it before normalizing
        if (movementDirection != Vector2.Zero)
        {
            movementDirection.Normalize();
        }

        return movementDirection;

    }

    private void MoveCamera(GameTime gameTime, int move = 0)
    {
        var speed = 200;
        var seconds = gameTime.GetElapsedSeconds();
        var movementDirection = GetMovementDirection(virtualInput!.Gamepad!);
        if (move == 1)
            _cameraPosition += speed * movementDirection * seconds * move;
    }
    #endregion

    #region Overrides

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
        _camera = new OrthographicCamera(viewportadapter);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = LoadFont(Content);
        Drawing.font = _font;  // i am sorry for using global variables
        inputs = GetPlatformInputs();

        // TODO: use this.Content to load your game content here
        _tiledMap = Content.Load<TiledMap>("1");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

    }

    protected override void Update(GameTime gameTime)
    {
        var actions = inputs.Select(i => i.Update(gameTime).Read()).ToList();
        var merged = Merge(actions);
        if (merged.Exit)
            Exit();

        // TODO: Add your update logic here
        _tiledMapRenderer.Update(gameTime);

        MoveCamera(gameTime);
        //_camera.LookAt(_cameraPosition);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _tiledMapRenderer.Draw(/*_camera.GetViewMatrix()*/);
        _spriteBatch.Begin();
        var screenHeight = GraphicsDevice.Viewport.Height;
        var screenWidth = GraphicsDevice.Viewport.Width;
        var captionOrigin = new Vector2(screenWidth / 2.0f, screenHeight / 10.0f);
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        Drawing.DrawStringCentered($"NinjaX game {version}", captionOrigin, Color.Black, _spriteBatch);
        //_spriteBatch.DrawString(_font, $"Status: {botConrol.Status}", new Vector2(40, screenHeight / 10.0f * 2), Color.LightGray);
        _spriteBatch.DrawString(_font, $"Datetime: {DateTime.Now.ToString("HH:mm:ss")}", new Vector2(40, screenHeight / 10.0f * 3), Color.LightGray);
        inputs.Select(i => i.Draw(_spriteBatch)).ToList();
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    #endregion


}
