using System;
using System.Collections.Generic;
using System.Configuration; 
//using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows;
using System.Diagnostics;
using log4net;



/// <summary>
/// A collection of classes and utils for generating MXF formatted XML files for input into Windows7 MCE using loadmxf.exe
/// </summary>
namespace XMLTV2MXF_WPF
{
    /// <summary>
    /// A class to represent a traditional TV Channel
    /// </summary>
    class TVChannel
    {
        /// <summary>
        /// A unique ID for each channel.  This is what is output into the MXF.
        /// </summary>
        public int id_; 
        /// <summary>
        /// The string that represents the channel in the smltv file
        /// </summary>
        public string searchString_;
        /// <summary>
        /// The name of the channel
        /// </summary>
        public string name_;
        /// <summary>
        /// URL for the logo for the channel (usually file://c:\\something )
        /// </summary>
        public string logoURL;

        /// <summary>
        /// Constructor for the channel class
        /// </summary>
        /// <param name="id">Unique ID for the channel</param>
        /// <param name="name">The name of the channel</param>
        /// <param name="searchString">The string that represents the channel in the xmltv file</param>
        public TVChannel(int id, string name, string searchString)
        {
            id_ = id;
            name_ = name;
            searchString_ = searchString;
        }

        /// <summary>
        /// Get the id of the channel
        /// </summary>
        /// <returns>A string s#, to represent the channel</returns>
        public string id()
        {
            return "s" + id_.ToString();
        }

        /// <summary>
        /// Get the service uid for the channel in MXF format
        /// </summary>
        /// <returns>String in format "!Service!uid"</returns>
        public string uid()
        {
            return "!Service!" + callsign();
        }

        /// <summary>
        /// Get the channel UID for MXF file
        /// </summary>
        /// <returns>String in format "!Channel!uid"</returns>
        public string channelUID()
        {
            return "!Channel!" + XMLTV2MXF_WPF.Properties.Settings.Default.ProviderString + "!" + id_.ToString();
        }

        /// <summary>
        /// Get the callsign (name) of the channel for the MXF file
        /// </summary>
        /// <returns>String: name in uppercase</returns>
        public string callsign()
        {
            return searchString_.ToUpper();
        }

        /// <summary>
        /// Get whom the provider of the channel is.  W7MCE suffixes the channel listing name with this. 
        /// </summary>
        /// <returns>String in "!Affiliate!provider" format</returns>
        public string affiliate()
        {
            return "!Affiliate!" + XMLTV2MXF_WPF.Properties.Settings.Default.ProviderString;
        }

        /// <summary>
        /// Get an identifier the logo for the channel for display in the guide
        /// </summary>
        /// <returns>String in format "!Image!name"</returns>
        public string logoUID()
        {
            return "!Image!Z" + callsign();
        }

        /// <summary>
        /// Get an id the logo for the channel for display in the guide
        /// </summary>
        /// <returns>String in format "i#"</returns>
        public string logoID()
        {
            return "i" + id_.ToString();
        }


    }

    /// <summary>
    /// A Class to represent a particular instance of a TV Programming airing
    /// </summary>
    class TVProgramme
    {
        /// <summary>
        /// Time that the show starts.  In UTC Time.
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// Time that the show ends.  In UTC Time.
        /// </summary>
        public DateTime stopTime;
        /// <summary>
        /// The channel string as found in the xmltv file
        /// </summary>
        public string channel;
        /// <summary>
        /// The description from the xmltv file
        /// </summary>
        public string description;
        /// <summary>
        /// Rating from xmltv.  Not currently used.
        /// </summary>
        public string rating;
        /// <summary>
        /// Title of the program from xmltv file.
        /// </summary>
        public string title;
        /// <summary>
        /// An id for the program that is unique within an MXF file
        /// </summary>
        public int id;
        /// <summary>
        /// Whether the program is part of a series or not.
        /// We usually set this to true by default, but we should be more clever about it by checking reccurrance etc.
        /// </summary>
        public bool isSeries;
        /// <summary>
        /// unique ID for program in the MXF database.  usually a generated guid
        /// </summary>
        public string uid;


