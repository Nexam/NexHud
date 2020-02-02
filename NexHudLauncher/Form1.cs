using NexHUD;
using NexHUD.Settings;
using NexHUDCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Valve.VR;

namespace NexHudLauncher
{
    public partial class Form1 : Form
    {
        private bool m_isLoadingSetting = false;
        public Form1()
        {
            InitializeComponent();

            loadSettings();
        }

        private void saveSettings()
        {
            if (m_isLoadingSetting)
                return;
            switch(engineMode.SelectedIndex)
            {
                case 0: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.Auto; break;
                case 1: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.Vr; break;
                case 2: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.WindowOverlay; break;
                //case 3: NexHudSettings.get().nexHudMode = NexHudEngineMode.WindowDebug; break;
            }
            NexHudSettings.GetInstance().launchWithElite = autoLaunch.Checked;

            NexHudSettings.save();
        }
        private void loadSettings()
        {
            m_isLoadingSetting = true;
            NexHudSettings.load();
            switch (NexHudSettings.GetInstance().nexHudMode )
            {
                case NexHudEngineMode.Auto: engineMode.SelectedIndex = 0;break;
                case NexHudEngineMode.Vr: engineMode.SelectedIndex = 1; break;
                case NexHudEngineMode.WindowOverlay: engineMode.SelectedIndex = 2; break;
              //  case NexHudEngineMode.WindowDebug: engineMode.SelectedIndex = 3; break;
            }
            autoLaunch.Checked = NexHudSettings.GetInstance().launchWithElite;

            Shortcuts.loadShortcuts();
            ShortcutEntry[] shortcuts = Shortcuts.getShortcuts();
            for(int i = 0; i < shortcuts.Length; i++)
            {
                Button b = null;
                if( shortcuts[i].id == ShortcutId.menu.ToString() )
                {
                    b = btnSCmenu;
                    menuMode.SelectedIndex = shortcuts[i].holdMode ? 1 : 2;
                }
            }

            m_isLoadingSetting = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void e_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void classicToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void launchWithEliteDangerousToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            configKeybindRequest(ShortcutId.menu);
        }

        private void configKeybindRequest(ShortcutId _id)
        {
            KeybindModal _modal = new KeybindModal(this, _id);
            _modal.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void autoLaunch_CheckedChanged(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            autoLaunch.Checked = !autoLaunch.Checked;
        }

        private void autoLaunch_Click(object sender, EventArgs e)
        {
           // saveSettings();
        }
    }
}
