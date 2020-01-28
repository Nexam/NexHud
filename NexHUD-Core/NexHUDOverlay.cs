using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Valve.VR;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using NexHUDCore.NxItems;
using System.Diagnostics;

namespace NexHUDCore
{
    public class NexHUDOverlay
    {
        public const int SCROLL_AMOUNT_PER_SWIPE = 1500;

        Overlay _inGameOverlay;
        Texture_t _textureData;
        string _cachePath;
        double _zoomLevel;
        int _windowWidth;
        int _windowHeight;
        bool _isRendering = false;
        bool _wasVisible = false;
        bool _renderInGameOverlay = true;
        VREvent_t eventData = new VREvent_t();

        public int WindowWidth { get { return _windowWidth; } }
        public int WindowHeight { get { return _windowHeight; } }

        public Bitmap BMPTexture;

        private static Bitmap m_BMPGradient;
        private float m_GradientValue = 0.0f;

        public bool GradientIntro = true;


        #region OGL Stuff
        int _glInputTextureId = 0;
        int _glOutputTextureId = 0;
        int _glAlphaMaskTextureId = 0;
        int _glFrameBufferId = 0;
        int _glFragmentShaderProgramId = 0;
        //int _glDepthRenderBuffer = 0;
        #endregion


        public bool UpdateEveryFrame { get; set; }

        bool _dirtySize = true;


        string _overlayKey;
        string _overlayName;


        public Overlay InGameOverlay
        {
            get { return _inGameOverlay; }
        }

        public bool IsRendering
        {
            get { return _isRendering; }
        }

        public int GLTextureID
        {
            get { return _glInputTextureId; }
        }

        private bool m_HideRequest = false;
        public bool RenderInGameOverlay
        {
            get { return _renderInGameOverlay; }
            set
            {
                _renderInGameOverlay = value;
                if (InGameOverlay != null)
                {
                    if (_renderInGameOverlay)
                    {
                        m_HideRequest = false;
                        if (!InGameOverlay.IsVisible() )
                        {
                            m_GradientValue = 0;
                            InGameOverlay.Show();
                        }
                        //InGameOverlay.Show();
                    }
                    else
                    {
                        m_HideRequest = true;
                        //InGameOverlay.Hide();
                        //m_GradientValue = 0;
                    }
                }
            }
        }

        public string CachePath { get { return _cachePath; } set { _cachePath = value; } }


        public double ZoomLevel { get { return _zoomLevel; } set { _zoomLevel = value; } }


        public NexHUDOverlay(int windowWidth, int windowHeight, string overlayKey, string overlayName)
        {
            if (!SteamVR_NexHUD.Initialised)
                SteamVR_NexHUD.Init();


            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _overlayKey = overlayKey;
            _overlayName = overlayName;

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            CheckGradient();


            CreateInGameOverlay();


            SteamVR_NexHUD.Overlays.Add(this);

            SetupTextures();
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (_glFragmentShaderProgramId > 0)
                GL.DeleteProgram(_glFragmentShaderProgramId);
        }


        public void CreateInGameOverlay(bool forcePrefix = false)
        {
            _inGameOverlay = new Overlay((SteamVR_NexHUD.PrefixOverlayType || forcePrefix ? "ingame." : "") + _overlayKey, _overlayName, 2.0f, true);
            _inGameOverlay.SetTextureSize(_windowWidth, _windowHeight);
            _inGameOverlay.Show();
        }

        public void Destroy()
        {
            if (_inGameOverlay != null)
                DestroyInGameOverlay();

            SteamVR_NexHUD.Overlays.Remove(this);

        }

        public void DestroyInGameOverlay()
        {
            _inGameOverlay.Destroy();
            _inGameOverlay = null;
        }


        private Graphics m_graphics;

        private NxOverlay m_nxOverlay;

        public NxOverlay NxOverlay { get { return m_nxOverlay; } }
        public Graphics NxGraphics { get { return m_graphics; } }


        private void CheckGradient()
        {
            if (m_BMPGradient == null)
            {
                m_BMPGradient = ResHelper.GetResourceImage("Resources.Gradient.png");
            }
        }

