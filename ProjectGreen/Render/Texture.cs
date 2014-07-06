using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System;

namespace ProjectGreen
{
    class Texture
    {
        readonly int id;
        readonly int width;
        readonly int height;

        private Texture(int id, int width, int height)
        {
            this.id = id;
            this.width = width;
            this.height = height;
        }

        #region Construction
        public static Texture Create(int width, int height)
        {
            return Create(width, height, PixelType.UnsignedByte);
        }

        public static Texture Create(int width, int height, PixelType pxType)
        {
            return Create(width,
                height,
                pxType,
                PixelInternalFormat.Rgba,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra);
        }

        public static Texture Create(int width,
            int height,
            PixelType pxType,
            PixelInternalFormat intFmt,
            OpenTK.Graphics.OpenGL.PixelFormat fmt)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                intFmt,
                width,
                height,
                0,
                fmt,
                pxType,
                IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            Texture result = new Texture(id, width, height);

            return result;
        }
        #endregion

        #region IO
        // TODO - some nice explainations here
        public static Texture FromFile(string path)
        {
            using (Bitmap bmp = new Bitmap(path))
            {
                return Texture.FromBitmap(bmp);
            }
        }

        public static Texture FromBitmap(Bitmap bmp)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            Rectangle imageBounds = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(imageBounds,
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba, // internal (binary) representation of data
                bmpData.Width, // width of texture and bitmap
                bmpData.Height, // height of texture and bitmap
                0, // no border
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, // channels of pixels in bitmap
                PixelType.UnsignedByte, // storage type of pixels in bitmap
                bmpData.Scan0); // pointer to data

            bmp.UnlockBits(bmpData);

            string s = GL.GetString(StringName.Extensions);
            OpenTK.Graphics.OpenGL.ErrorCode ec = OpenTK.Graphics.OpenGL.GL.GetError();

            return new Texture(id, bmp.Width, bmp.Height);
        }

        public void Save(string filename)
        {
            int height;
            int width;

            GL.BindTexture(TextureTarget.Texture2D, this);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out height);

            Bitmap bmp = new Bitmap(width, height);

            byte[] textureBytes = new byte[width * height * 4];
            GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, textureBytes);

            Rectangle bounds = new Rectangle(0, 0, width, height);
            BitmapData data = bmp.LockBits(bounds, ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(textureBytes, 0, data.Scan0, textureBytes.Length);

            bmp.UnlockBits(data);

            bmp.Save(filename);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        #endregion

        public int Width
        { get { return width; } }

        public int Height
        { get { return height; } }

        public static implicit operator int(Texture texture)
        {
            return texture.id;
        }

        public static readonly Texture Empty = new Texture(0, 0, 0);

        static Texture black;
        public static Texture Black
        {
            get
            {
                if (black == null)
                {
                    using (Bitmap bmp = new Bitmap(1, 1))
                    {
                        using (Graphics g = Graphics.FromImage(bmp)) { g.Clear(Color.Black); }
                        black = Texture.FromBitmap(bmp);
                    }
                }
                return black;
            }
        }

        static Texture white;
        public static Texture White
        {
            get
            {
                if (white == null)
                {
                    using (Bitmap bmp = new Bitmap(1, 1))
                    {
                        using (Graphics g = Graphics.FromImage(bmp)) { g.Clear(Color.White); }
                        white = Texture.FromBitmap(bmp);
                    }
                }
                return white;
            }
        }
    }
}
