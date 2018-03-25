using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using InsuranceBotMaster.AIML.Normalize;

namespace InsuranceBotMaster.AIML.Utils
{
    /// <summary>
    /// A bespoke Dictionary<,> for loading, adding, checking, removing and extracting
    /// settings.
    /// </summary>
    public class SettingsDictionary
    {
        #region Attributes

        /// <summary>
        /// Holds a dictionary of settings
        /// </summary>
        private Dictionary<string, string> _settingsHash = new Dictionary<string, string>();

        /// <summary>
        /// Contains an ordered collection of all the keys (unfortunately Dictionary<,>s are
        /// not ordered)
        /// </summary>
        private List<string> _orderedKeys = new List<string>();

        /// <summary>
        /// The bot this dictionary is associated with
        /// </summary>
        protected Bot Bot;

        /// <summary>
        /// The number of items in the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return _orderedKeys.Count;
            }
        }

        /// <summary>
        /// An XML representation of the contents of this dictionary
        /// </summary>
        public XmlDocument DictionaryAsXML
        {
            get
            {
                XmlDocument result = new XmlDocument();
                XmlDeclaration dec = result.CreateXmlDeclaration("1.0", "UTF-8", "");
                result.AppendChild(dec);
                XmlNode root = result.CreateNode(XmlNodeType.Element, "root", "");
                result.AppendChild(root);
                foreach (string key in _orderedKeys)
                {
                    XmlNode item = result.CreateNode(XmlNodeType.Element, "item", "");
                    XmlAttribute name = result.CreateAttribute("name");
                    name.Value = key;
                    XmlAttribute value = result.CreateAttribute( "value");
                    value.Value = _settingsHash[key];
                    item.Attributes.Append(name);
                    item.Attributes.Append(value);
                    root.AppendChild(item);
                }
                return result;
            }
        }

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot for whom this is a settings dictionary</param>
        public SettingsDictionary(Bot bot)
        {
            Bot = bot;
        }

        #region Methods
        /// <summary>
        /// Loads bespoke settings into the class from the file referenced in pathToSettings.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="pathToSettings">The file containing the settings</param>
        public void LoadSettings(string pathToSettings)
        {
            if (pathToSettings.Length > 0)
            {
                FileInfo fi = new FileInfo(pathToSettings);
                if (fi.Exists)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(pathToSettings);
                    LoadSettings(xmlDoc);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// Loads bespoke settings to the class from the XML supplied in the args.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="settingsAsXML">The settings as an XML document</param>
        public void LoadSettings(XmlDocument settingsAsXML)
        {
            // empty the hash
            ClearSettings();

            XmlNodeList rootChildren = settingsAsXML.DocumentElement.ChildNodes;

            foreach (XmlNode myNode in rootChildren)
            {
                if ((myNode.Name == "item") & (myNode.Attributes.Count == 2))
                {
                    if ((myNode.Attributes[0].Name == "name") & (myNode.Attributes[1].Name == "value"))
                    {
                        string name = myNode.Attributes["name"].Value;
                        string value = myNode.Attributes["value"].Value;
                        if (name.Length > 0)
                        {
                            AddSetting(name, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a bespoke setting to the Settings class (accessed via the grabSettings(string name)
        /// method.
        /// </summary>
        /// <param name="name">The name of the new setting</param>
        /// <param name="value">The value associated with this setting</param>
        public void AddSetting(string name, string value)
        {
            string key = MakeCaseInsensitive.TransformInput(name);
            if (key.Length > 0)
            {
                RemoveSetting(key);
                _orderedKeys.Add(key);
                _settingsHash.Add(MakeCaseInsensitive.TransformInput(key), value);
            }
        }

        /// <summary>
        /// Removes the named setting from this class
        /// </summary>
        /// <param name="name">The name of the setting to remove</param>
        public void RemoveSetting(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            _orderedKeys.Remove(normalizedName);
            RemoveFromHash(normalizedName);
        }

        /// <summary>
        /// Removes a named setting from the Dictionary<,>
        /// </summary>
        /// <param name="name">the key for the Dictionary<,></param>
        private void RemoveFromHash(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            _settingsHash.Remove(normalizedName);
        }

        /// <summary>
        /// Updates the named setting with a new value whilst retaining the position in the
        /// dictionary
        /// </summary>
        /// <param name="name">the name of the setting</param>
        /// <param name="value">the new value</param>
        public void UpdateSetting(string name, string value)
        {
            string key = MakeCaseInsensitive.TransformInput(name);
            if (_orderedKeys.Contains(key))
            {
                RemoveFromHash(key);
                _settingsHash.Add(MakeCaseInsensitive.TransformInput(key), value);
            }
        }

        /// <summary>
        /// Clears the dictionary to an empty state
        /// </summary>
        public void ClearSettings()
        {
            _orderedKeys.Clear();
            _settingsHash.Clear();
        }

        /// <summary>
        /// Returns the value of a setting given the name of the setting
        /// </summary>
        /// <param name="name">the name of the setting whose value we're interested in</param>
        /// <returns>the value of the setting</returns>
        public string GrabSetting(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            if (ContainsSettingCalled(normalizedName))
            {
                return _settingsHash[normalizedName];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks to see if a setting of a particular name exists
        /// </summary>
        /// <param name="name">The setting name to check</param>
        /// <returns>Existential truth value</returns>
        public bool ContainsSettingCalled(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            if (normalizedName.Length > 0)
            {
                return _orderedKeys.Contains(normalizedName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a collection of the names of all the settings defined in the dictionary
        /// </summary>
        /// <returns>A collection of the names of all the settings defined in the dictionary</returns>
        public string[] SettingNames
        {
            get
            {
                string[] result = new string[_orderedKeys.Count];
                _orderedKeys.CopyTo(result, 0);
                return result;
            }
        }

        /// <summary>
        /// Copies the values in the current object into the SettingsDictionary passed as the target
        /// </summary>
        /// <param name="target">The target to recieve the values from this SettingsDictionary</param>
        public void Clone(SettingsDictionary target)
        {
            foreach (string key in _orderedKeys)
            {
                target.AddSetting(key, GrabSetting(key));
            }
        }
        #endregion
    }
}