        private void CreateDefaultBitmap()
        {
            BMPTexture = new Bitmap(_windowWidth, _windowHeight);
            m_graphics = Graphics.FromImage(BMPTexture);
            m_graphics.Clear(Color.Transparent);

            m_nxOverlay = new NxOverlay(this, true);


            m_nxOverlay.Render(m_graphics);


        }
        protected virtual void SetupTextures()
        {
            //nexam stuff
            CreateDefaultBitmap();

            SteamVR_NexHUD.Log("Setting up texture for " + _overlayKey);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            if (SteamVR_NexHUD.TraceLevel)
                SteamVR_NexHUD.Log("BindTexture: " + GL.GetError());

            _glInputTextureId = GL.GenTexture();

            if (SteamVR_NexHUD.TraceLevel)
                SteamVR_NexHUD.Log("GenTexture: " + GL.GetError());

            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            if (SteamVR_NexHUD.TraceLevel)
                SteamVR_NexHUD.Log("TexParameter: " + GL.GetError());

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            if (SteamVR_NexHUD.TraceLevel)
                SteamVR_NexHUD.Log("TexParameter: " + GL.GetError());

            _textureData = new Texture_t();
            _textureData.eColorSpace = EColorSpace.Linear;
            _textureData.eType = ETextureType.OpenGL;
            _textureData.handle = (IntPtr)_glInputTextureId;


            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            _glFrameBufferId = GL.GenFramebuffer();
            _glOutputTextureId = GL.GenTexture();
            _textureData.handle = (IntPtr)_glOutputTextureId;

            GL.BindTexture(TextureTarget.Texture2D, _glOutputTextureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _windowWidth, _windowHeight, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _glFrameBufferId);

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _glOutputTextureId, 0);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                SteamVR_NexHUD.Log("[OPENGL] Failed to setup frame buffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            if (SteamVR_NexHUD.DefaultFragmentShaderPath != null /*|| FragmentShaderPath != null*/)
            {
                SteamVR_NexHUD.Log("[OPENGL] Using Fragment Shader : "+ SteamVR_NexHUD.DefaultFragmentShaderPath);

                // string path = FragmentShaderPath != null ? FragmentShaderPath : SteamVR_NexHUD.DefaultFragmentShaderPath;
                // CompileShader(path);
                CompileShader(SteamVR_NexHUD.DefaultFragmentShaderPath);
            }


            SteamVR_NexHUD.Log("Texture Setup complete for " + _overlayKey);
        }

        void CompileShader(string path)
        {
            string _fragShader = ResHelper.GetResourceText(path);

            if (_fragShader == null || _fragShader == string.Empty)
            {
                SteamVR_NexHUD.Log("[OPENGL] No Shader Found at " + path);
                return;
            }

            int fragShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShaderId, _fragShader);// File.ReadAllText(path));
            GL.CompileShader(fragShaderId);

            _glFragmentShaderProgramId = GL.CreateProgram();
            GL.AttachShader(_glFragmentShaderProgramId, fragShaderId);
            GL.LinkProgram(_glFragmentShaderProgramId);

            SteamVR_NexHUD.Log("[OPENGL] Shader Result: " + GL.GetProgramInfoLog(_glFragmentShaderProgramId));

            GL.DetachShader(_glFragmentShaderProgramId, fragShaderId);
            GL.DeleteShader(fragShaderId);

            GL.Uniform1(GL.GetUniformLocation(_glFragmentShaderProgramId, "_MainTex"), 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);
        }