        /// <summary>
        /// Main constructor for class
        /// </summary>
        /// <param name="theID">The ID to use for the program.  Needs to be unique to with an MXF file.</param>
        /// <param name="GUID">The guid that will be loaded into the MXF.</param>
        public TVProgramme(int theID, string GUID)
        {
            id = theID;
            isSeries = true;
            uid = "!Program!"+GUID;
        }

        /// <summary>
        /// Get the length of the program in seconds.
        /// </summary>
        /// <returns>String:  The program length in seconds</returns>
        public string duration()
        {
            return stopTime.Subtract(startTime).TotalSeconds.ToString();
        }

        /// <summary>
        /// Get a string representation of the program
        /// </summary>
        /// <returns>A string representation</returns>
        public override string ToString()
        {
            return title + " : " + description + ",@ " + startTime.ToString() + ",  for " + duration() + ", on " + channel + ", " + rating;
        }

        /// <summary>
        /// Get a short string representation of the program
        /// </summary>
        /// <returns>A short string representation</returns>
        public string ToShortString()
        {
            return id.ToString() + " : " + uid + ":" + title;
        }
    }

    /// <summary>
    /// The main program class for generating the MXF
    /// </summary>
    class XMLTV2MXF_Main
    {
        /// <summary>
        /// log4net object.  Relies on calling program to configure...
        /// </summary>
        static ILog logger;

        /// <summary>
        /// Collection of all the programs.
        /// </summary>
        static System.Collections.ArrayList progs;
        /// <summary>
        /// Collection of ll the channels.
        /// </summary>
        static System.Collections.ArrayList channels;

        /// <summary>
        /// Main execution point for the xmltv2mxf conversions
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        public static void doProcessing()
        {
            //logging
            logger = LogManager.GetLogger("XMLTV2MXF.XMLTV2MXF");
            logger.Debug("Starting up");

            // init the program/channel collections
            progs = new System.Collections.ArrayList();
            channels = new System.Collections.ArrayList();

            logger.Debug("About to read in channels xml file");
            try
            {
                // Get the channel information
                readChannelSettings(XMLTV2MXF_WPF.Properties.Settings.Default.ChannelsXML);
            }
            catch (Exception e)
            {
                logger.Error("Error reading Channels file! Check .exe.settings",e);
                return;
            }


            logger.Debug("About to process input xmltv file");
            try
            {
                // process the xmltv file
                processXMLTV(XMLTV2MXF_WPF.Properties.Settings.Default.inputXMLTVfile);
            }
            catch (Exception e)
            {
                logger.Error("Error reading xmltv file! Check .exe.settings",e);
                return;
            }

            logger.Debug("About to create and output MXF file");
            try
            {
                // output the MXF
                writeMXF(XMLTV2MXF_WPF.Properties.Settings.Default.outputMXFfile);
            }
            catch (Exception e)
            {
                logger.Error("Error writing MXF file! Check .exe.settings, permissions, assembly trust",e);
                return;
            }
            logger.Debug("Finished");
        }

        /// <summary>
        /// Convert the string found in the xmltv file into a DateTime object in UTC time
        /// </summary>
        /// <param name="theDateTimeStr">The xmltv string representing the DateTime</param>
        /// <returns>A new DateTime object in UTC time</returns>
        static DateTime DateTimefromXMLTVNZFormat(string theDateTimeStr)
        {
            // fix this - it is really ugly
            DateTime theDateTime = new System.DateTime(Int32.Parse(theDateTimeStr.Substring(0, 4)), Int32.Parse(theDateTimeStr.Substring(4, 2)), Int32.Parse(theDateTimeStr.Substring(6, 2)), Int32.Parse(theDateTimeStr.Substring(8, 2)), Int32.Parse(theDateTimeStr.Substring(10, 2)), Int32.Parse(theDateTimeStr.Substring(12, 2)));

            theDateTime=theDateTime.ToUniversalTime();      

            return theDateTime;
        }

