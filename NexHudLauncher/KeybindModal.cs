using NexHUD;
using NexHUD.Inputs;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexHudLauncher
{
    public partial class KeybindModal : Form
    {
        private const string TITLE_RECORD = "Press key(s) for '{0}'";
        private const string TITLE_CHOICE = "key(s) chosen for '{0}'";
        private const double MOD_CHOICE_TIME = 2000;
        private Launcher m_mainForm;
        private bool m_isRecording;
        private Key[] m_modifiers = new Key[0];
        private Key? m_key = null;
        private double m_sameKeysTimer;
        private ShortcutId m_shortcut;
        public KeybindModal(Launcher _mainForm, ShortcutId _id)
        {
            InitializeComponent();
            //progressBar1.Maximum = MOD_CHOICE_TIME;            
            m_mainForm = _mainForm;
            this.FormClosing += KeybindModal_FormClosing;
            m_shortcut = _id;
            labelTitle.Text = string.Format(TITLE_RECORD, _id);
            btnAccept.Enabled = false;
            btnAgain.Enabled = false;
            startRecording();
           
        }
        public void startRecording()
        {
            labelTitle.Text = string.Format(TITLE_RECORD, m_shortcut);
            m_isRecording = true;
            timer1.Enabled = true;
            btnAgain.Enabled = false;
            btnAccept.Enabled = false;
            btnAccept.BackColor = Color.LightGray;
            m_key = null;
            m_modifiers = new Key[0];
            m_sameKeysTimer = 0;
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Marquee;

        }
        public void stopRecording()
        {
            labelTitle.Text = string.Format(TITLE_CHOICE, m_shortcut);
            m_isRecording = false;
            btnAgain.Enabled = true;
            btnAccept.Enabled = true;
            btnAccept.BackColor = Color.GreenYellow;
            timer1.Enabled = false;
            btnAccept.Enabled = true;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;// progressBar1.Maximum;
        }

        private void KeybindModal_FormClosing(object sender, FormClosingEventArgs e)
        {
        }


        private void onOpenTkUpdate(object sender, OpenTK.FrameEventArgs e)
        {
            labelKey.Text = "Key down ? "+ Keyboard.GetState().IsAnyKeyDown.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void KeybindModal_KeyDown(object sender, KeyEventArgs e)
        {
           // label3.Text = "(2)Key down ? " + Keyboard.GetState().IsAnyKeyDown.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_isRecording)
            {
                KeyboardState _keys = Keyboard.GetState();
                //Modifiers
                List<Key> list = new List<Key>();
                foreach (Key m in KeyHelper.modifiers)
                {
                    if (_keys.IsKeyDown(m) && !list.Contains(m))
                        list.Add(m);
                }
                string _textModifiers = "";
                for (int i = 0; i < list.Count; i++)
                {
                    _textModifiers += KeyHelper.keyToString(list[i]) + ((i < list.Count - 1) ? "+" : "");
                }
                labelMods.Text = _textModifiers;

                //Key
                Key? _key = null;
                int _count = Enum.GetValues(typeof(Key)).Length;
                for (int i = 0; i < _count; i++)
                {
                    if (!KeyHelper.isModifier((Key)i) && _keys.IsKeyDown((Key)i))
                        _key = (Key)i;

                }
                labelKey.Text = _key?.ToString();

                if (list.Count == m_modifiers.Length && list.Count > 0)
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;
                    bool allFound = true;
                    foreach (Key k in list)
                    {
                        if (!m_modifiers.Contains(k))
                        {
                            allFound = false;
                            break;
                        }
                    }
                    if (allFound)
                        m_sameKeysTimer += timer1.Interval;
                    else
                        m_sameKeysTimer = 0;
                }
                else
                    m_sameKeysTimer = 0;
                m_modifiers = list.ToArray();
                progressBar1.Value = 100 - (int)Math.Min( (m_sameKeysTimer/ MOD_CHOICE_TIME)*100.0, MOD_CHOICE_TIME);
                
                    ///progressBar1.Value = 100 - ;
                if ( _key != null || m_sameKeysTimer >= MOD_CHOICE_TIME)
                {
                    m_modifiers = list.ToArray();
                    m_key = _key;
                    timer1.Enabled = false;

                    stopRecording();
                }

            }
        }

        public void displayShortcut()
        {
            string _textModifiers = "";
            for (int i = 0; i < m_modifiers.Length; i++)
            {
                _textModifiers += KeyHelper.keyToString(m_modifiers[i]) + ((i < m_modifiers.Length - 1) ? "+" : "");
            }
            labelMods.Text = _textModifiers;
            labelKey.Text = m_key?.ToString();
        }

        private void btnAgain_Click(object sender, EventArgs e)
        {
            startRecording();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (m_key != null || m_modifiers.Length > 0)
            {
                Shortcuts.setShortcut(m_shortcut, m_modifiers, m_key);
            }
            Shortcuts.saveShortcuts( m_mainForm.targetForKeys == NexHUDCore.NexHudEngineMode.WindowOverlay );
            m_mainForm.loadSettings();
            Close();           
        }
    }
}