        public virtual void UpdateTexture()
        {
            if (!UpdateEveryFrame /*|| !(GradientIntro && m_GradientValue < 10)*/ )
                return;

            if (BMPTexture == null)
                return;


            if (m_nxOverlay != null)
            {
                if (m_nxOverlay.isDirty)
                {
                    m_graphics.Clear(Color.Transparent);
                    m_nxOverlay.Render(m_graphics);
                }
            }


            Bitmap _bitmap = BMPTexture;
            if (m_BMPGradient != null && GradientIntro && ( ( m_GradientValue < 1 && !m_HideRequest ) || (m_GradientValue > 0 && m_HideRequest) ) )
            {
                if (!m_HideRequest)
                    m_GradientValue += SteamVR_NexHUD.deltaTime * 2f;
                else
                    m_GradientValue -= SteamVR_NexHUD.deltaTime * 4f;

                if( m_GradientValue <= 0 && m_HideRequest)
                {
                    InGameOverlay.Hide();
                    return;
                }

                if (m_BMPGradient.Width != _bitmap.Width || m_BMPGradient.Height != _bitmap.Height)
                {
                    m_BMPGradient = new Bitmap(m_BMPGradient, new Size(_bitmap.Width, _bitmap.Height));

                    BitmapData alphaMapData = m_BMPGradient.LockBits(
                        new Rectangle(0, 0, m_BMPGradient.Width, m_BMPGradient.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    _glAlphaMaskTextureId = GL.GenTexture();

                    GL.BindTexture(TextureTarget.Texture2D, _glOutputTextureId);
                    GL.BindTexture(TextureTarget.Texture2D, _glAlphaMaskTextureId);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_BMPGradient.Width, m_BMPGradient.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, alphaMapData.Scan0);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    m_BMPGradient.UnlockBits(alphaMapData);

                    if (_glFragmentShaderProgramId > 0)
                    {
                        GL.ActiveTexture(TextureUnit.Texture1);
                        GL.BindTexture(TextureTarget.Texture2D, _glAlphaMaskTextureId);
                        GL.ActiveTexture(TextureUnit.Texture0); // Reset
                    }

                }
            }
            /* if (GradientIntro && m_GradientValue < 10)
             {
                 Stopwatch _watch = new Stopwatch();
                 _watch.Start();
                 Bitmap _alpha = new Bitmap(m_BMPGradient, new Size(_bitmap.Width, _bitmap.Height));
                 float _alphaValue = 0;
                 Color _color;
                 for (int x = 0; x < _bitmap.Width; x++)
                 {
                     for (int y = 0; y < _bitmap.Height; y++)
                     {
                         _color = _bitmap.GetPixel(x, y);
                         _alphaValue = ((float)_alpha.GetPixel(x, y).R)/255f;
                         if (_alphaValue >= (1 - (m_GradientValue * .1f)))
                             _alphaValue = 1;
                         else
                             _alphaValue = 0;
                         _alphaValue = _color.A * _alphaValue;
                         if (_alphaValue < 0)
                             _alphaValue = 0;
                         if (_alphaValue > 255)
                             _alphaValue = 255;

                        // _color = Color.FromArgb((int)_alphaValue, _color);
                       //  _bitmap.SetPixel(x, y, _color);
                     }
                 }
                 m_GradientValue += SteamVR_NexHUD.deltaTime;
                 _watch.Stop();
                 Console.WriteLine(this._overlayName + " Gradient intro operation. m_GradientValue=" + m_GradientValue + "  || Time operation: " + _watch.ElapsedMilliseconds + "ms");
             }
             */

            BitmapData bmpData = _bitmap.LockBits(
                new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);

            if (_dirtySize)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _bitmap.Width, _bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
                _dirtySize = false;
            }
            else
            {
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, _bitmap.Width, _bitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _glFrameBufferId);
            GL.Viewport(0, 0, _bitmap.Width, _bitmap.Height);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (_glAlphaMaskTextureId > 0 && _glFragmentShaderProgramId > 0)
            {
                GL.UseProgram(_glFragmentShaderProgramId);
                GL.Uniform1(GL.GetUniformLocation(_glFragmentShaderProgramId, "_AlphaMap"), 1);
                GL.Uniform2(GL.GetUniformLocation(_glFragmentShaderProgramId, "_OutputSize"), (float)_windowWidth, (float)_windowHeight);
                GL.Uniform1(GL.GetUniformLocation(_glFragmentShaderProgramId, "_blend"), 1 - (m_GradientValue));
            }

            DrawQuad();

            if (_glAlphaMaskTextureId > 0 && _glFragmentShaderProgramId == 0)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFuncSeparate(BlendingFactorSrc.Zero, BlendingFactorDest.One, BlendingFactorSrc.One, BlendingFactorDest.Zero);

                GL.BindTexture(TextureTarget.Texture2D, _glAlphaMaskTextureId);

                DrawQuad();

                //GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.Zero
            }

            GL.Disable(EnableCap.Blend);


            _bitmap.UnlockBits(bmpData);

            GL.BindTexture(TextureTarget.Texture2D, 0);

        }


        void DrawQuad()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1); GL.Vertex2(-1, 1); // Top Left
            GL.TexCoord2(1, 1); GL.Vertex2(1, 1); // Top Right
            GL.TexCoord2(1, 0); GL.Vertex2(1, -1); // Bottom Right
            GL.TexCoord2(0, 0); GL.Vertex2(-1, -1); // Bottom Left
            GL.End();
        }


        bool CanDoUpdates()
        {
            //This prevents Draw() from failing on get of bitmap when attachment is delayed for controllers
            if (InGameOverlay != null && !InGameOverlay.AttachmentSuccess)
                return false;

            if (InGameOverlay != null && InGameOverlay.IsVisible())
                return true;

            return false;
        }



        public virtual void Update()
        {

            if (m_nxOverlay != null)
                m_nxOverlay.Update();

            if (!_isRendering)
                return;

            // Mouse inputs are for dashboards only right now.

            if (InGameOverlay != null)
            {
                while (InGameOverlay.PollEvent(ref eventData))
                {
                    EVREventType type = (EVREventType)eventData.eventType;

                    // HandleEvent(type, eventData);
                }
            }


            _wasVisible = true;


        }


        public virtual void Draw()
        {
            if (!CanDoUpdates())
                return;


            UpdateTexture();


            if (InGameOverlay != null && InGameOverlay.IsVisible())
            {
                InGameOverlay.SetTexture(ref _textureData);
                //InGameOverlay.Show();
            }


        }

    }
}