        /// <summary>
        /// Convert a DateTime object into the correct string format for MXF files
        /// </summary>
        /// <param name="theDateTime"></param>
        /// <returns>A String in the MXF date time format</returns>
        static string DateTimeToMXFFormat(DateTime theDateTime)
        {
            return theDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// Procedure to read in an xmltv file, storing programs as they are found
        /// </summary>
        /// <param name="XMLTVfilename">The filename to open</param>
        static void processXMLTV(string XMLTVfilename)
        {
            logger.Debug("Processing: " + XMLTVfilename);

            XmlTextReader xmlReader = new XmlTextReader(XMLTVfilename);


            xmlReader.Read(); // get the Root Node

            while (xmlReader.Read())  // read to end of file
            {
                // only interested in programme nodes
                if (xmlReader.Name == "programme")
                {
                    // create the program
                    TVProgramme newProg = new TVProgramme(progs.Count+1, System.Guid.NewGuid().ToString("N"));
                    newProg.startTime = DateTimefromXMLTVNZFormat(xmlReader.GetAttribute("start").Trim());
                    newProg.stopTime = DateTimefromXMLTVNZFormat(xmlReader.GetAttribute("stop").Trim());
                    newProg.channel = xmlReader.GetAttribute("channel").Trim();

                    // horrible way of recording when we are at the end of the programme XML tree
                    int depth = xmlReader.Depth;

                    xmlReader.Read();
                    while (xmlReader.Depth > depth) // keep going while we are 'inside' the program in XML
                    {
                        // only interested in elements for now
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            
                            switch (xmlReader.Name.ToString())
                            {
                                case "title":
                                    newProg.title = xmlReader.ReadString().Trim();
                                    break;
                                case "rating":
                                    newProg.rating = xmlReader.ReadString().Trim();
                                    break;
                                case "desc":
                                    newProg.description = xmlReader.ReadString().Trim().Replace('"',' ');
                                    break;
                            }
                        }   
                        xmlReader.Read(); // get the next element within the program
                    } 
                    progs.Add(newProg); // remember for later
                    
                }
                
            }

            // we are done
            xmlReader.Close();
        }

