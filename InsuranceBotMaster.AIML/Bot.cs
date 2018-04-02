using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Net.Mail;

using InsuranceBotMaster.AIML.Utils;

namespace InsuranceBotMaster.AIML
{
    /// <summary>
    /// Encapsulates a bot. If no settings.xml file is found or referenced the bot will try to
    /// default to safe settings.
    /// </summary>
    public class Bot
    {
        #region Attributes

        /// <summary>
        /// A dictionary object that looks after all the settings associated with this bot
        /// </summary>
        public SettingsDictionary GlobalSettings;

        /// <summary>
        /// A dictionary of all the gender based substitutions used by this bot
        /// </summary>
        public SettingsDictionary GenderSubstitutions;

        /// <summary>
        /// A dictionary of all the first person to second person (and back) substitutions
        /// </summary>
        public SettingsDictionary Person2Substitutions;

        /// <summary>
        /// A dictionary of first / third person substitutions
        /// </summary>
        public SettingsDictionary PersonSubstitutions;

        /// <summary>
        /// Generic substitutions that take place during the normalization process
        /// </summary>
        public SettingsDictionary Substitutions;

        /// <summary>
        /// The default predicates to set up for a user
        /// </summary>
        public SettingsDictionary DefaultPredicates;

        /// <summary>
        /// Holds information about the available custom tag handling classes (if loaded)
        /// Key = class name
        /// Value = TagHandler class that provides information about the class
        /// </summary>
        private Dictionary<string, TagHandler> CustomTags;

        /// <summary>
        /// Holds references to the assemblies that hold the custom tag handling code.
        /// </summary>
        private Dictionary<string, Assembly> LateBindingAssemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// An List<> containing the tokens used to split the input into sentences during the 
        /// normalization process
        /// </summary>
        public List<string> Splitters = new List<string>();

        /// <summary>
        /// A buffer to hold log messages to be written out to the log file when a max size is reached
        /// </summary>
        private List<string> LogBuffer = new List<string>();

        /// <summary>
        /// How big to let the log buffer get before writing to disk
        /// </summary>
        private int MaxLogBufferSize
        {
            get
            {
                return Convert.ToInt32(GlobalSettings.GrabSetting("maxlogbuffersize"));
            }
        }

        /// <summary>
        /// Flag to show if the bot is willing to accept user input
        /// </summary>
        public bool isAcceptingUserInput = true;

        /// <summary>
        /// The message to show if a user tries to use the bot whilst it is set to not process user input
        /// </summary>
        private string NotAcceptingUserInputMessage
        {
            get
            {
                return GlobalSettings.GrabSetting("notacceptinguserinputmessage");
            }
        }

        /// <summary>
        /// The maximum amount of time a request should take (in milliseconds)
        /// </summary>
        public double TimeOut
        {
            get
            {
                return Convert.ToDouble(GlobalSettings.GrabSetting("timeout"));
            }
        }

        /// <summary>
        /// The message to display in the event of a timeout
        /// </summary>
        public string TimeOutMessage
        {
            get
            {
                return GlobalSettings.GrabSetting("timeoutmessage");
            }
        }

        /// <summary>
        /// The locale of the bot as a CultureInfo object
        /// </summary>
        public CultureInfo Locale
        {
            get
            {
                return new CultureInfo(GlobalSettings.GrabSetting("culture"));
            }
        }

        /// <summary>
        /// Will match all the illegal characters that might be inputted by the user
        /// </summary>
        public Regex Strippers
        {
            get
            {
                return new Regex(GlobalSettings.GrabSetting("stripperregex"), RegexOptions.IgnorePatternWhitespace);
            }
        }

