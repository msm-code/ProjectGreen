using System.IO;

namespace ProjectGreen
{
    interface IContentDecoder
    {
        object Decode(StreamReader data);
    }
}
