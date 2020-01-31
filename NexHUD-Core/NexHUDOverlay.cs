using NexHUDCore.NxItems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Valve.VR;

namespace NexHUDCore
{
    public class NexHudOverlay
    {

        OpenVrOverlay m_vrOverlay;
        Texture_t _textureData;
        string _cachePath;
        double _zoomLevel;
        int _windowWidth;
        int _windowHeight;
        bool _isRendering = false;
        bool _wasVisible = false;
        bool _renderVrOverlay = true;
        VREvent_t eventData = new VREvent_t();

        private Graphics m_graphics;
        private NxOverlay m_nxOverlay;
        public NxOverlay NxOverlay { get { return m_nxOverlay; } }
        public Graphics NxGraphics { get { return m_graphics; } }


        public int WindowWidth { get { return _windowWidth; } }
        public int WindowHeight { get { return _windowHeight; } }

        public Bitmap BMPTexture;

        private static Bitmap m_BMPGradient;
        private float m_GradientValue = 0.0f;

        public bool GradientIntro = true;

        private float m_wmSize = 0.3f;

        private Vector2 m_wmPosition;
        private bool m_wmDraw = true;




        #region OGL Stuff
        int _glInputTextureId = 0;
        int _glOutputTextureId = 0;
        int _glAlphaMaskTextureId = 0;
        int _glFrameBufferId = 0;
        int _glFragmentShaderProgramId = 0;
        //int _glDepthRenderBuffer = 0;
        #endregion



        bool _dirtySize = true;


        string _overlayKey;
        string _overlayName;


        /*public OpenVrOverlay vrOverlay
        {
            get { return _vrOverlay; }
        }*/

        public bool IsRendering
        {
            get { return _isRendering; }
        }

        public int GLTextureID
        {
            get { return _glInputTextureId; }
        }

        private bool m_HideRequest = false;
        public bool renderOverlay
        {
            get { return NexHudEngine.engineMode == NexHudEngineMode.Vr ? _renderVrOverlay : m_wmDraw; }
            set
            {
                if (NexHudEngine.engineMode == NexHudEngineMode.Vr)
                {
                    _renderVrOverlay = value;
                    if (m_vrOverlay != null)
                    {
                        if (_renderVrOverlay)
                        {
                            m_HideRequest = false;
                            if (!m_vrOverlay.IsVisible())
                            {
                                m_GradientValue = 0;
                                m_vrOverlay.Show();
                            }
                            //_vrOverlay.Show();
                        }
                        else
                        {
                            m_HideRequest = true;
                            //_vrOverlay.Hide();
                            //m_GradientValue = 0;
                        }
                    }
                }
                else
                {
                    m_wmDraw = value;
                }
            }
        }
        public float Alpha
        {
            get
            {
                if (m_vrOverlay != null)
                    return m_vrOverlay.Alpha;
                else
                    return m_wmDraw ? 1 : 0;
            }
            set
            {
                if (m_vrOverlay != null)
                    m_vrOverlay.Alpha = value;
            }
        }
        public string CachePath { get { return _cachePath; } set { _cachePath = value; } }


        public double ZoomLevel { get { return _zoomLevel; } set { _zoomLevel = value; } }

        public float WmSize { get => m_wmSize; set => m_wmSize = value; }
        public Vector2 WmPosition { get => m_wmPosition; set => m_wmPosition = value; }

        public NexHudOverlay(int windowWidth, int windowHeight, string overlayKey, string overlayName)
        {
            if (!NexHudEngine.Initialised)
                throw new Exception("NexHud Engine is not started");


            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _overlayKey = overlayKey;
            _overlayName = overlayName;

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            CheckGradient();

            if (NexHudEngine.engineMode == NexHudEngineMode.Vr)
                createVROverlay();
            NexHudEngine.Overlays.Add(this);

            setupOpenGl();
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            deleteGlFragProgram();
        }
        internal void deleteGlFragProgram()
        {
            if (_glFragmentShaderProgramId > 0)
            {
                try
                {
                    GL.DeleteProgram(_glFragmentShaderProgramId);
                }
                catch (Exception ex)
                {
                    NexHudEngine.Log(ex.Message);
                }
            }
        }

        public void setVRPosition(Vector3 _position, Vector3 _rotation)
        {
            if (m_vrOverlay != null)
            {
                m_vrOverlay.SetAttachment(AttachmentType.Absolute, _position, _rotation);
                m_vrOverlay.Alpha = 1;
            }
        }
        public void setWMPosition(Vector2 _position, float _size )
        {
            WmPosition = _position;
            WmSize = _size;
        }
        public void setVRWidth(float _witdh)
        {
            if (m_vrOverlay != null)
                m_vrOverlay.Width = _witdh;
        }
        private void createVROverlay(bool forcePrefix = false)
        {
            m_vrOverlay = new OpenVrOverlay( _overlayKey, _overlayName, 2.0f, true);
            m_vrOverlay.SetTextureSize(_windowWidth, _windowHeight);
            m_vrOverlay.Show();
        }

        public void Destroy()
        {
            if (m_vrOverlay != null)
                destroyVrOverlay();

            NexHudEngine.Overlays.Remove(this);

        }

