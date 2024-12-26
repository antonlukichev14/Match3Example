using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFont;

namespace Match3Example
{
    class TextRender
    {
        public static TextRender Instance;
        Dictionary<char, uint> charDictionary = new Dictionary<char, uint>();

        GlyphTexture[] glyphTextures;
        int VAO, VBO;

        public TextRender(string pathToFont, string chars)
        {
            Instance = this;

            Library library = new Library();
            Face face = new Face(library, pathToFont);
            face.SetPixelSizes(0, 128);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            List<GlyphTexture> glyphTexturesList = new List<GlyphTexture>();

            for (uint i = 0; i < chars.Length; i++)
            {
                uint cCi = face.GetCharIndex(chars[(int)i]);
                charDictionary.Add(chars[(int)i], i);

                face.LoadChar(chars[(int)i], LoadFlags.Default, LoadTarget.Normal);
                face.Glyph.RenderGlyph(RenderMode.Normal);
                byte[] a = face.Glyph.Bitmap.BufferData;

                int texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, face.Glyph.Bitmap.Width, face.Glyph.Bitmap.Rows, 0, PixelFormat.Red, PixelType.UnsignedByte, a);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMinFilter.Linear);

                GlyphTexture glyphTexture = new GlyphTexture();
                glyphTexture.TextureID = texture;
                glyphTexture.Size = new Vector2(face.Glyph.Bitmap.Width, face.Glyph.Bitmap.Rows);
                glyphTexture.Bearing = new Vector2(face.Glyph.BitmapLeft, face.Glyph.BitmapTop);
                glyphTexture.Advance = (int)face.Glyph.Advance.X;

                glyphTexturesList.Add(glyphTexture);
            }

            glyphTextures = glyphTexturesList.ToArray();

            face.Dispose();
            library.Dispose();

            InstanceVertexBufferData();
        }

        public void InstanceVertexBufferData()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, 0, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Render(Shader shader, Camera camera, string text, Vector2 position, float scale, Vector3 color)
        {
            shader.Use();
            camera.Use(shader);

            shader.SetUniformVector3("textColor", color);
            GL.ActiveTexture(0);
            GL.BindVertexArray(VAO);

            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    if (charDictionary.ContainsKey(text[i]))
                    {
                        RenderGlyph((int)charDictionary[text[i]], position, scale);
                        position.X += glyphTextures[(int)charDictionary[text[i]]].Advance * scale;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        void RenderGlyph(int c, Vector2 position, float scale)
        {
            GlyphTexture ch = glyphTextures[c];

            float xpos = position.X + ch.Bearing.X * scale;
            float ypos = position.Y - (ch.Size.Y - ch.Bearing.Y) * scale;

            float w = ch.Size.X * scale;
            float h = ch.Size.Y * scale;

            float[,] vertices = new float[6, 4] {
                { xpos,     ypos + h,   0.0f, 0.0f },            
            { xpos,     ypos,       0.0f, 1.0f },
            { xpos + w, ypos,       1.0f, 1.0f },

            { xpos,     ypos + h,   0.0f, 0.0f },
            { xpos + w, ypos,       1.0f, 1.0f },
            { xpos + w, ypos + h,   1.0f, 0.0f }
            };

            GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * vertices.Length, vertices);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }

    struct GlyphTexture
    {
        public int TextureID; // ID handle of the glyph texture
        public Vector2 Size; // Size of glyph
        public Vector2 Bearing; // Offset from baseline to left/top of glyph
        public int Advance; // Offset to advance to next glyph
    }
}
