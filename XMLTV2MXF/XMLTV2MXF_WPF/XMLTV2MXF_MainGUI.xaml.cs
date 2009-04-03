using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Win32;
// Third party utils
using AndrewTweddle.Tools.Utilities.CommandLine;
using log4net;


namespace XMLTV2MXF_WPF
{
    /// <summary>
    /// Interaction logic for XMLTV2MXF_MainGUI.xaml
    /// </summary>
    public partial class XMLTV2MXF_MainGUI : Window
    {
        public DataSet dsChannels;

        public int minLogLevel;
        public bool inBatchMode;

        private readonly ILog logger;

        /// <summary>
        /// Main constructor class
        /// </summary>
        public XMLTV2MXF_MainGUI()
        {
            //Load from App.Config file
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger("XMLTV2MXF.XMLTV2MXF_MainGUI");
           

            // set log level
            if (App.CommandLineArgs.ContainsKey("/loglevel"))
            {
                switch (App.CommandLineArgs["/loglevel"].ToString())
                {
                    case "3":
                        // Dead Silent
                        minLogLevel = 3;
                        break;
                    case "2":
                        // Display error
                        minLogLevel = 2;
                        break;
                    case "1":
                        // Display info
                        minLogLevel = 1;
                        break;
                    case "0":
                        // Display info
                        minLogLevel = 0;
                        break;
                    default:
                        // default to info messages
                        minLogLevel = 1;
                        break;
                }
            }
            else
            {
                // default to all messages
                minLogLevel = 0;
            }

            // check for command line "/batch:true" and start processing
            if (App.CommandLineArgs.ContainsKey("/batch") && App.CommandLineArgs["/batch"].ToString() == "true")
            {
                inBatchMode = true;
                logger.Debug("Version: " + this.GetType().Assembly.GetName().Version.ToString());
                logger.Debug("In batch mode");
                logger.Debug("Set logging level to: " + minLogLevel.ToString());

                logger.Debug("Starting Processing:");
                // Run Things
                runProcesses();

                logger.Debug("Finishing Up:");
                // Exit
                Application.Current.Shutdown();
            }
            else
            {

                InitializeComponent();

                inBatchMode = false;
                logger.Debug("Version: " + this.GetType().Assembly.GetName().Version.ToString());
                logger.Debug("In GUI mode");
                logger.Debug("Set logging level to: " + minLogLevel.ToString());

                logger.Debug("Configuring Controls:");

                // set Prev/Next Buttons
                btnPrevStep.IsEnabled = false;
                btnNextStep.IsEnabled = true;

                // populate controls

                //input page
                tbInputXMLTVFile.Text = XMLTV2MXF_WPF.Properties.Settings.Default.inputXMLTVfile;
                cbUsePreProcessor.IsChecked = XMLTV2MXF_WPF.Properties.Settings.Default.usePreProcessor;
                tbPreProcessorCommand.Text = XMLTV2MXF_WPF.Properties.Settings.Default.preProcessorCommand;

                // config page
                tbProviderLabel.Text = XMLTV2MXF_WPF.Properties.Settings.Default.ProviderString;
                // bind the Channel Config Grid
                XElement theChannels = XElement.Load(XMLTV2MXF_WPF.Properties.Settings.Default.ChannelsXML);
                dgChannels.DataContext = theChannels.Elements("Channel");

                dsChannels = new DataSet();
                dsChannels.ReadXml(XMLTV2MXF_WPF.Properties.Settings.Default.ChannelsXML);
                dgChannels.DataContext = dsChannels.Tables[0];


                // output page
                tbOutputMXFFile.Text = XMLTV2MXF_WPF.Properties.Settings.Default.outputMXFfile;
                cbUsePostProcessor.IsChecked = XMLTV2MXF_WPF.Properties.Settings.Default.usePostProcessor;
                tbPostProcessorCommand.Text = XMLTV2MXF_WPF.Properties.Settings.Default.postProcessorCommand;


                // Hide Progress Bar
                //ProgressBar1.Visibility = System.Windows.Visibility.Hidden;
            }
          
        }

        /// <summary>
        /// Open dialog box to select the input file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseInputXMLTVFile_Click(object sender, RoutedEventArgs e)
        {
            // we need to display an open file dialog:
            Microsoft.Win32.FileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> dlgResult = dlg.ShowDialog();

            if (dlgResult==true)
            {
                tbInputXMLTVFile.Text = dlg.FileName;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Simpy enable/disable the Previous / Next buttons as appropriate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabsWizard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            btnPrevStep.IsEnabled = true;
            btnNextStep.IsEnabled = true;
            
            if (tabsWizard.SelectedIndex == 0)
            {
                btnPrevStep.IsEnabled = false;
            }
            else if (tabsWizard.SelectedIndex == tabsWizard.Items.Count -1)
            {
                btnNextStep.IsEnabled = false;
            }

        }

        /// <summary>
        /// Change the tabsWizard selected tab to the previous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevStep_Click(object sender, RoutedEventArgs e)
        {
            if(tabsWizard.SelectedIndex >0)
                tabsWizard.SelectedIndex--;
        }

        /// <summary>
        /// Change the tabsWizard selected tab to the next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (tabsWizard.SelectedIndex < tabsWizard.Items.Count - 1)
                tabsWizard.SelectedIndex++;
        }

        /// <summary>
        /// Ask if the user wants to save settings, then exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("btnExit Clicked - checking save");

