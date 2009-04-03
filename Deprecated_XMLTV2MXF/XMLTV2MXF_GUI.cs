using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;

namespace XMLTV2MXF
{
    public partial class XMLTV2MXF_GUI : Form
    {
        public XMLTV2MXF_GUI()
        {
            InitializeComponent();

            // Load up the .exe.config settings into the controls

            //input page
            tbInputFileName.Text = XMLTV2MXF.Properties.Settings.Default.inputXMLTVfile;
            cbUsePreProcessor.Checked = XMLTV2MXF.Properties.Settings.Default.usePreProcessor;
            tbPreProcessorCommand.Text = XMLTV2MXF.Properties.Settings.Default.preProcessorCommand;

            // config page
            tbProviderString.Text = XMLTV2MXF.Properties.Settings.Default.ProviderString;
            tbChannelXMLFile.Text = XMLTV2MXF.Properties.Settings.Default.ChannelsXML;

            // output page
            tbMXFFile.Text = XMLTV2MXF.Properties.Settings.Default.outputMXFfile;
            cbUsePostProcessor.Checked = XMLTV2MXF.Properties.Settings.Default.usePostProcessor;
            tbPostProcessorCommand.Text = XMLTV2MXF.Properties.Settings.Default.postProcessorCommand;


            // Start off with the prev button disabled
            btnBack.Enabled = false;
            tabWizard.SelectedIndex = 0;
        }

        /// <summary>
        /// Step the tabWizard along to the next step.
        /// Stops if there are no following steps
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tabWizard.SelectedIndex < tabWizard.TabCount-1)
            {
                tabWizard.SelectedIndex++;
            }
            
        }

        /// <summary>
        /// Step the tabWizard back to the previous step.
        /// Stops if there are no preceeding steps
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tabWizard.SelectedIndex > 0)
            {
                tabWizard.SelectedIndex--;
            }
        }

        /// <summary>
        /// Enable or disable the Next/Back buttons depending on whether they apply
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void tabWizard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabWizard.SelectedIndex == 0)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }
            else if (tabWizard.SelectedIndex == tabWizard.TabCount - 1)
            {
                btnNext.Enabled = false;
                btnBack.Enabled = true;
            }
            else
            {
                btnNext.Enabled = true;
                btnBack.Enabled = true;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          
           DialogResult = DialogResult.Cancel;
           Close();
        }

        /// <summary>
        /// Saves the control values back into the .exe.config and then
        /// returns to the console executable DialogResult.OK so that the conversion is executed
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            // write back the values in the controls to the .settings

            // input page
            XMLTV2MXF.Properties.Settings.Default.inputXMLTVfile = tbInputFileName.Text;
            XMLTV2MXF.Properties.Settings.Default.preProcessorCommand = tbPreProcessorCommand.Text;
            XMLTV2MXF.Properties.Settings.Default.usePreProcessor = cbUsePreProcessor.Checked;

            // config page
            XMLTV2MXF.Properties.Settings.Default.ProviderString = tbProviderString.Text;
            XMLTV2MXF.Properties.Settings.Default.ChannelsXML = tbChannelXMLFile.Text;

            // output page
            XMLTV2MXF.Properties.Settings.Default.outputMXFfile = tbMXFFile.Text;
            XMLTV2MXF.Properties.Settings.Default.usePostProcessor = cbUsePostProcessor.Checked;
            XMLTV2MXF.Properties.Settings.Default.postProcessorCommand = tbPostProcessorCommand.Text;


            // save
            XMLTV2MXF.Properties.Settings.Default.Save();


            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Open a file open dialog to select the input xmltv file
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnBrowseInputFile_Click(object sender, EventArgs e)
        {
            // we need to display an open file dialog:
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = tbInputFileName.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbInputFileName.Text = dlg.FileName; 

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Open a file open dialog to select the channel definition XML file
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnSelectChannelXML_Click(object sender, EventArgs e)
        {
            // we need to display an open file dialog:
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = tbChannelXMLFile.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbChannelXMLFile.Text = dlg.FileName;

            }
        }

        /// <summary>
        /// Open the selected channels file for editing with notepad
        /// Should replace this with an internal version.
        /// </summary>
        /// <param name="sender">(Windows Event Stuff)</param>
        /// <param name="e">(Windows Event Stuff)</param>
        private void btnEditChannels_Click(object sender, EventArgs e)
        {
            Process prNotepad = new Process();
            prNotepad.StartInfo.FileName = "Notepad.exe";

            prNotepad.StartInfo.Arguments = tbChannelXMLFile.Text;

            prNotepad.Start();

        }
    }
}
