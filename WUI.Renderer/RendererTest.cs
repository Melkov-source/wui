using static SDL2.SDL;

namespace WUI.Renderer;

public class RendererTest
{
    static IntPtr window;
    static IntPtr glContext;
    
    public static void Main(string[] args)
        {
            // Инициализация SDL с видеосистемой
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Не удалось инициализировать SDL: " + SDL_GetError());
                return;
            }

            // Устанавливаем параметры OpenGL (версия 3.2 Core)
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 2);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK,
                (int)SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);

            // Создаём окно
            window = SDL_CreateWindow("SDL2-CS UI Window",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                800, 600,
                SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine("Не удалось создать окно: " + SDL_GetError());
                return;
            }

            // Создаём OpenGL-контекст
            glContext = SDL_GL_CreateContext(window);
            SDL_GL_MakeCurrent(window, glContext);

            // Основной цикл
            bool running = true;
            SDL_Event sdlEvent;

            while (running)
            {
                // Обработка событий
                while (SDL_PollEvent(out sdlEvent) != 0)
                {
                    if (sdlEvent.type == SDL_EventType.SDL_QUIT)
                        running = false;
                }

                // Отрисовка — просто очистим экран и зальём цветом
                GL_Clear();

                SDL_GL_SwapWindow(window);
            }

            // Очистка
            SDL_GL_DeleteContext(glContext);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }

        static void GL_Clear()
        {
            // Импорт через OpenGL вызовы (будем писать вручную)
            // На этом этапе ты можешь подключить OpenGL bindings, например, OpenGL.Net или OpenTK.GL

            var color = Color.FromHex("#7FFFD4");
            
            OpenGL.glClearColor(color.R01, color.G01, color.B01, color.A01); // Цвет фона
            OpenGL.glClear(OpenGL.GL_COLOR_BUFFER_BIT);
        }
}