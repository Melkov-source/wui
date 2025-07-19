namespace WUI.Renderer;

public static class OpenGL
{
    public const uint GL_COLOR_BUFFER_BIT = 0x00004000;

    [System.Runtime.InteropServices.DllImport("opengl32.dll")]
    public static extern void glClear(uint mask);

    [System.Runtime.InteropServices.DllImport("opengl32.dll")]
    public static extern void glClearColor(float red, float green, float blue, float alpha);
}