        /// <summary>
        /// Generate and MXF formatted XML file from the info we collected in progs and channels 
        /// from the xmltv file
        /// </summary>
        /// <param name="outputFileName">The XML file to write to</param>
        static void writeMXF(string outputFileName)
        {
            // create the file
            XmlTextWriter xmlWriter = new XmlTextWriter(outputFileName, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented; // make it pretty

            // kick things off
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("MXF");
            xmlWriter.WriteAttributeString("xmlns:sql", "urn:schemas-microsoft-com:xml-sql");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // there is a bunch of stuff that we write as a header
            writePreamble(xmlWriter);
            xmlWriter.WriteStartElement("With");
            xmlWriter.WriteAttributeString("provider", "provider1");

            // keywords for grouping and searching
            writeKeywords(xmlWriter);

            // images for channels/series/programs
            writeImages(xmlWriter);

            // actors etc
            writePeople(xmlWriter);

            // series info
            writeSeries(xmlWriter);

            // seasons ,the parent of series
            writeSeasons(xmlWriter);

            // the TV program info
            writePrograms(xmlWriter);

            // who is providing the listings (Us!)
            writeAffiliates(xmlWriter);

            // channel info
            writeServices(xmlWriter);

            // when the programs are aired
            writeScheduleEntries(xmlWriter);

            // more channel stuff really
            writeLineups(xmlWriter);

            
            // finish up
            xmlWriter.WriteEndElement(); // with
            xmlWriter.WriteEndElement(); // root MXF Node
            xmlWriter.Close();

            // done!
            // A properly formatted file to load into W7MCE
        }

        /// <summary>
        /// Write a whole lot of header info out to the file
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writePreamble(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Assembly");
            theWriter.WriteAttributeString("name", "mcepg");
            theWriter.WriteAttributeString("version", "6.0.6000.0");
            theWriter.WriteAttributeString("cultureInfo", "");
            theWriter.WriteAttributeString("publicKey", "0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9");

            theWriter.WriteStartElement("NameSpace");
            theWriter.WriteAttributeString("name", "Microsoft.MediaCenter.Guide");

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Lineup");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Channel");
            theWriter.WriteAttributeString("parentFieldName", "lineup");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Service");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "ScheduleEntry");
            theWriter.WriteAttributeString("groupName", "ScheduleEntries");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Keyword");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "KeywordGroup");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Person");
            theWriter.WriteAttributeString("groupName", "People");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "ActorRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "DirectorRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "WriterRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "HostRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "GuestActorRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "ProducerRole");
            theWriter.WriteAttributeString("parentFieldName", "program");
            theWriter.WriteEndElement(); // Type Node


            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "GuideImage");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Affiliate");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "SeriesInfo");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Season");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteEndElement(); // Namespace Node
            theWriter.WriteEndElement(); // Assembly Node


            theWriter.WriteStartElement("Assembly");
            theWriter.WriteAttributeString("name", "mcstore");
            theWriter.WriteAttributeString("version", "6.0.6000.0");
            theWriter.WriteAttributeString("cultureInfo", "");
            theWriter.WriteAttributeString("publicKey", "0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9");

            theWriter.WriteStartElement("NameSpace");
            theWriter.WriteAttributeString("name", "Microsoft.MediaCenter.Store");

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "Provider");
            theWriter.WriteEndElement(); // Type Node

            theWriter.WriteStartElement("Type");
            theWriter.WriteAttributeString("name", "UId");
            theWriter.WriteAttributeString("parentFieldName", "target");
            theWriter.WriteEndElement(); // Type Node            

            theWriter.WriteEndElement(); // Namespace Node
            theWriter.WriteEndElement(); // Assembly Node

            theWriter.WriteStartElement("Providers");
            theWriter.WriteStartElement("Provider");
            theWriter.WriteAttributeString("id", "provider1");
            theWriter.WriteAttributeString("name", ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteAttributeString("displayName", ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteAttributeString("copyright", ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteEndElement(); // Provider Node     

            theWriter.WriteEndElement(); // Providers Node

        }

        /// <summary>
        /// Write out the Keywords section of the MXF File.
        /// This is used for grouping and sorting in the guide.
        /// Not implemented at this stage.  Just writes dummy info at this stage
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeKeywords(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Keywords");

            theWriter.WriteStartElement("Keyword");
            theWriter.WriteAttributeString("id", "k100");
            theWriter.WriteAttributeString("word", "General");
            theWriter.WriteEndElement(); // Keyword Node

            theWriter.WriteEndElement(); // Keywords


            theWriter.WriteStartElement("KeywordGroups");


            theWriter.WriteStartElement("KeywordGroup");
            theWriter.WriteAttributeString("uid", "!KeywordGroup!k1");
            theWriter.WriteAttributeString("groupName", "k1");
            theWriter.WriteAttributeString("keywords", "k100");
            theWriter.WriteEndElement(); // Keyword Group

            theWriter.WriteEndElement(); // Keyword Groups
        }

        /// <summary>
        /// Write out an individual image to the MXF file
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="Id">id of the image in "i#" format</param>
        /// <param name="uid">uid for image in "!Image!name" format</param>
        /// <param name="imageURL">url for image. usually in "file://c:\somewhere
        ///  format, but can be http:// too</param>
        static void writeImage(XmlTextWriter theWriter, string Id, string uid, string imageURL)
        {
             theWriter.WriteStartElement("GuideImage");
             theWriter.WriteAttributeString("id", Id);
             theWriter.WriteAttributeString("uid", uid);
             theWriter.WriteAttributeString("imageUrl", imageURL);
            theWriter.WriteEndElement(); // Guide Image
        }

        /// <summary>
        /// Write out the images used in the file.
        /// They can be used for channels, programs, series.
        /// At this stage, each channel has a logo
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeImages(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("GuideImages");

            foreach (TVChannel currChannel in channels)
            {
                writeImage(theWriter, currChannel.logoID(), currChannel.logoUID(), currChannel.logoURL);
            }

            /*
            writeImage(theWriter,"i1","!Image!TVONE", "file://c:\\Guide\\Logos\\Colour\\one.png");
            writeImage(theWriter, "i2", "!Image!CHANNEL2", "file://c:\\Guide\\Logos\\Colour\\2.png");
            writeImage(theWriter, "i3", "!Image!TV3", "file://c:\\Guide\\Logos\\Colour\\3.png");
            writeImage(theWriter, "i4", "!Image!C4", "file://c:\\Guide\\Logos\\Colour\\C4.png");
            writeImage(theWriter, "i5", "!Image!Maori", "file://c:\\Guide\\Logos\\Colour\\Maori.png");
            writeImage(theWriter, "i6", "!Image!TVNZ6", "file://c:\\Guide\\Logos\\Colour\\TVNZ6.png");
            writeImage(theWriter, "i7", "!Image!TVNZ7", "file://c:\\Guide\\Logos\\Colour\\TVNZ7.png");
            writeImage(theWriter, "i8", "!Image!SportsExtra", "file://c:\\Guide\\Logos\\Colour\\SportsExtra.png");
            writeImage(theWriter, "i10", "!Image!TVNZ", "file://c:\\Guide\\Logos\\Colour\\TVNZ.png");
            writeImage(theWriter, "i100", "!Image!24", "file://c:\\Guide\\Logos\\Shows\\24.png");
            */

            theWriter.WriteEndElement(); // Guide Images
        }

        /// <summary>
        /// Write out the people referenced by progams, series.
        /// Not yet implemented
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writePeople(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("People");
            /*
            theWriter.WriteStartElement("Person");
            theWriter.WriteAttributeString("id", "p1");
            theWriter.WriteAttributeString("name", "Dave Letterman");
            theWriter.WriteAttributeString("uid", "!Person!Dave Letterman");
            theWriter.WriteEndElement(); // Person
            */

            theWriter.WriteEndElement(); // People
        }

        /// <summary>
        /// Write out the series information referenced by programs.
        /// Not yet implemented.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeSeries(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("SeriesInfos");
            
        /*  Example for reference:
            theWriter.WriteStartElement("SeriesInfo");
            theWriter.WriteAttributeString("id", "si1");
            theWriter.WriteAttributeString("uid", "!Series!24");
            theWriter.WriteAttributeString("title", "24");
            theWriter.WriteAttributeString("shortTitle", "24");
            theWriter.WriteAttributeString("description", "Jack Bower does cool stuff.");
            theWriter.WriteAttributeString("shortDescription", "24.  Cool.");
            theWriter.WriteAttributeString("startAirdate", "2009-01-01T00:00:00");
            theWriter.WriteAttributeString("endAirdate", "2010-01-01T00:00:00");
            theWriter.WriteAttributeString("guideImage", "i100");
            theWriter.WriteEndElement(); // Series Info
         */
            theWriter.WriteEndElement(); // Series Infos
        }

        /// <summary>
        /// Write out the season information referenced by seires.
        /// Not yet implemented.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeSeasons(XmlTextWriter theWriter)
        {
                    theWriter.WriteStartElement("Seasons");
       /* theWriter.WriteStartElement("Season");
        theWriter.WriteAttributeString("id","sn1");
        theWriter.WriteAttributeString("uid","!Season!164959780");
            theWriter.WriteAttributeString("series","si1");
            theWriter.WriteAttributeString("title","24: Season 05");
            theWriter.WriteAttributeString("year","2009");
          theWriter.WriteEndElement(); // Season
        */
           theWriter.WriteEndElement(); // Seasons
        }

        /// <summary>
        /// Write out a program to the MXF.  There are attributes that we do not utilise yet, such as series and people info.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="id">id for the program, unique for the program in the MXF file</param>
        /// <param name="uid">id for the program, unique for the program in the W7MCE MXF database</param>
        /// <param name="title">Title of the program</param>
        /// <param name="description">Synopsis</param>
        /// <param name="isSeries">Whether it is a series or not.  We default this to true for now</param>
        static void writeSimpleProgram(XmlTextWriter theWriter, string id, string uid, string title, string description, bool isSeries)
        {

            theWriter.WriteStartElement("Program");
            theWriter.WriteAttributeString("id", id);
            theWriter.WriteAttributeString("uid", uid);
            theWriter.WriteAttributeString("title", title);
            theWriter.WriteAttributeString("description", description);
            if (isSeries) theWriter.WriteAttributeString("isSeries", "true");
            
            /* Other stuff we are not yet using
                theWriter.WriteAttributeString("shortDescription", "Should 1 be prime?");
                theWriter.WriteAttributeString("episodeTitle", "Divisible by one and itself");
                theWriter.WriteAttributeString("language", "en-us");
                theWriter.WriteAttributeString("episodeNumber", "1");
                theWriter.WriteAttributeString("originalAirdate", "2001-09-10T00:00:00");
                theWriter.WriteAttributeString("keywords", "k100");
                theWriter.WriteAttributeString("series", "si1");
                theWriter.WriteAttributeString("isProgramEpisodic", "1");
                theWriter.WriteAttributeString("isSeries", "1");
                theWriter.WriteAttributeString("isKids", "0");

                theWriter.WriteStartElement("ActorRole");
                theWriter.WriteAttributeString("person", "p1");
                theWriter.WriteAttributeString("rank", "1");
                theWriter.WriteEndElement(); // Actor Role   
             */

            theWriter.WriteEndElement(); // Program
 
        }

        /// <summary>
        /// Write out all of the programs, that have been asigned to a known channel
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writePrograms(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Programs");
            foreach (TVChannel currChannel in channels)
            {
                foreach (TVProgramme currProg in progs)
                {
                    if (currProg.channel==currChannel.searchString_)
                    {
                        writeSimpleProgram(theWriter, currProg.id.ToString(), currProg.uid, currProg.title, currProg.description, currProg.isSeries);
                    }
                }

            }
            theWriter.WriteEndElement(); // Programs
        }

        /// <summary>
        /// Write out the affiliates for the channels.  
        /// This is the string that shows up behind the channel name in the guide listings settings tool.
        /// We get this from the .exe.config file in the "providerString" setting.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeAffiliates(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Affiliates");
            theWriter.WriteStartElement("Affiliate");
            theWriter.WriteAttributeString("name", ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteAttributeString("uid", "!Affiliate!"+ConfigurationSettings.AppSettings["providerString"]);
            //theWriter.WriteAttributeString("logoImage", "i100");
            theWriter.WriteEndElement(); // Affiliate
            theWriter.WriteEndElement(); // Affiliates
        }

        /// <summary>
        /// Write out the details for an individual service (channel)
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="id">id for the service, unique within MXF</param>
        /// <param name="uid">uid for the service, unique within W7MCE MXF database</param>
        /// <param name="name">name for the service</param>
        /// <param name="callSign">This I think matches up with the name as found by MCE when it scans</param>
        /// <param name="affiliate">Who provides the channel</param>
        /// <param name="logoimage">Image id "i#" to display</param>
        static void writeService(XmlTextWriter theWriter, string id, string uid, string name, string callSign, string affiliate, string logoimage)
        {
            theWriter.WriteStartElement("Service");
            theWriter.WriteAttributeString("id", id);
            theWriter.WriteAttributeString("uid", uid);
            theWriter.WriteAttributeString("name", name);
            theWriter.WriteAttributeString("callSign", callSign);
            theWriter.WriteAttributeString("affiliate", affiliate);
            theWriter.WriteAttributeString("logoImage", logoimage);
            theWriter.WriteEndElement(); // Service
        }

        /// <summary>
        /// Write out all the services (channels) we have learnt from the xmltv file.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeServices(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Services");

            foreach (TVChannel currChannel in channels)
            {
                writeService(theWriter, currChannel.id(), currChannel.uid(), currChannel.name_, currChannel.callsign(), currChannel.affiliate(), currChannel.logoID());
            }
            
            /*  Sample entries
            writeService(theWriter, "s1", "!Service!TVONE_XMLTVNZ", "One", "TVONE", "!Affiliate!XMLTVNZ", "i1");
            writeService(theWriter, "s2", "!Service!CHANNEL2_XMLTVNZ", "Channel 2", "CHANNEL2", "!Affiliate!XMLTVNZ", "i2");
            writeService(theWriter, "s3", "!Service!TV3_XMLTVNZ", "TV3", "TV3", "!Affiliate!XMLTVNZ", "i3");
            writeService(theWriter, "s4", "!Service!C4_XMLTVNZ", "C4", "C4", "!Affiliate!XMLTVNZ", "i4");
            writeService(theWriter, "s5", "!Service!MAORITV_XMLTVNZ", "Maori TV", "MAORITV", "!Affiliate!XMLTVNZ", "i5");
            writeService(theWriter, "s6", "!Service!TVNZ6_XMLTVNZ", "TVNZ 6", "TVNZ6", "!Affiliate!XMLTVNZ", "i6");
            writeService(theWriter, "s7", "!Service!TVNZ7_XMLTVNZ", "TVNZ 7", "TVNZ7", "!Affiliate!XMLTVNZ", "i7");
            writeService(theWriter, "s8", "!Service!SPORTSEXTRA_XMLTVNZ", "Sports Extra", "SPORTSEXTRA", "!Affiliate!XMLTVNZ", "i8");
            */

            theWriter.WriteEndElement(); // Services
        }

        /// <summary>
        ///  Write out a particular instance of a program being aired.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="id">id of the program, unique within the MXF file</param>
        /// <param name="startTime">String representing the startime in UTC.  Only listed for first program in schedule.</param>
        /// <param name="duration">Programme length in seconds</param>
        static void writeScheduleEntry(XmlTextWriter theWriter, string id, string startTime, string duration)
        {
            theWriter.WriteStartElement("ScheduleEntry");
            theWriter.WriteAttributeString("program", id);
            if(startTime!=null)
                theWriter.WriteAttributeString("startTime", startTime);
            theWriter.WriteAttributeString("duration", duration);
            /* Other attributes we don't yet use
            theWriter.WriteAttributeString("isCC", "1");
            theWriter.WriteAttributeString("isStereo", "1");
            theWriter.WriteAttributeString("isRepeat", "1");
            */
            theWriter.WriteEndElement(); // ScheduleEntry

        }

        /// <summary>
        /// Write out all the program instances we know about for a given channel.
        /// Does a horrible pass through every program.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="service">The service ID that we are writing out</param>
        /// <param name="channelStr">The string that marches up with the string that was attached to the program in the xmptv file</param>
        static void writeScheduleForChannel(XmlTextWriter theWriter, string service, string channelStr)
        {
            theWriter.WriteStartElement("ScheduleEntries");
            theWriter.WriteAttributeString("service", service);

            bool isFirst = true;  // we need to know this so that we output the start time for the first entry only
            int ShowCount = 0;
            //cycle through the programs and create entries for the given service (channel)
            foreach (TVProgramme currProg in progs)
            {
                if (currProg.channel == channelStr)
                {
                    ShowCount++; // keep for debub purposes
                    if (isFirst)  // write out the starttime for the first entry only
                    {
                        writeScheduleEntry(theWriter, currProg.id.ToString(), DateTimeToMXFFormat(currProg.startTime), currProg.duration());
                        isFirst = false;
                    }
                    else
                    {
                        writeScheduleEntry(theWriter, currProg.id.ToString(), null, currProg.duration());
                    }
                }
            }

            theWriter.WriteEndElement(); // ScheduleEntries
            // output debug info
            if(ShowCount>0)
                logger.Info("Processed Channel: " + channelStr + " wrote " + ShowCount.ToString() + " entries ");
        }

        /// <summary>
        /// Read in the channel settings from the given XML file.
        /// This tells us what channels we are interested in from the xmltv file and matches them up
        /// with the MXF fiel entries generated.
        /// </summary>
        /// <param name="fName">The name of the XML file containing the channel settings.</param>
        static void readChannelSettings(string fName)
        {
                // we are going to load up the channel settings
                logger.Debug("About to open: " + fName);
                XmlTextReader xmlChannels = new XmlTextReader(fName);
                while (xmlChannels.Read())
                {
                    if (xmlChannels.NodeType == XmlNodeType.Element && xmlChannels.Depth < 3)
                    {
                        if ((xmlChannels.Name == "Channel"))
                        {
                            //logger.Debug(xmlChannels.ReadOuterXml());
                            string cid = xmlChannels.GetAttribute("cid");
                            string name = xmlChannels.GetAttribute("name");
                            if (name == null) name = cid;
                            TVChannel newChannel = new TVChannel(channels.Count + 1, name, cid);
                            newChannel.logoURL = xmlChannels.GetAttribute("logo");
                            channels.Add(newChannel);
                            //logger.Debug("Processed Channel :" + newChannel.name_ + " from " + fName);
                        }
                    }

                }
                xmlChannels.Close();
        }


        /// <summary>
        /// Write out the instances of each program for each channel.
        /// This horribly performs mutliple passes of the progs collection.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeScheduleEntries(XmlTextWriter theWriter)
        {
            foreach (TVChannel currChannel in channels)
            {
                writeScheduleForChannel(theWriter, currChannel.id(), currChannel.searchString_);
            }

            /*  Sample entries
            writeScheduleForChannel(theWriter, "s1", "tv1");
            writeScheduleForChannel(theWriter, "s2", "tv2");
            writeScheduleForChannel(theWriter, "s3", "tv3");
            writeScheduleForChannel(theWriter, "s4", "c4");
            writeScheduleForChannel(theWriter, "s5", "maori");
            writeScheduleForChannel(theWriter, "s6", "tvnz6");
            writeScheduleForChannel(theWriter, "s7", "tvnz7");
            writeScheduleForChannel(theWriter, "s8", "tvnzsportextra");
             */
        }

        /// <summary>
        /// Write out the definition for a channel (service). 
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        /// <param name="uid">unique id for channel in the W7MCE MXF database in "!Channel!name" format</param>
        /// <param name="lineup">The collection of channels in "!Lineup!" format</param>
        /// <param name="service">The service that the channel corresponds to in "!Service!" format</param>
        /// <param name="number">Channel number</param>
        static void writeChannel(XmlTextWriter theWriter, string uid, string lineup, string service, string number)
        {
            theWriter.WriteStartElement("Channel");
            theWriter.WriteAttributeString("uid", uid);
            theWriter.WriteAttributeString("lineup", lineup);
            theWriter.WriteAttributeString("service", service);
            theWriter.WriteAttributeString("number", number);
            theWriter.WriteEndElement(); // Channel
        }
 
        /// <summary>
        /// Write out the lineups (collections of channels)
        /// We only write out 1 at this stage, so it is hard-coded.
        /// For the lineup it writes out each of the channels we know about.
        /// </summary>
        /// <param name="theWriter">The object that writes to the file</param>
        static void writeLineups(XmlTextWriter theWriter)
        {
            theWriter.WriteStartElement("Lineups");

            theWriter.WriteStartElement("Lineup");
            theWriter.WriteAttributeString("id", "l1");
            theWriter.WriteAttributeString("uid", "!Lineup!" + ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteAttributeString("name", ConfigurationSettings.AppSettings["providerString"]);
            theWriter.WriteAttributeString("primaryProvider", "!MCLineup!MainLineup");

            theWriter.WriteStartElement("channels");

            foreach (TVChannel currChannel in channels)
            {
                writeChannel(theWriter, currChannel.channelUID(), "l1", currChannel.id(), currChannel.id_.ToString());
            }

            /* Example entries
            writeChannel(theWriter,"!Channel!FREEVIEWHD1!1", "l1", "s1", "1");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!2", "l1", "s2", "2");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!3", "l1", "s3", "3");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!4", "l1", "s4", "4");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!5", "l1", "s5", "5");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!6", "l1", "s6", "6");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!7", "l1", "s7", "7");
            writeChannel(theWriter, "!Channel!FREEVIEWHD1!8", "l1", "s8", "8");
            */
 
            theWriter.WriteEndElement(); // Channels

            theWriter.WriteEndElement(); // Line Up

            theWriter.WriteEndElement(); // Line Ups
        }
    }


}
