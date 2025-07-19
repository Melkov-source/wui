using System;
using System.Runtime.InteropServices;
using SDL2;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;

namespace WUI.Renderer;

public class RendererTest
{
    static IntPtr window;
    static IntPtr glContext;

    static int _vao, _vbo, _shaderProgram;

    public static void Main(string[] args)
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine("SDL_Init Error: " + SDL.SDL_GetError());
            return;
        }

        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
        SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);

        window = SDL.SDL_CreateWindow("SDL + OpenGL + OpenTK",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            800, 600,
            SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        if (window == IntPtr.Zero)
        {
            Console.WriteLine("Window Error: " + SDL.SDL_GetError());
            return;
        }

        glContext = SDL.SDL_GL_CreateContext(window);
        SDL.SDL_GL_MakeCurrent(window, glContext);

        // ⬅️ Правильный биндинг контекста через SDL
        GL.LoadBindings(new SDLBindingsContext());

        InitGL();

        bool running = true;
        while (running)
        {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    running = false;
            }

            Render();
            SDL.SDL_GL_SwapWindow(window);
        }

        Cleanup();
        SDL.SDL_GL_DeleteContext(glContext);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }

    static void InitGL()
    {
        float[] vertices = {
             0.0f,  0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f
        };

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        string vertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPos;
            void main() {
                gl_Position = vec4(aPos, 1.0);
            }";

        string fragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;
            void main() {
                FragColor = vec4(0.2, 1.0, 0.5, 1.0);
            }";

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        CheckCompile(vertexShader, "vertex");

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        CheckCompile(fragmentShader, "fragment");

        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);
        GL.LinkProgram(_shaderProgram);
        GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out int linked);
        if (linked == 0)
            throw new Exception("Shader link error: " + GL.GetProgramInfoLog(_shaderProgram));

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
    }

    static void Render()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    static void CheckCompile(int shader, string type)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string log = GL.GetShaderInfoLog(shader);
            throw new Exception($"{type} shader compile error:\n{log}");
        }
    }

    static void Cleanup()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteProgram(_shaderProgram);
    }
}