        /// <summary>
        /// Flag to denote if the bot is writing messages to its logs
        /// </summary>
        public bool IsLogging
        {
            get
            {
                string islogging = GlobalSettings.GrabSetting("islogging");
                if (islogging.ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// When the Bot was initialised
        /// </summary>
        public DateTime StartedOn = DateTime.Now;

        /// <summary>
        /// The supposed sex of the bot
        /// </summary>
        public Gender Sex
        {
            get
            {
                int sex = Convert.ToInt32(GlobalSettings.GrabSetting("gender"));
                Gender result;
                switch (sex)
                {
                    case -1:
                        result = Gender.Unknown;
                        break;
                    case 0:
                        result = Gender.Female;
                        break;
                    case 1:
                        result = Gender.Male;
                        break;
                    default:
                        result = Gender.Unknown;
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// The directory to look in for the AIML files
        /// </summary>
        public string PathToAIML
        {
            get
            {
                return Path.Combine(!string.IsNullOrEmpty(BasePath) ? BasePath : Environment.CurrentDirectory, GlobalSettings.GrabSetting("aimldirectory"));
            }
        }

        /// <summary>
        /// The directory to look in for the various XML configuration files
        /// </summary>
        public string PathToConfigFiles
        {
            get
            {
                return Path.Combine(!string.IsNullOrEmpty(BasePath) ? BasePath : Environment.CurrentDirectory, GlobalSettings.GrabSetting("configdirectory"));
            }
        }

        /// <summary>
        /// The directory into which the various log files will be written
        /// </summary>
        public string PathToLogs
        {
            get
            {
                return Path.Combine(!string.IsNullOrEmpty(BasePath) ? BasePath : Environment.CurrentDirectory, GlobalSettings.GrabSetting("logdirectory"));
            }
        }

        /// <summary>
        /// The number of categories this bot has in its graphmaster "brain"
        /// </summary>
        public int Size;

        /// <summary>
        /// The "brain" of the bot
        /// </summary>
        public Node Graphmaster;

        /// <summary>
        /// If set to false the input from AIML files will undergo the same normalization process that
        /// user input goes through. If true the bot will assume the AIML is correct. Defaults to true.
        /// </summary>
        public bool TrustAIML = true;

        /// <summary>
        /// The maximum number of characters a "that" element of a path is allowed to be. Anything above
        /// this length will cause "that" to be "*". This is to avoid having the graphmaster process
        /// huge "that" elements in the path that might have been caused by the bot reporting third party
        /// data.
        /// </summary>
        public int MaxThatSize = 256;

        public string BasePath = string.Empty;

        #endregion

        #region Delegates

        public delegate void LogMessageDelegate();

        #endregion

        #region Events

        public event LogMessageDelegate WrittenToLog;

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        public Bot()
        {
            this.Setup();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public Bot(string basePath)
        {
            this.Setup();
            BasePath = basePath;
        }

        #region Settings methods

        /// <summary>
        /// Loads AIML from .aiml files into the graphmaster "brain" of the bot
        /// </summary>
        public void LoadAIMLFromFiles()
        {
            AIMLLoader loader = new AIMLLoader(this);
            loader.LoadAIML();
        }

        /// <summary>
        /// Allows the bot to load a new XML version of some AIML
        /// </summary>
        /// <param name="newAIML">The XML document containing the AIML</param>
        /// <param name="filename">The originator of the XML document</param>
        public void LoadAIMLFromXML(XmlDocument newAIML, string filename)
        {
            AIMLLoader loader = new AIMLLoader(this);
            loader.LoadAIMLFromXML(newAIML, filename);
        }

        /// <summary>
        /// Instantiates the dictionary objects and collections associated with this class
        /// </summary>
        private void Setup()
        {
            GlobalSettings = new SettingsDictionary(this);
            GenderSubstitutions = new SettingsDictionary(this);
            Person2Substitutions = new SettingsDictionary(this);
            PersonSubstitutions = new SettingsDictionary(this);
            Substitutions = new SettingsDictionary(this);
            DefaultPredicates = new SettingsDictionary(this);
            CustomTags = new Dictionary<string, TagHandler>();
            Graphmaster = new Node();
        }

        /// <summary>
        /// Loads settings based upon the default location of the Settings.xml file
        /// </summary>
        public void LoadSettings()
        {
            // try a safe default setting for the settings xml file
            string path = Path.Combine(!string.IsNullOrEmpty(BasePath) ? BasePath : Environment.CurrentDirectory, Path.Combine("ConfigurationFiles", "Settings.xml"));
            
            LoadSettings(path);
        }

        /// <summary>
        /// Loads settings and configuration info from various xml files referenced in the settings file passed in the args. 
        /// Also generates some default values if such values have not been set by the settings file.
        /// </summary>
        /// <param name="pathToSettings">Path to the settings xml file</param>
        public void LoadSettings(string pathToSettings)
        {
            GlobalSettings.LoadSettings(pathToSettings);

            // Checks for some important default settings
            if (!GlobalSettings.ContainsSettingCalled("version"))
            {
                GlobalSettings.AddSetting("version", Environment.Version.ToString());
            }
            if (!GlobalSettings.ContainsSettingCalled("name"))
            {
                GlobalSettings.AddSetting("name", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("botmaster"))
            {
                GlobalSettings.AddSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("master"))
            {
                GlobalSettings.AddSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("author"))
            {
                GlobalSettings.AddSetting("author", "Nicholas H.Tollervey");
            }
            if (!GlobalSettings.ContainsSettingCalled("location"))
            {
                GlobalSettings.AddSetting("location", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("gender"))
            {
                GlobalSettings.AddSetting("gender", "-1");
            }
            if (!GlobalSettings.ContainsSettingCalled("birthday"))
            {
                GlobalSettings.AddSetting("birthday", "2006/11/08");
            }
            if (!GlobalSettings.ContainsSettingCalled("birthplace"))
            {
                GlobalSettings.AddSetting("birthplace", "Towcester, Northamptonshire, UK");
            }
            if (!GlobalSettings.ContainsSettingCalled("website"))
            {
                GlobalSettings.AddSetting("website", "http://sourceforge.net/projects/InsuranceBotMaster.AIML");
            }
            if (!GlobalSettings.ContainsSettingCalled("islogging"))
            {
                GlobalSettings.AddSetting("islogging", "False");
            }
            if (!GlobalSettings.ContainsSettingCalled("timeout"))
            {
                GlobalSettings.AddSetting("timeout", "2000");
            }
            if (!GlobalSettings.ContainsSettingCalled("timeoutmessage"))
            {
                GlobalSettings.AddSetting("timeoutmessage", "ERROR: The request has timed out.");
            }
            if (!GlobalSettings.ContainsSettingCalled("culture"))
            {
                GlobalSettings.AddSetting("culture", "en-US");
            }
            if (!GlobalSettings.ContainsSettingCalled("splittersfile"))
            {
                GlobalSettings.AddSetting("splittersfile", "Splitters.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("person2substitutionsfile"))
            {
                GlobalSettings.AddSetting("person2substitutionsfile", "Person2Substitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("personsubstitutionsfile"))
            {
                GlobalSettings.AddSetting("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("gendersubstitutionsfile"))
            {
                GlobalSettings.AddSetting("gendersubstitutionsfile", "GenderSubstitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("defaultpredicates"))
            {
                GlobalSettings.AddSetting("defaultpredicates", "DefaultPredicates.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("substitutionsfile"))
            {
                GlobalSettings.AddSetting("substitutionsfile", "Substitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("aimldirectory"))
            {
                GlobalSettings.AddSetting("aimldirectory", "aiml");
            }
            if (!GlobalSettings.ContainsSettingCalled("configdirectory"))
            {
                GlobalSettings.AddSetting("configdirectory", "config");
            }
            if (!GlobalSettings.ContainsSettingCalled("logdirectory"))
            {
                GlobalSettings.AddSetting("logdirectory", "logs");
            }
            if (!GlobalSettings.ContainsSettingCalled("maxlogbuffersize"))
            {
                GlobalSettings.AddSetting("maxlogbuffersize", "64");
            }
            if (!GlobalSettings.ContainsSettingCalled("notacceptinguserinputmessage"))
            {
                GlobalSettings.AddSetting("notacceptinguserinputmessage", "This bot is currently set to not accept user input.");
            }
            if (!GlobalSettings.ContainsSettingCalled("stripperregex"))
            {
                GlobalSettings.AddSetting("stripperregex", "[^0-9a-zA-Z]");
            }

            // Load the dictionaries for this Bot from the various configuration files
            Person2Substitutions.LoadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("person2substitutionsfile")));
            PersonSubstitutions.LoadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("personsubstitutionsfile")));
            GenderSubstitutions.LoadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("gendersubstitutionsfile")));
            DefaultPredicates.LoadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("defaultpredicates")));
            Substitutions.LoadSettings(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("substitutionsfile")));

            // Grab the splitters for this bot
            LoadSplitters(Path.Combine(PathToConfigFiles, GlobalSettings.GrabSetting("splittersfile")));
        }

        /// <summary>
        /// Loads the splitters for this bot from the supplied config file (or sets up some safe defaults)
        /// </summary>
        /// <param name="pathToSplitters">Path to the config file</param>
        private void LoadSplitters(string pathToSplitters)
        {
            FileInfo splittersFile = new FileInfo(pathToSplitters);
            if (splittersFile.Exists)
            {
                XmlDocument splittersXmlDoc = new XmlDocument();
                splittersXmlDoc.Load(pathToSplitters);
                // the XML should have an XML declaration like this:
                // <?xml version="1.0" encoding="utf-8" ?> 
                // followed by a <root> tag with children of the form:
                // <item value="value"/>
                if (splittersXmlDoc.ChildNodes.Count == 2)
                {
                    if (splittersXmlDoc.LastChild.HasChildNodes)
                    {
                        foreach (XmlNode myNode in splittersXmlDoc.LastChild.ChildNodes)
                        {
                            if ((myNode.Name == "item") & (myNode.Attributes.Count == 1))
                            {
                                string value = myNode.Attributes["value"].Value;
                                Splitters.Add(value);
                            }
                        }
                    }
                }
            }
            if (Splitters.Count == 0)
            {
                // we don't have any splitters, so lets make do with these...
                Splitters.Add(".");
                Splitters.Add("!");
                Splitters.Add("?");
                Splitters.Add(";");
            }
        }
        #endregion

        #region Logging methods

        /// <summary>
        /// The last message to be entered into the log (for testing purposes)
        /// </summary>
        public string LastLogMessage = string.Empty;

        /// <summary>
        /// Writes a (timestamped) message to the bot's log.
        /// 
        /// Log files have the form of yyyyMMdd.log.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void WriteToLog(string message)
        {
            LastLogMessage = message;
            if (IsLogging)
            {
                LogBuffer.Add(DateTime.Now.ToString() + ": " + message + Environment.NewLine);
                if (LogBuffer.Count > MaxLogBufferSize - 1)
                {
                    // Write out to log file
                    DirectoryInfo logDirectory = new DirectoryInfo(PathToLogs);
                    if (!logDirectory.Exists)
                    {
                        logDirectory.Create();
                    }

                    string logFileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    FileInfo logFile = new FileInfo(Path.Combine(PathToLogs, logFileName));
                    StreamWriter writer;
                    if (!logFile.Exists)
                    {
                        writer = logFile.CreateText();
                    }
                    else
                    {
                        writer = logFile.AppendText();
                    }

                    foreach (string msg in LogBuffer)
                    {
                        writer.WriteLine(msg);
                    }
                    writer.Close();
                    LogBuffer.Clear();
                }
            }
            if (!Equals(null, WrittenToLog))
            {
                WrittenToLog();
            }
        }

        #endregion

        #region Conversation methods

        /// <summary>
        /// Given some raw input and a unique ID creates a response for a new user
        /// </summary>
        /// <param name="rawInput">the raw input</param>
        /// <param name="UserGUID">an ID for the new user (referenced in the result object)</param>
        /// <returns>the result to be output to the user</returns>
        public Result Chat(string rawInput, string UserGUID)
        {
            Request request = new Request(rawInput, new User(UserGUID, this), this);
            return Chat(request);
        }

        /// <summary>
        /// Given a request containing user input, produces a result from the bot
        /// </summary>
        /// <param name="request">the request from the user</param>
        /// <returns>the result to be output to the user</returns>
        public Result Chat(Request request)
        {
            Result result = new Result(request.User, this, request);

            if (isAcceptingUserInput)
            {
                // Normalize the input
                AIMLLoader loader = new AIMLLoader(this);
                Normalize.SplitIntoSentences splitter = new Normalize.SplitIntoSentences(this);
                string[] rawSentences = splitter.Transform(request.RawInput);
                foreach (string sentence in rawSentences)
                {
                    result.InputSentences.Add(sentence);
                    string path = loader.GeneratePath(sentence, request.User.getLastBotOutput(), request.User.Topic, true);
                    result.NormalizedPaths.Add(path);
                }

                // grab the templates for the various sentences from the graphmaster
                foreach (string path in result.NormalizedPaths)
                {
                    SubQuery query = new SubQuery(path);
                    query.Template = Graphmaster.Evaluate(path, query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }

                // process the templates into appropriate output
                foreach (SubQuery query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            XmlNode templateNode = AIMLTagHandler.GetNode(query.Template);
                            string outputSentence = ProcessNode(templateNode, query, request, result, request.User);
                            if (outputSentence.Length > 0)
                            {
                                result.OutputSentences.Add(outputSentence);
                            }
                        }
                        catch (Exception e)
                        {
                            WriteToLog("WARNING! A problem was encountered when trying to process the input: " + request.RawInput + " with the template: \"" + query.Template + "\"");
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(NotAcceptingUserInputMessage);
            }

            // populate the Result object
            result.Duration = DateTime.Now - request.StartedOn;
            request.User.AddResult(result);

            return result;
        }

        /// <summary>
        /// Recursively evaluates the template nodes returned from the bot
        /// </summary>
        /// <param name="node">the node to evaluate</param>
        /// <param name="query">the query that produced this node</param>
        /// <param name="request">the request from the user</param>
        /// <param name="result">the result to be sent to the user</param>
        /// <param name="user">the user who originated the request</param>
        /// <returns>the output string</returns>
        private string ProcessNode(XmlNode node, SubQuery query, Request request, Result result, User user)
        {
            // check for timeout (to avoid infinite loops)
            if (request.StartedOn.AddMilliseconds(request.Bot.TimeOut) < DateTime.Now)
            {
                request.Bot.WriteToLog("WARNING! Request timeout. User: " + request.User.UserID + " raw input: \"" + request.RawInput + "\" processing template: \"" + query.Template + "\"");
                request.HasTimedOut = true;
                return string.Empty;
            }

            // process the node
            string tagName = node.Name.ToLower();
            if (tagName == "template")
            {
                StringBuilder templateResult = new StringBuilder();
                if (node.HasChildNodes)
                {
                    // recursively check
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        templateResult.Append(ProcessNode(childNode, query, request, result, user));
                    }
                }
                return templateResult.ToString();
            }
            else
            {
                AIMLTagHandler tagHandler = null;
                tagHandler = getBespokeTags(user, query, request, result, node);
                if (Equals(null, tagHandler))
                {
                    switch (tagName)
                    {
                        case "bot":
                            tagHandler = new AIMLTagHandlers.Bot(this, user, query, request, result, node);
                            break;
                        case "condition":
                            tagHandler = new AIMLTagHandlers.Condition(this, user, query, request, result, node);
                            break;
                        case "date":
                            tagHandler = new AIMLTagHandlers.Date(this, user, query, request, result, node);
                            break;
                        case "formal":
                            tagHandler = new AIMLTagHandlers.Formal(this, user, query, request, result, node);
                            break;
                        case "gender":
                            tagHandler = new AIMLTagHandlers.Gender(this, user, query, request, result, node);
                            break;
                        case "get":
                            tagHandler = new AIMLTagHandlers.Get(this, user, query, request, result, node);
                            break;
                        case "gossip":
                            tagHandler = new AIMLTagHandlers.Gossip(this, user, query, request, result, node);
                            break;
                        case "id":
                            tagHandler = new AIMLTagHandlers.Id(this, user, query, request, result, node);
                            break;
                        case "input":
                            tagHandler = new AIMLTagHandlers.Input(this, user, query, request, result, node);
                            break;
                        case "javascript":
                            tagHandler = new AIMLTagHandlers.Javascript(this, user, query, request, result, node);
                            break;
                        case "learn":
                            tagHandler = new AIMLTagHandlers.Learn(this, user, query, request, result, node);
                            break;
                        case "lowercase":
                            tagHandler = new AIMLTagHandlers.Lowercase(this, user, query, request, result, node);
                            break;
                        case "person":
                            tagHandler = new AIMLTagHandlers.Person(this, user, query, request, result, node);
                            break;
                        case "person2":
                            tagHandler = new AIMLTagHandlers.Person2(this, user, query, request, result, node);
                            break;
                        case "random":
                            tagHandler = new AIMLTagHandlers.Random(this, user, query, request, result, node);
                            break;
                        case "sentence":
                            tagHandler = new AIMLTagHandlers.Sentence(this, user, query, request, result, node);
                            break;
                        case "set":
                            tagHandler = new AIMLTagHandlers.Set(this, user, query, request, result, node);
                            break;
                        case "size":
                            tagHandler = new AIMLTagHandlers.Size(this, user, query, request, result, node);
                            break;
                        case "sr":
                            tagHandler = new AIMLTagHandlers.Sr(this, user, query, request, result, node);
                            break;
                        case "srai":
                            tagHandler = new AIMLTagHandlers.Srai(this, user, query, request, result, node);
                            break;
                        case "star":
                            tagHandler = new AIMLTagHandlers.Star(this, user, query, request, result, node);
                            break;
                        case "system":
                            tagHandler = new AIMLTagHandlers.System(this, user, query, request, result, node);
                            break;
                        case "that":
                            tagHandler = new AIMLTagHandlers.That(this, user, query, request, result, node);
                            break;
                        case "thatstar":
                            tagHandler = new AIMLTagHandlers.ThatStar(this, user, query, request, result, node);
                            break;
                        case "think":
                            tagHandler = new AIMLTagHandlers.Think(this, user, query, request, result, node);
                            break;
                        case "topicstar":
                            tagHandler = new AIMLTagHandlers.TopicStar(this, user, query, request, result, node);
                            break;
                        case "uppercase":
                            tagHandler = new AIMLTagHandlers.Uppercase(this, user, query, request, result, node);
                            break;
                        case "version":
                            tagHandler = new AIMLTagHandlers.Version(this, user, query, request, result, node);
                            break;
                        default:
                            tagHandler = null;
                            break;
                    }
                }
                if (Equals(null, tagHandler))
                {
                    return node.InnerText;
                }
                else
                {
                    if (tagHandler.IsRecursive)
                    {
                        if (node.HasChildNodes)
                        {
                            // recursively check
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                if (childNode.NodeType != XmlNodeType.Text)
                                {
                                    childNode.InnerXml = ProcessNode(childNode, query, request, result, user);
                                }
                            }
                        }
                        return tagHandler.Transform();
                    }
                    else
                    {
                        string resultNodeInnerXML = tagHandler.Transform();
                        XmlNode resultNode = AIMLTagHandler.GetNode("<node>" + resultNodeInnerXML + "</node>");
                        if (resultNode.HasChildNodes)
                        {
                            StringBuilder recursiveResult = new StringBuilder();
                            // recursively check
                            foreach (XmlNode childNode in resultNode.ChildNodes)
                            {
                                recursiveResult.Append(ProcessNode(childNode, query, request, result, user));
                            }
                            return recursiveResult.ToString();
                        }
                        else
                        {
                            return resultNode.InnerXml;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Searches the CustomTag collection and processes the AIML if an appropriate tag handler is found
        /// </summary>
        /// <param name="user">the user who originated the request</param>
        /// <param name="query">the query that produced this node</param>
        /// <param name="request">the request from the user</param>
        /// <param name="result">the result to be sent to the user</param>
        /// <param name="node">the node to evaluate</param>
        /// <returns>the output string</returns>
        public AIMLTagHandler getBespokeTags(User user, SubQuery query, Request request, Result result, XmlNode node)
        {
            if (CustomTags.ContainsKey(node.Name.ToLower()))
            {
                TagHandler customTagHandler = CustomTags[node.Name.ToLower()];

                AIMLTagHandler newCustomTag = customTagHandler.Instantiate(LateBindingAssemblies);
                if (Equals(null, newCustomTag))
                {
                    return null;
                }
                else
                {
                    newCustomTag.User = user;
                    newCustomTag.Query = query;
                    newCustomTag.Request = request;
                    newCustomTag.Result = result;
                    newCustomTag.TemplateNode = node;
                    newCustomTag.Bot = this;
                    return newCustomTag;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Saves the graphmaster node (and children) to a binary file to avoid processing the AIML each time the 
        /// bot starts
        /// </summary>
        /// <param name="path">the path to the file for saving</param>
        public void saveToBinaryFile(string path)
        {
            // check to delete an existing version of the file
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }

            FileStream saveFile = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(saveFile, Graphmaster);
            saveFile.Close();
        }

        /// <summary>
        /// Loads a dump of the graphmaster into memory so avoiding processing the AIML files again
        /// </summary>
        /// <param name="path">the path to the dump file</param>
        public void LoadFromBinaryFile(string path)
        {
            FileStream loadFile = File.OpenRead(path);
            BinaryFormatter bf = new BinaryFormatter();
            Graphmaster = (Node)bf.Deserialize(loadFile);
            loadFile.Close();
        }

        #endregion

        #region Latebinding custom-tag dll handlers

        /// <summary>
        /// Loads any custom tag handlers found in the dll referenced in the argument
        /// </summary>
        /// <param name="pathToDLL">the path to the dll containing the custom tag handling code</param>
        public void loadCustomTagHandlers(string pathToDLL)
        {
            Assembly tagDLL = Assembly.LoadFrom(pathToDLL);
            Type[] tagDLLTypes = tagDLL.GetTypes();
            for (int i = 0; i < tagDLLTypes.Length; i++)
            {
                object[] typeCustomAttributes = tagDLLTypes[i].GetCustomAttributes(false);
                for (int j = 0; j < typeCustomAttributes.Length; j++)
                {
                    if (typeCustomAttributes[j] is CustomTagAttribute)
                    {
                        // We've found a custom tag handling class
                        // so store the assembly and store it away in the Dictionary<,> as a TagHandler class for 
                        // later usage

                        // store Assembly
                        if (!LateBindingAssemblies.ContainsKey(tagDLL.FullName))
                        {
                            LateBindingAssemblies.Add(tagDLL.FullName, tagDLL);
                        }

                        // create the TagHandler representation
                        TagHandler newTagHandler = new TagHandler
                        {
                            AssemblyName = tagDLL.FullName,
                            ClassName = tagDLLTypes[i].FullName,
                            TagName = tagDLLTypes[i].Name.ToLower()
                        };
                        if (CustomTags.ContainsKey(newTagHandler.TagName))
                        {
                            throw new Exception("ERROR! Unable to add the custom tag: <" + newTagHandler.TagName + ">, found in: " + pathToDLL + " as a handler for this tag already exists.");
                        }
                        else
                        {
                            CustomTags.Add(newTagHandler.TagName, newTagHandler);
                        }
                    }
                }
            }
        }
        #endregion
       
    }
}
