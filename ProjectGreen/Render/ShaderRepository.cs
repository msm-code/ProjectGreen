using ProjectGreen.Render.Shaders;
using System.Collections.Generic;

namespace ProjectGreen.Render
{
    class ShaderRepository
    {
        public const string TexturedDraw = "texture-draw-prog";

        static Dictionary<string, ShaderProgram> programs = new Dictionary<string, ShaderProgram>();

        public static void Add(string id, ShaderProgram program)
        {
            programs.Add(id, program);
        }

        public static ShaderProgram Get(string id)
        {
            return programs[id];
        }
    }
}
