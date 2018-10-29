using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Vision {
    /// <summary>
    ///     Plugin options will be loaded automatically when this is created.
    /// </summary>
    public static class PluginOptions {
        //this is used to load and save options to the correct folder
        public static readonly  string PluginPath   = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        public static readonly  string CascadesPath = PluginPath + "Cascades\\";
        private static readonly string optionsPath  = PluginPath + "Options.xml";

        // user-side options
        public static          int                     CameraDelayMs = 500;
        public static          bool                    UseImageCorrection;
        public static readonly Dictionary<int, string> PeopleFaces = new Dictionary<int, string>();

        static PluginOptions() {
            //find the correct folder and store it for later use by the load and save methods
            if (File.Exists(optionsPath)) {
                LoadOptionsFromXml();
            }
        }

        private static void LoadOptionsFromXml() {
            var optionsXml = new XmlDocument();
            optionsXml.Load(optionsPath);
            foreach (XmlNode option in optionsXml.DocumentElement.SelectNodes("Option")) {
                string optionValue = option.Attributes["value"].Value;
                switch (option.Attributes["name"].Value) {
                    case nameof(CameraDelayMs):
                        CameraDelayMs = Convert.ToInt32(optionValue) != 0 ? Convert.ToInt32(optionValue) : 500;
                        break;
                    case nameof(UseImageCorrection):
                        UseImageCorrection = Convert.ToBoolean(optionValue);
                        break;
                }
            }

            for (var i = 0; i < optionsXml.DocumentElement.SelectNodes("Faces/Face").Count; i++) {
                XmlNode faceNode = optionsXml.DocumentElement.SelectNodes("Faces/Face")[i];
                PeopleFaces.Add(
                    Convert.ToInt32(faceNode.Attributes["id"].Value),
                    faceNode.Attributes["name"].Value
                );
            }
        }

        public static void SaveOptionsToXml() {
            using (var writer = new XmlTextWriter(optionsPath, new UTF8Encoding()) {
                Formatting  = Formatting.Indented,
                Indentation = 4
            }) {
                writer.WriteStartDocument();
                writer.WriteComment("Vision plugin options");
                writer.WriteStartElement("Options");
                {
                    SaveOptionNodeToXml(writer, nameof(CameraDelayMs),      CameraDelayMs.ToString());
                    SaveOptionNodeToXml(writer, nameof(UseImageCorrection), UseImageCorrection.ToString());
                    writer.WriteStartElement("Faces");
                    {
                        for (var i = 0; i < PeopleFaces.Count; i++) {
                            SaveFaceNodeToXml(writer, PeopleFaces.ElementAt(i).Key.ToString(), PeopleFaces.ElementAt(i).Value);
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // options
                writer.WriteEndDocument();
            }
        }

        private static void SaveOptionNodeToXml(XmlWriter writer, string nodename, string nodevalue) {
            writer.WriteStartElement("Option");
            writer.WriteAttributeString("name",  nodename);
            writer.WriteAttributeString("value", nodevalue);
            writer.WriteEndElement();
        }

        private static void SaveFaceNodeToXml(XmlWriter writer, string faceId, string faceName) {
            writer.WriteStartElement("Face");
            writer.WriteAttributeString("name", faceName);
            writer.WriteAttributeString("id",   faceId);
            writer.WriteEndElement();
        }
    }
}