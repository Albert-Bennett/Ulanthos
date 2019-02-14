using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Ulanthos.Framework
{
    /// <summary>
    /// An Xml serialization helper.
    /// </summary>
    public class XMLSerializer
    {
        /// <summary>
        /// Used to serialize data.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="filePath">The desired filpath for the file to reside in.</param>
        /// <param name="fileExtention">The ending of the filepath ie .xml.</param>
        public static void Serialize<T>(object obj, string filePath, string fileExtention)
        {
            using (FileStream stream = File.Open(filePath + fileExtention, FileMode.Create))
            {
                XmlSerializer serial = new XmlSerializer(typeof(T));
                serial.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Used to serialize data.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="filePath">The desired filpath for the file to reside in.</param>
        public static void Serialize<T>(object obj, string filePath)
        {
            Serialize<T>(obj, filePath, ".xml");
        }

        /// <summary>
        /// Used to deserialize data.
        /// </summary>
        /// <param name="filePath">The filepath where the xml file is in.</param>
        /// <param name="fileExtention">The file extention.</param>
        /// <returns>The loaded in data.</returns>
        public static T Deserialize<T>(string filePath, string fileExtention)
        {
            using (XmlReader reader = XmlReader.Create(filePath + fileExtention))
            {
                XmlSerializer serial = new XmlSerializer(typeof(T));
                return (T)serial.Deserialize(reader);
            }
        }
    }
}
