using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace DC22.Shell.Help
{
    
    public static class Help
    {
        private class HelpItem
        {
            private string m_id;
            public string Id
            {
                get { return m_id; }
                set { m_id = value; }
            }

            private string m_value;
            public string Value
            {
                get { return m_value; }
                set { m_value = value; }
            }

            private string m_text;
            public string Text
            {
                get { return m_text; }
                set { m_text = value; }
            }
        }

        private class NestedDictionary<TKey, TValue> : Dictionary<TKey, NestedDictionary<TKey, TValue>>
        {
            public TValue Value { set; get; }

            public new NestedDictionary<TKey, TValue> this[TKey key]
            {
                set { base[key] = value; }
                get
                {
                    if (!base.ContainsKey(key))
                    {
                        base[key] = new NestedDictionary<TKey, TValue>();
                    }
                    return base[key];
                }
            }
        }

        private static int level = 0;
        private static NestedDictionary<string, string> helpDictionary;
        private static List<string> parentIds = new List<string>();
       
        public static void LoadHelpDictionary()
        {
            XmlDocument xmlDoc = new XmlDocument();

            string helpPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetName().CodeBase);
            string helpFileName = String.Format(@"Help\Help_{0}.xml", DC22.Shell.PL.DataTools.CurrentLanguage);
            string path = Path.Combine(helpPath, helpFileName);
            xmlDoc.Load(path);
            helpDictionary = new NestedDictionary<string, string>();
            XmlNode x = xmlDoc.DocumentElement;
               
            DisplayNodes(x, ref parentIds);
        }

        public static string getHelpText(string key, string parentKey)
        {
            string helpText = "";

            if (!String.IsNullOrEmpty(parentKey))
            {
                Dictionary<string, NestedDictionary<string, string>> helpDictionaryParent = null;
                getHelpDictionary(parentKey, helpDictionary, ref helpDictionaryParent);
                if ((helpDictionaryParent != null) && !String.IsNullOrEmpty(key))
                {
                    if (helpDictionaryParent.ContainsKey(key))
                    {
                        helpText = helpDictionaryParent[key]["help_text"].Value;
                    }
                }
                if ((helpDictionaryParent != null) && String.IsNullOrEmpty(helpText))
                {
                    helpText = helpDictionaryParent["help_text"].Value;
                }
            }

            if (String.IsNullOrEmpty(helpText))
            {
                getHelpDictionaryValue("main_menu", helpDictionary, ref helpText);
            }
            return helpText;
        }

        private static void getHelpDictionary(string key, Dictionary<string, NestedDictionary<string, string>> dictionary, ref Dictionary<string, NestedDictionary<string, string>> helpDictionary)
        {
            foreach (string dictionaryKey in dictionary.Keys)
            {
                if (dictionaryKey == key)
                {
                    helpDictionary = dictionary[dictionaryKey];
                    break;
                }
                object nextLevel = dictionary[dictionaryKey];
                if (nextLevel == null)
                {
                    continue;
                }
                getHelpDictionary(key, (Dictionary<string, NestedDictionary<string, string>>)nextLevel, ref helpDictionary);
            }
        }

        private static void getHelpDictionaryValue(string key, Dictionary<string, NestedDictionary<string, string>> dictionary, ref string text)
        {
            foreach (string dictionaryKey in dictionary.Keys)
            {
                if (dictionaryKey == key)
                {
                   text = ((NestedDictionary<string, string>)dictionary[dictionaryKey])["help_text"].Value;
                   break;
                }
                object nextLevel = dictionary[dictionaryKey];
                if (nextLevel == null)
                {
                    continue;
                }
                getHelpDictionaryValue(key, (Dictionary<string, NestedDictionary<string, string>>)nextLevel, ref text);
            }
        }

        private static void DisplayNodes(XmlNode node, ref List<string> Ids)
        {
            //Print attributes of the node
            if (node.Attributes != null)
            {
                XmlAttributeCollection attrs = node.Attributes;
                string nodeId = node.Attributes["id"].Value;
                string nodeText = node.Attributes["text"].Value;
                if (level == 0)
                {
                    helpDictionary[nodeId]["help_text"].Value = nodeText;
                }
                if (level == 1)
                {
                    helpDictionary[Ids[0]][nodeId]["help_text"].Value = nodeText;
                }
                if (level == 2)
                {
                    helpDictionary[Ids[0]][Ids[1]][nodeId]["help_text"].Value = nodeText;
                }
                if (level == 3)
                {
                    helpDictionary[Ids[0]][Ids[1]][Ids[2]][nodeId]["help_text"].Value = nodeText;
                }
                if (level == 4)
                {
                    helpDictionary[Ids[0]][Ids[1]][Ids[2]][Ids[3]][nodeId]["help_text"].Value = nodeText;
                }
            }
            
            //Print individual children of the node
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                XmlAttributeCollection attrs = child.Attributes;
                string childId = node.Attributes["id"].Value;

                if (Ids.Count != 0 && Ids.Contains(childId))
                {
                    level = Ids.IndexOf(childId) + 1;
                }
                else
                {
                    if (Ids.Count <= level)
                    {
                        Ids.Insert(level, childId);
                    }
                    else
                    {
                        Ids[level] = childId;
                    }
                    level++;
                }

                DisplayNodes(child, ref Ids);
            }
        }
    }
}
