using NexHUD;
using NexHUD.Settings;
using NexHUDCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Valve.VR;

namespace NexHudLauncher
{
    public partial class Launcher : Form
    {
        private bool m_isLoadingSetting = false;
        private Process m_autoLaunchedProcess = null;
        public Launcher()
        {
            InitializeComponent();

            loadSettings();

            labelVersion.Text = NexHudMain.version;
        }

        public void saveSettings()
        {
            if (m_isLoadingSetting)
                return;
            switch (engineMode.SelectedIndex)
            {
                case 0: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.Auto; break;
                case 1: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.Vr; break;
                case 2: NexHudSettings.GetInstance().nexHudMode = NexHudEngineMode.WindowOverlay; break;
                    //case 3: NexHudSettings.get().nexHudMode = NexHudEngineMode.WindowDebug; break;
            }
            NexHudSettings.GetInstance().launchWithElite = autoLaunch.Checked;

            EliteCheckTimer.Enabled = autoLaunch.Checked;

            NexHudSettings.save();
        }
        public void loadSettings()
        {
            m_isLoadingSetting = true;
            NexHudSettings.load();
            switch (NexHudSettings.GetInstance().nexHudMode)
            {
                case NexHudEngineMode.Auto: engineMode.SelectedIndex = 0; break;
                case NexHudEngineMode.Vr: engineMode.SelectedIndex = 1; break;
                case NexHudEngineMode.WindowOverlay: engineMode.SelectedIndex = 2; break;
                    //  case NexHudEngineMode.WindowDebug: engineMode.SelectedIndex = 3; break;
            }
            autoLaunch.Checked = NexHudSettings.GetInstance().launchWithElite;

            Shortcuts.loadShortcuts();
            ShortcutEntry[] shortcuts = Shortcuts.getShortcuts();
            for (int i = 0; i < shortcuts.Length; i++)
            {
                Button b = null;
                if (shortcuts[i].id == ShortcutId.menu.ToString())
                {
                    b = btnSCmenu;
                    menuMode.SelectedIndex = shortcuts[i].holdMode ? 0 : 1;
                }
                else if (shortcuts[i].id == ShortcutId.back.ToString())
                    b = btnSCback;
                else if (shortcuts[i].id == ShortcutId.select.ToString())
                    b = btnSCselect;
                else if (shortcuts[i].id == ShortcutId.up.ToString())
                    b = btnSCup;
                else if (shortcuts[i].id == ShortcutId.down.ToString())
                    b = btnSCdown;
                else if (shortcuts[i].id == ShortcutId.left.ToString())
                    b = btnSCleft;
                else if (shortcuts[i].id == ShortcutId.right.ToString())
                    b = btnSCright;

                string bText = "";
                for(int m = 0; m < shortcuts[i].OpenTkModifiers.Length; m++)
                {
                    if (m > 0)
                        bText += "+";
                    bText += KeyHelper.keyToString(shortcuts[i].OpenTkModifiers[m]);
                }
                if(shortcuts[i].OpentTkKey!= OpenTK.Input.Key.Unknown)
                    bText += (bText.Length > 0 ? "+": "") + KeyHelper.keyToString(shortcuts[i].OpentTkKey);
                if( b != null )
                    b.Text = bText;
            }

            EliteCheckTimer.Enabled = autoLaunch.Checked;
            m_isLoadingSetting = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
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
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if( sender == btnSCmenu )
                configKeybindRequest(ShortcutId.menu);
            else if (sender == btnSCback)
                configKeybindRequest(ShortcutId.back);
            else if (sender == btnSCselect)
                configKeybindRequest(ShortcutId.select);
            else if (sender == btnSCup)
                configKeybindRequest(ShortcutId.up);
            else if (sender == btnSCdown)
                configKeybindRequest(ShortcutId.down);
            else if (sender == btnSCleft)
                configKeybindRequest(ShortcutId.left);
            else if (sender == btnSCright)
                configKeybindRequest(ShortcutId.right);
        }

        private void configKeybindRequest(ShortcutId _id)
        {
            KeybindModal _modal = new KeybindModal(this, _id);
            _modal.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
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
            OpenFolder(Environment.CurrentDirectory + "\\logs\\");
        }

        private void autoLaunch_Click(object sender, EventArgs e)
        {
            // saveSettings();
        }

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            OpenFolder(Environment.CurrentDirectory + "\\config\\");
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkGithub.Text);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            launchNexHud(false);
        }
        private void launchNexHud(bool auto)
        {
            Process[] process = Process.GetProcessesByName("NexHUD");
            if (process.Length == 0 && m_autoLaunchedProcess == null)
            {
                if (auto)
                    m_autoLaunchedProcess = Process.Start(Environment.CurrentDirectory + "\\NexHud.exe");
                else
                    Process.Start(Environment.CurrentDirectory + "\\NexHud.exe");
                WindowState = FormWindowState.Minimized;
                Hide();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            launchNexHud(false);
        }

        private void EliteCheckTimer_Tick(object sender, EventArgs e)
        {
            if (NexHudSettings.GetInstance().launchWithElite)
            {
                //EDLauncher : EDLaunch.exe
                //ED: EliteDangerous64.exe
                Process[] process = Process.GetProcessesByName("EliteDangerous64");
                if (process.Length > 0)
                {
                    //Elite launched!
                    if( m_autoLaunchedProcess == null)
                        launchNexHud(true);
                }
                else if(m_autoLaunchedProcess != null)
                {
                    if( !m_autoLaunchedProcess.HasExited)
                        m_autoLaunchedProcess.Kill();
                    m_autoLaunchedProcess = null;
                }
            }

        }

        private void menuMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shortcuts.setMenuMode(menuMode.SelectedIndex == 0);
        }
    }
}
