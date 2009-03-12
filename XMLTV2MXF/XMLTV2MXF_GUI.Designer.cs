namespace XMLTV2MXF
{
    partial class XMLTV2MXF_GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBatch = new System.Windows.Forms.Label();
            this.tabWizard = new System.Windows.Forms.TabControl();
            this.tabStep1 = new System.Windows.Forms.TabPage();
            this.tbPreProcessorCommand = new System.Windows.Forms.TextBox();
            this.lblPreCommand = new System.Windows.Forms.Label();
            this.cbUsePreProcessor = new System.Windows.Forms.CheckBox();
            this.btnBrowseInputFile = new System.Windows.Forms.Button();
            this.tbInputFileName = new System.Windows.Forms.TextBox();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.tabStep2 = new System.Windows.Forms.TabPage();
            this.tabStep3 = new System.Windows.Forms.TabPage();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblProvider = new System.Windows.Forms.Label();
            this.tbProviderString = new System.Windows.Forms.TextBox();
            this.btnSelectChannelXML = new System.Windows.Forms.Button();
            this.tbChannelXMLFile = new System.Windows.Forms.TextBox();
            this.lblChannels = new System.Windows.Forms.Label();
            this.tbPostProcessorCommand = new System.Windows.Forms.TextBox();
            this.lblCommandInfo = new System.Windows.Forms.Label();
            this.cbUsePostProcessor = new System.Windows.Forms.CheckBox();
            this.btnBrowseMXF = new System.Windows.Forms.Button();
            this.tbMXFFile = new System.Windows.Forms.TextBox();
            this.lblMXFoutput = new System.Windows.Forms.Label();
            this.btnEditChannels = new System.Windows.Forms.Button();
            this.tabWizard.SuspendLayout();
            this.tabStep1.SuspendLayout();
            this.tabStep2.SuspendLayout();
            this.tabStep3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(381, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "XMLTV2MXF Configuration Wizard";
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Location = new System.Drawing.Point(14, 35);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(390, 13);
            this.lblBatch.TabIndex = 1;
            this.lblBatch.Text = "You are seeing this because you did not specify \"/BATCH\" on the command line.";
            // 
            // tabWizard
            // 
            this.tabWizard.Controls.Add(this.tabStep1);
            this.tabWizard.Controls.Add(this.tabStep2);
            this.tabWizard.Controls.Add(this.tabStep3);
            this.tabWizard.Location = new System.Drawing.Point(17, 60);
            this.tabWizard.Name = "tabWizard";
            this.tabWizard.SelectedIndex = 0;
            this.tabWizard.Size = new System.Drawing.Size(511, 188);
            this.tabWizard.TabIndex = 2;
            this.tabWizard.SelectedIndexChanged += new System.EventHandler(this.tabWizard_SelectedIndexChanged);
            // 
            // tabStep1
            // 
            this.tabStep1.Controls.Add(this.tbPreProcessorCommand);
            this.tabStep1.Controls.Add(this.lblPreCommand);
            this.tabStep1.Controls.Add(this.cbUsePreProcessor);
            this.tabStep1.Controls.Add(this.btnBrowseInputFile);
            this.tabStep1.Controls.Add(this.tbInputFileName);
            this.tabStep1.Controls.Add(this.lblInputFile);
            this.tabStep1.Location = new System.Drawing.Point(4, 22);
            this.tabStep1.Name = "tabStep1";
            this.tabStep1.Padding = new System.Windows.Forms.Padding(3);
            this.tabStep1.Size = new System.Drawing.Size(503, 162);
            this.tabStep1.TabIndex = 0;
            this.tabStep1.Text = "Step 1: Input";
            this.tabStep1.UseVisualStyleBackColor = true;
            // 
            // tbPreProcessorCommand
            // 
            this.tbPreProcessorCommand.Location = new System.Drawing.Point(11, 103);
            this.tbPreProcessorCommand.Multiline = true;
            this.tbPreProcessorCommand.Name = "tbPreProcessorCommand";
            this.tbPreProcessorCommand.Size = new System.Drawing.Size(486, 32);
            this.tbPreProcessorCommand.TabIndex = 5;
            // 
            // lblPreCommand
            // 
            this.lblPreCommand.AutoSize = true;
            this.lblPreCommand.Location = new System.Drawing.Point(6, 87);
            this.lblPreCommand.Name = "lblPreCommand";
            this.lblPreCommand.Size = new System.Drawing.Size(275, 13);
            this.lblPreCommand.TabIndex = 4;
            this.lblPreCommand.Text = "Command ($INPUTFILE is replaced with the path above)";
            // 
            // cbUsePreProcessor
            // 
            this.cbUsePreProcessor.AutoSize = true;
            this.cbUsePreProcessor.Location = new System.Drawing.Point(6, 67);
            this.cbUsePreProcessor.Name = "cbUsePreProcessor";
            this.cbUsePreProcessor.Size = new System.Drawing.Size(108, 17);
            this.cbUsePreProcessor.TabIndex = 3;
            this.cbUsePreProcessor.Text = "use preProcessor";
            this.cbUsePreProcessor.UseVisualStyleBackColor = true;
            // 
            // btnBrowseInputFile
            // 
            this.btnBrowseInputFile.Location = new System.Drawing.Point(465, 13);
            this.btnBrowseInputFile.Name = "btnBrowseInputFile";
            this.btnBrowseInputFile.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseInputFile.TabIndex = 2;
            this.btnBrowseInputFile.Text = "...";
            this.btnBrowseInputFile.UseVisualStyleBackColor = true;
            this.btnBrowseInputFile.Click += new System.EventHandler(this.btnBrowseInputFile_Click);
            // 
            // tbInputFileName
            // 
            this.tbInputFileName.Location = new System.Drawing.Point(91, 13);
            this.tbInputFileName.Name = "tbInputFileName";
            this.tbInputFileName.Size = new System.Drawing.Size(368, 20);
            this.tbInputFileName.TabIndex = 1;
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.CausesValidation = false;
            this.lblInputFile.Location = new System.Drawing.Point(6, 16);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(79, 13);
            this.lblInputFile.TabIndex = 0;
            this.lblInputFile.Text = "input xmltv File:";
            // 
            // tabStep2
            // 
            this.tabStep2.Controls.Add(this.btnEditChannels);
            this.tabStep2.Controls.Add(this.btnSelectChannelXML);
            this.tabStep2.Controls.Add(this.tbChannelXMLFile);
            this.tabStep2.Controls.Add(this.lblChannels);
            this.tabStep2.Controls.Add(this.tbProviderString);
            this.tabStep2.Controls.Add(this.lblProvider);
            this.tabStep2.Location = new System.Drawing.Point(4, 22);
            this.tabStep2.Name = "tabStep2";
            this.tabStep2.Padding = new System.Windows.Forms.Padding(3);
            this.tabStep2.Size = new System.Drawing.Size(503, 162);
            this.tabStep2.TabIndex = 1;
            this.tabStep2.Text = "Step 2: Processing";
            this.tabStep2.UseVisualStyleBackColor = true;
            // 
            // tabStep3
            // 
            this.tabStep3.Controls.Add(this.tbPostProcessorCommand);
            this.tabStep3.Controls.Add(this.lblCommandInfo);
            this.tabStep3.Controls.Add(this.cbUsePostProcessor);
            this.tabStep3.Controls.Add(this.btnBrowseMXF);
            this.tabStep3.Controls.Add(this.tbMXFFile);
            this.tabStep3.Controls.Add(this.lblMXFoutput);
            this.tabStep3.Location = new System.Drawing.Point(4, 22);
            this.tabStep3.Name = "tabStep3";
            this.tabStep3.Size = new System.Drawing.Size(503, 162);
            this.tabStep3.TabIndex = 2;
            this.tabStep3.Text = "Step 3: Output";
            this.tabStep3.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(63, 254);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(191, 254);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(421, 293);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(109, 53);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "Save and Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(419, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblProvider
            // 
            this.lblProvider.AutoSize = true;
            this.lblProvider.Location = new System.Drawing.Point(57, 18);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(48, 13);
            this.lblProvider.TabIndex = 0;
            this.lblProvider.Text = "provider:";
            // 
            // tbProviderString
            // 
            this.tbProviderString.Location = new System.Drawing.Point(111, 18);
            this.tbProviderString.Name = "tbProviderString";
            this.tbProviderString.Size = new System.Drawing.Size(111, 20);
            this.tbProviderString.TabIndex = 1;
            // 
            // btnSelectChannelXML
            // 
            this.btnSelectChannelXML.Location = new System.Drawing.Point(471, 54);
            this.btnSelectChannelXML.Name = "btnSelectChannelXML";
            this.btnSelectChannelXML.Size = new System.Drawing.Size(26, 23);
            this.btnSelectChannelXML.TabIndex = 5;
            this.btnSelectChannelXML.Text = "...";
            this.btnSelectChannelXML.UseVisualStyleBackColor = true;
            this.btnSelectChannelXML.Click += new System.EventHandler(this.btnSelectChannelXML_Click);
            // 
            // tbChannelXMLFile
            // 
            this.tbChannelXMLFile.Location = new System.Drawing.Point(111, 54);
            this.tbChannelXMLFile.Name = "tbChannelXMLFile";
            this.tbChannelXMLFile.Size = new System.Drawing.Size(354, 20);
            this.tbChannelXMLFile.TabIndex = 4;
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.CausesValidation = false;
            this.lblChannels.Location = new System.Drawing.Point(12, 57);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(93, 13);
            this.lblChannels.TabIndex = 3;
            this.lblChannels.Text = "Channel XML File:";
            this.lblChannels.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbPostProcessorCommand
            // 
            this.tbPostProcessorCommand.Location = new System.Drawing.Point(11, 110);
            this.tbPostProcessorCommand.Multiline = true;
            this.tbPostProcessorCommand.Name = "tbPostProcessorCommand";
            this.tbPostProcessorCommand.Size = new System.Drawing.Size(486, 32);
            this.tbPostProcessorCommand.TabIndex = 11;
            // 
            // lblCommandInfo
            // 
            this.lblCommandInfo.AutoSize = true;
            this.lblCommandInfo.Location = new System.Drawing.Point(6, 94);
            this.lblCommandInfo.Name = "lblCommandInfo";
            this.lblCommandInfo.Size = new System.Drawing.Size(287, 13);
            this.lblCommandInfo.TabIndex = 10;
            this.lblCommandInfo.Text = "Command ($OUTPUTFILE is replaced with the path above)";
            // 
            // cbUsePostProcessor
            // 
            this.cbUsePostProcessor.AutoSize = true;
            this.cbUsePostProcessor.Location = new System.Drawing.Point(6, 74);
            this.cbUsePostProcessor.Name = "cbUsePostProcessor";
            this.cbUsePostProcessor.Size = new System.Drawing.Size(113, 17);
            this.cbUsePostProcessor.TabIndex = 9;
            this.cbUsePostProcessor.Text = "use postProcessor";
            this.cbUsePostProcessor.UseVisualStyleBackColor = true;
            // 
            // btnBrowseMXF
            // 
            this.btnBrowseMXF.Location = new System.Drawing.Point(465, 20);
            this.btnBrowseMXF.Name = "btnBrowseMXF";
            this.btnBrowseMXF.Size = new System.Drawing.Size(26, 23);
            this.btnBrowseMXF.TabIndex = 8;
            this.btnBrowseMXF.Text = "...";
            this.btnBrowseMXF.UseVisualStyleBackColor = true;
            // 
            // tbMXFFile
            // 
            this.tbMXFFile.Location = new System.Drawing.Point(91, 20);
            this.tbMXFFile.Name = "tbMXFFile";
            this.tbMXFFile.Size = new System.Drawing.Size(368, 20);
            this.tbMXFFile.TabIndex = 7;
            // 
            // lblMXFoutput
            // 
            this.lblMXFoutput.AutoSize = true;
            this.lblMXFoutput.CausesValidation = false;
            this.lblMXFoutput.Location = new System.Drawing.Point(6, 23);
            this.lblMXFoutput.Name = "lblMXFoutput";
            this.lblMXFoutput.Size = new System.Drawing.Size(84, 13);
            this.lblMXFoutput.TabIndex = 6;
            this.lblMXFoutput.Text = "output MXF File:";
            // 
            // btnEditChannels
            // 
            this.btnEditChannels.Location = new System.Drawing.Point(390, 80);
            this.btnEditChannels.Name = "btnEditChannels";
            this.btnEditChannels.Size = new System.Drawing.Size(75, 23);
            this.btnEditChannels.TabIndex = 6;
            this.btnEditChannels.Text = "Edit...";
            this.btnEditChannels.UseVisualStyleBackColor = true;
            this.btnEditChannels.Click += new System.EventHandler(this.btnEditChannels_Click);
            // 
            // XMLTV2MXF_GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 358);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.tabWizard);
            this.Controls.Add(this.lblBatch);
            this.Controls.Add(this.lblTitle);
            this.Name = "XMLTV2MXF_GUI";
            this.Text = "XMLTV2MXF_GUI";
            this.TopMost = true;
            this.tabWizard.ResumeLayout(false);
            this.tabStep1.ResumeLayout(false);
            this.tabStep1.PerformLayout();
            this.tabStep2.ResumeLayout(false);
            this.tabStep2.PerformLayout();
            this.tabStep3.ResumeLayout(false);
            this.tabStep3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.TabControl tabWizard;
        private System.Windows.Forms.TabPage tabStep1;
        private System.Windows.Forms.TabPage tabStep2;
        private System.Windows.Forms.TabPage tabStep3;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbInputFileName;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.Button btnBrowseInputFile;
        private System.Windows.Forms.CheckBox cbUsePreProcessor;
        private System.Windows.Forms.TextBox tbPreProcessorCommand;
        private System.Windows.Forms.Label lblPreCommand;
        private System.Windows.Forms.TextBox tbProviderString;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.Button btnSelectChannelXML;
        private System.Windows.Forms.TextBox tbChannelXMLFile;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.TextBox tbPostProcessorCommand;
        private System.Windows.Forms.Label lblCommandInfo;
        private System.Windows.Forms.CheckBox cbUsePostProcessor;
        private System.Windows.Forms.Button btnBrowseMXF;
        private System.Windows.Forms.TextBox tbMXFFile;
        private System.Windows.Forms.Label lblMXFoutput;
        private System.Windows.Forms.Button btnEditChannels;
    }
}