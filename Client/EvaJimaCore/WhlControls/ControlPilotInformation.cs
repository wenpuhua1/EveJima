﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using EvaJimaCore;
using EveJimaCore.API;


namespace EveJimaCore.WhlControls
{
    public partial class ControlPilotInformation : BaseContainer
    {
        public ControlPilotInformation()
        {
            InitializeComponent();

            label16.Text = Localization.Messages.Get("Tab_PilotInfo_Pilots");
            label14.Text = Localization.Messages.Get("Tab_PilotInfo_SelectedPilot");
            label17.Text = Localization.Messages.Get("Tab_PilotInfo_History");
            cmdCopyPilotsFromClipboard.Value = Localization.Messages.Get("Tab_PilotInfo_CopyPilotsFromClipboard");
            cmdClearHistory.Value = Localization.Messages.Get("Tab_PilotInfo_ClearHistory");
        }

        public override void ActivateContainer()
        {
            EventNavigateInternalBrowser(Global.ApplicationSettings.Common.StatisticVisitorsCounterPage);
        }

        [Description("Pilot name"), Category("Data")]
        public string PilotName
        {
            get { return txtSelectedPilotName.Text; }
            set
            {
                txtSelectedPilotName.Text = value;

                Refresh();
            }
        }

        private void txtSelectedPilotName_TextChanged(object sender, EventArgs e)
        {
            if( string.IsNullOrEmpty( txtSelectedPilotName.Text.Trim() ))
            {
                cmdShowZkillboard.IsActive = false;
            }
             else
            {
                cmdShowZkillboard.IsActive = true;
            }
        }

        private void cmdCopyPilotsFromClipboard_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            var txtInClip = Clipboard.GetText();

            if (string.IsNullOrEmpty(txtInClip))
            {
                return;
            }

            string[] pilots;

            pilots = txtInClip.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (var pilot in pilots)
            {
                listBox1.Items.Add(pilot);
            }

            Refresh();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            txtSelectedPilotName.Text = listBox1.Text;
        }

        private void cmdShowZkillboard_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSelectedPilotName.Text.Trim()))
            {
                MessageBox.Show(Localization.Messages.Get("Tab_PilotInfo_EnterPilotName"));
                return;
            }

            var url = Zkillboard.GetZkillboardUrlByName(txtSelectedPilotName.Text.Trim());

            if(string.IsNullOrEmpty(url))
            {
                MessageBox.Show(Localization.Messages.Get("Tab_PilotInfo_PilotNotFound"));
                return;
            }

            Global.InternalBrowser.OnBrowserNavigate(url);

            if (crlPilotsHistory.Items.Contains(txtSelectedPilotName.Text.Trim()) == false)
            {
                crlPilotsHistory.Items.Add(txtSelectedPilotName.Text.Trim());
            }
        }

        private void cmdClearHistory_Click(object sender, EventArgs e)
        {
            crlPilotsHistory.Items.Clear();
        }

        private void Event_PilotsHistoryClick(object sender, EventArgs e)
        {
            if (crlPilotsHistory.SelectedItem == null) return;

            if (crlPilotsHistory.SelectedItem.ToString() != String.Empty)
            {
                txtSelectedPilotName.Text = crlPilotsHistory.SelectedItem.ToString();
            }
        }
    }
}
