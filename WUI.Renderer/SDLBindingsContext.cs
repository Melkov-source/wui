using OpenTK;

namespace WUI.Renderer;

public class SDLBindingsContext : IBindingsContext
{
    public IntPtr GetProcAddress(string procName)
    {
        return SDL2.SDL.SDL_GL_GetProcAddress(procName);
    }
}