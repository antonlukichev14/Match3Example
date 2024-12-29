using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SharpFont;
using Match3Example.Text;

namespace Match3Example
{
    class TextRender
    {
        //Все используемые символы в рендере. Необходимо указать минимум символов для экономии памяти и вычислительной мощности
        //All used characters in the render. It is necessary to specify a minimum number of characters to save memory and computational power
        private static string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдезийклмнопрстуфхцчшщъыьэюяАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789!@#$%^&*()-_=+[]{};:',.<>?/|`~";

        //Глобальная ссылка на единственный экземпляр класса
        //Global reference to the single instance of the class
        public static TextRender Instance;

        //Массив всех загруженных текстур символов
        //Array of all loaded glyph textures
        GlyphTexture[] glyphTextures;

        //Словарь, который хранит информация о том, какому символу (char) соответствует текстура в glyphTextures
        //Dictionary that stores information about which character (char) corresponds to the texture in glyphTextures
        Dictionary<char, uint> charDictionary = new Dictionary<char, uint>();

        int VAO, VBO;

        private float charDistance = 2f;
        private float defaultScale = 0.01f;

        public TextRender(string pathToFont, float charDistance)
        {
            Instance = this;

            this.charDistance = charDistance;

            Library library = new Library();
            Face face = new Face(library, pathToFont);
            face.SetPixelSizes(0, 128);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            List<GlyphTexture> glyphTexturesList = new List<GlyphTexture>();

            for (uint i = 0; i < chars.Length; i++)
            {
                GlyphTexture glyphTexture = LoadGlyphTexture(face, i);
                glyphTexturesList.Add(glyphTexture);
            }

            glyphTextures = glyphTexturesList.ToArray();

            face.Dispose();
            library.Dispose();

            InstanceVertexBufferData();
        }

        private GlyphTexture LoadGlyphTexture(Face face, uint index)
        {
            uint cCi = face.GetCharIndex(chars[(int)index]);
            charDictionary.Add(chars[(int)index], index);

            face.LoadChar(chars[(int)index], LoadFlags.Default, LoadTarget.Normal);
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

            return glyphTexture;
        }

        private void InstanceVertexBufferData()
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
            TextRenderSettings settings = new TextRenderSettings();
            settings.align = TextRenderAlign.Left;

            Render(shader, camera, text, position, scale, color, settings);
        }

        public void Render(Shader shader, Camera camera, string text, Vector2 position, float scale, Vector3 color, TextRenderSettings settings)
        {
            scale *= defaultScale;

            float alignPos = 0;

            if (settings.align == TextRenderAlign.Center)
                alignPos = AlignCenterPosition(text, position, scale);

            position.X -= alignPos / 2;

            shader.Use();
            camera.Use(shader);

            shader.SetUniformVector3("textColor", color);
            GL.ActiveTexture(0);
            GL.BindVertexArray(VAO);

            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    if (text[i] == ' ')
                    {
                        position.X += glyphTextures[0].Advance * scale + charDistance * scale;
                    }
                    else if (charDictionary.ContainsKey(text[i]))
                    {
                        RenderGlyph((int)charDictionary[text[i]], position, scale);
                        position.X += glyphTextures[(int)charDictionary[text[i]]].Advance * scale + charDistance * scale;
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

        private float AlignCenterPosition(string text, Vector2 position, float scale)
        {
            float alignPos = 0;

            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    if (text[i] == ' ')
                    {
                        alignPos += glyphTextures[0].Advance * scale + charDistance * scale;
                    }
                    else if (charDictionary.ContainsKey(text[i]))
                    {
                        alignPos += glyphTextures[(int)charDictionary[text[i]]].Advance * scale + charDistance * scale;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return alignPos;
        }

        private void RenderGlyph(int c, Vector2 position, float scale)
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
}

namespace Match3Example.Text
{
    public struct GlyphTexture
    {
        public int TextureID; // ID handle of the glyph texture
        public Vector2 Size; // Size of glyph
        public Vector2 Bearing; // Offset from baseline to left/top of glyph
        public int Advance; // Offset to advance to next glyph
    }

    public struct TextRenderSettings
    {
        public TextRenderAlign align;

        public TextRenderSettings(TextRenderAlign align)
        {
            this.align = align; 
        }
    }

    public enum TextRenderAlign
    {
        Left = 0,
        Center = 1
    }
}