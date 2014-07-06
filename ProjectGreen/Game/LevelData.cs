using System.Collections.Generic;
using OpenTK;
using Box2CS;

namespace ProjectGreen
{
    class LevelData
    {
        public LevelData()
        {
            this.Images = new List<BumpedSpriteDisplay>();
        }
        
        public World World { get; set; }
        public Body PlayerBody { get; set; }
        public Body ExitBody { get; set; }

        public List<BumpedSpriteDisplay> Images { get; set; }

        public string NextLevel { get; set; }
    }
}
