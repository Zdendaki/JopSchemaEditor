using JopSchemaEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;
using System.Windows.Input;

namespace MonoGame.WpfCore.MonoGameControls;

public interface IMonoGameViewModel : IDisposable
{
    IGraphicsDeviceService GraphicsDeviceService { get; set; }

    void Initialize();

    void LoadContent();

    void UnloadContent();

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);

    void AfterRender();

    void OnActivated(object sender, EventArgs args);

    void OnDeactivated(object sender, EventArgs args);

    void OnExiting(object sender, EventArgs args);

    void OnMouseDown(MouseStateArgs mouseState);

    void OnMouseMove(MouseStateArgs mouseState);

    void OnMouseUp(MouseStateArgs mouseState);

    void OnMouseWheel(MouseStateArgs args, int delta);

    void OnKeyDown(KeyEventArgs e);

    void OnKeyUp(KeyEventArgs e);

    void OnDrop(DragStateArgs dragState);

    void SizeChanged(object sender, SizeChangedEventArgs args);
}

public class MonoGameViewModel : ViewModel, IMonoGameViewModel
{
    public MonoGameViewModel()
    {
    }

    public void Dispose()
    {
        Content?.Dispose();
        GC.SuppressFinalize(this);
    }

    public IGraphicsDeviceService GraphicsDeviceService { get; set; } = default!;

    protected GraphicsDevice GraphicsDevice => GraphicsDeviceService?.GraphicsDevice!;

    protected MonoGameServiceProvider Services { get; private set; } = default!;

    protected ContentManager Content { get; set; } = default!;

    protected List<IGameComponent> Components { get; } = new();

    public virtual void Initialize()
    {
        Services = new MonoGameServiceProvider();
        Services.AddService(GraphicsDeviceService);
        Content = new ContentManager(Services) { RootDirectory = "Content" };
    }

    protected void PostInitialize()
    {
        foreach (var component in Components)
            component.Initialize();
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent() { }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in Components)
            if (component is IUpdateable updateable && updateable.Enabled)
                updateable.Update(gameTime);
    }


    public virtual bool BeginDraw() => true;

    public virtual void EndDraw() { }

    public virtual void Draw(GameTime gameTime) { }

    void IMonoGameViewModel.Draw(GameTime gameTime)
    {
        if (BeginDraw())
        {
            foreach (var component in Components)
                if (component is IDrawable drawable && drawable.Visible)
                    drawable.Draw(gameTime);
            Draw(gameTime);
            EndDraw();
        }
    }

    public virtual void AfterRender() { }

    public virtual void OnActivated(object sender, EventArgs args) { }

    public virtual void OnDeactivated(object sender, EventArgs args) { }

    public virtual void OnExiting(object sender, EventArgs args) { }

    public virtual void OnMouseDown(MouseStateArgs mouseState) { }

    public virtual void OnMouseMove(MouseStateArgs mouseState) { }

    public virtual void OnMouseUp(MouseStateArgs mouseState) { }

    public virtual void OnMouseWheel(MouseStateArgs args, int delta) { }

    public virtual void OnKeyDown(KeyEventArgs e) { }

    public virtual void OnKeyUp(KeyEventArgs e) { }

    public virtual void OnDrop(DragStateArgs dragState) { }

    public virtual void SizeChanged(object sender, SizeChangedEventArgs args) { }
}