            // Configure the message box to be displayed
            string messageBoxText = "Do you want to save changes to settings?";
            string caption = "XMLTV2MXF";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // User pressed Yes button
                    // Save Then Exit
                    logger.Info("Saving then exiting...");
                    saveSettings();
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    // Just exit
                    logger.Info("Exiting without saving....");
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    logger.Debug("Exit Cancelled");
                    break;
            }

        }

        /// <summary>
        /// Change the tabsWizard selected tab to the next
        /// </summary>
        /// 
        private void saveSettings()
        {
            // write back the values in the controls to the .settings
            logger.Debug("Modding Settings");

            // input page
            XMLTV2MXF_WPF.Properties.Settings.Default.inputXMLTVfile = tbInputXMLTVFile.Text;
            XMLTV2MXF_WPF.Properties.Settings.Default.preProcessorCommand = tbPreProcessorCommand.Text;
            XMLTV2MXF_WPF.Properties.Settings.Default.usePreProcessor = (cbUsePreProcessor.IsChecked==true);

            // config page
            XMLTV2MXF_WPF.Properties.Settings.Default.ProviderString = tbProviderLabel.Text;
            logger.Debug("Writing Channels XML file: " + XMLTV2MXF_WPF.Properties.Settings.Default.ChannelsXML);
            dsChannels.WriteXml(XMLTV2MXF_WPF.Properties.Settings.Default.ChannelsXML, XmlWriteMode.IgnoreSchema);

            // output page
            XMLTV2MXF_WPF.Properties.Settings.Default.outputMXFfile = tbOutputMXFFile.Text;
            XMLTV2MXF_WPF.Properties.Settings.Default.usePostProcessor = (cbUsePostProcessor.IsChecked==true);
            XMLTV2MXF_WPF.Properties.Settings.Default.postProcessorCommand = tbPostProcessorCommand.Text;

            // save properties
            XMLTV2MXF_WPF.Properties.Settings.Default.Save();
            logger.Debug("Saved Settings");
        }

        private void True(object sender, EventArgs e)
        {

        }

        private void btnBrowseOutputMXFFile_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("Opening Dialog to get output file name");
            // we need to display an open file dialog:
            Microsoft.Win32.FileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> dlgResult = dlg.ShowDialog();

            if (dlgResult == true)
            {
                tbOutputMXFFile.Text = dlg.FileName;
                logger.Debug("Output file name now: " + tbOutputMXFFile.Text);
            }
        }

        /// <summary>
        /// When Execute button is clicked, run the pre/conversion/post processes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("btnExecute clicked - saving, then starting processing");
            saveSettings();
            
            runProcesses();
        }

        /// <summary>
        /// Run the pre/conversion/post processes.  The guts of the whole thing!
        /// </summary>
        private void runProcesses()
        {
            logger.Debug("Running PreProcessor if required");
            // if required, run the preprocessor command:
            if (XMLTV2MXF_WPF.Properties.Settings.Default.usePreProcessor)
            {
                try
                {
                    //           // Run the command
                    string cmd = XMLTV2MXF_WPF.Properties.Settings.Default.preProcessorCommand;
                    // substitue $INPUTFILE for the chosen input file
                    cmd = cmd.Replace("$INPUTFILE", XMLTV2MXF_WPF.Properties.Settings.Default.inputXMLTVfile);

                    logger.Debug("preProcessor set, trying to execute: " + cmd);

                    //          Process pr = new Process();
                    int application_split = cmd.IndexOf(".exe") + 4;
                    string FileName = cmd.Substring(0, application_split);
                    string Arguments = cmd.Substring(application_split, cmd.Length - application_split).Trim();

                    logger.Info("Executing preProcessor command:");

                    // Display Progress Bar
                    //ProgressBar1.Visibility = System.Windows.Visibility.Visible;
                        
                    logger.Info(CommandLineHelper.Run(FileName, Arguments));

                }
                catch (Exception e)
                {
                    logger.Error("Error running preProcessor! Check settings",e);
                    return;
                }
            }

            logger.Info("Performing Conversion:");
            XMLTV2MXF_Main.doProcessing();

            logger.Debug("Running PostProcessor if required");
            // if required, run the postprocessor command:
            if (XMLTV2MXF_WPF.Properties.Settings.Default.usePostProcessor)
            {
                try
                {
                    //           // Run the command
                    string cmd = XMLTV2MXF_WPF.Properties.Settings.Default.postProcessorCommand;
                    // substitue $INPUTFILE for the chosen input file
                    cmd = cmd.Replace("$OUTPUTFILE", XMLTV2MXF_WPF.Properties.Settings.Default.outputMXFfile);

                    logger.Debug("postProcessor set, trying to execute: " + cmd);

                    //          Process pr = new Process();
                    int application_split = cmd.IndexOf(".exe") + 4;
                    string FileName = cmd.Substring(0, application_split);
                    string Arguments = cmd.Substring(application_split, cmd.Length - application_split).Trim();

                    logger.Info("Executing postProcessor command:");

                    // Display Progress Bar
                    //ProgressBar1.Visibility = System.Windows.Visibility.Visible;

                    logger.Info(CommandLineHelper.Run(FileName, Arguments));

                }
                catch (Exception e)
                {
                    logger.Error("Error running postProcessor! Check settings",e);
                    return;
                }
            }

        }

         /// <summary>
        /// Give some help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("btnHelp Clicked - displaying dialog");

            // Configure the message box to be displayed
            string messageBoxText = "This application should be pretty straightforward to use"
              + Environment.NewLine + Environment.NewLine
              + "To execute using saved settings without displaying the GUI, use the command line argument:"
              + Environment.NewLine + "   /batch:true"
              + Environment.NewLine + " To change the amount of information output to console/log window:"
              + Environment.NewLine + "   /loglevel:N"
              + Environment.NewLine + "N = 0 (debug) to 3 (dead silent)"
              + Environment.NewLine + Environment.NewLine
              + "Version: " + this.GetType().Assembly.GetName().Version.ToString();

            string caption = "XMLTV2MXF Help";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;

            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            
        }

    }
}