        public void destroyVrOverlay()
        {
            m_vrOverlay.Destroy();
            m_vrOverlay = null;
        }



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
        protected virtual void setupOpenGl()
        {
            //nexam stuff
            CreateDefaultBitmap();

            NexHudEngine.Log("Setting up texture for " + _overlayKey);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            if (NexHudEngine.TraceLevel)
                NexHudEngine.Log("BindTexture: " + GL.GetError());

            _glInputTextureId = GL.GenTexture();

            if (NexHudEngine.TraceLevel)
                NexHudEngine.Log("GenTexture: " + GL.GetError());

            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            if (NexHudEngine.TraceLevel)
                NexHudEngine.Log("TexParameter: " + GL.GetError());

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            if (NexHudEngine.TraceLevel)
                NexHudEngine.Log("TexParameter: " + GL.GetError());

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
                NexHudEngine.Log("[OPENGL] Failed to setup frame buffer: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer).ToString());
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            if( NexHudEngine.engineMode == NexHudEngineMode.Vr)
                CompileShader("Resources.fragShader.frag");
            else
                CompileShader("Resources.fragShaderWm.frag");




            NexHudEngine.Log("Texture Setup complete for " + _overlayKey);
        }

        void CompileShader(string path)
        {
            string _fragShader = ResHelper.GetResourceText(path);

            if (_fragShader == null || _fragShader == string.Empty)
            {
                NexHudEngine.Log("[OPENGL] No Shader Found at " + path);
                return;
            }
            else
                NexHudEngine.Log("Loading Shader: " + path);

            int fragShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShaderId, _fragShader);// File.ReadAllText(path));
            GL.CompileShader(fragShaderId);

            _glFragmentShaderProgramId = GL.CreateProgram();
            GL.AttachShader(_glFragmentShaderProgramId, fragShaderId);
            GL.LinkProgram(_glFragmentShaderProgramId);

            NexHudEngine.Log("[OPENGL] Shader Result: " + GL.GetProgramInfoLog(_glFragmentShaderProgramId));

            GL.DetachShader(_glFragmentShaderProgramId, fragShaderId);
            GL.DeleteShader(fragShaderId);

            GL.Uniform1(GL.GetUniformLocation(_glFragmentShaderProgramId, "_MainTex"), 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);
        }

        internal void rebindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);
            BitmapData data = BMPTexture.LockBits(new Rectangle(0, 0, BMPTexture.Width, BMPTexture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, BMPTexture.Width, BMPTexture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            BMPTexture.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
        }
        private void openGlWindowRender()
        {
            if (!m_wmDraw)
                return;
            
            if (m_nxOverlay != null)
            {
                if (m_nxOverlay.isDirty)
                {
                    m_graphics.Clear(Color.Transparent);
                    m_nxOverlay.Render(m_graphics);

                    rebindTexture();

                }
            }
            GL.BindTexture(TextureTarget.Texture2D, _glInputTextureId);

            GL.Begin(PrimitiveType.Quads);

            float ux = 1;
            float uy = 1;
            if (BMPTexture.Width > BMPTexture.Height)
            {
                uy = (float)BMPTexture.Height / (float)BMPTexture.Width;
                uy *= (float)NexHudEngine.gameWindow.Width / (float)NexHudEngine.gameWindow.Height;
            }
            else
            {
                ux = (float)BMPTexture.Width / (float)BMPTexture.Height;
                ux *= (float)NexHudEngine.gameWindow.Height / (float)NexHudEngine.gameWindow.Width;
            }


            ux *= m_wmSize;
            uy *= m_wmSize;

            GL.TexCoord2(0, 0); GL.Vertex2(-ux + WmPosition.X,  uy + WmPosition.Y); //top left
            GL.TexCoord2(1, 0); GL.Vertex2( ux + WmPosition.X,  uy + WmPosition.Y); //top right
            GL.TexCoord2(1, 1); GL.Vertex2( ux + WmPosition.X, -uy + WmPosition.Y); //bottom right
            GL.TexCoord2(0, 1); GL.Vertex2(-ux + WmPosition.X, -uy + WmPosition.Y); //bottom left
            GL.End();

        }

        private void openGlVrRender()
        {

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
            if (m_BMPGradient != null && GradientIntro && ((m_GradientValue < 1 && !m_HideRequest) || (m_GradientValue > 0 && m_HideRequest)))
            {
                if (!m_HideRequest)
                    m_GradientValue += NexHudEngine.deltaTime * 2f;
                else
                    m_GradientValue -= NexHudEngine.deltaTime * 4f;

                if (m_GradientValue <= 0 && m_HideRequest)
                {
                    if (m_vrOverlay != null)
                        m_vrOverlay.Hide();
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
            if (NexHudEngine.engineMode == NexHudEngineMode.Vr)
            {
                if (m_vrOverlay != null && !m_vrOverlay.AttachmentSuccess)
                    return false;

                if (m_vrOverlay != null && m_vrOverlay.IsVisible())
                    return true;

                return false;
            }
            else
                return true;
        }



        public virtual void Update()
        {
            if (m_nxOverlay != null)
                m_nxOverlay.Update();

            if (!_isRendering)
                return;

            _wasVisible = true;
        }


        public virtual void Draw()
        {
            if (!CanDoUpdates())
                return;

            if (NexHudEngine.engineMode == NexHudEngineMode.Vr)
                openGlVrRender();
            else
                openGlWindowRender();


            if (m_vrOverlay != null && m_vrOverlay.IsVisible())
            {
                m_vrOverlay.SetTexture(ref _textureData);
                //m_vrOverlay.Show();
            }


        }

    }
}