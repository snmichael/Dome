using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Demo.Framework.Core
{
    public class SerializerUtility
    {
        #region XmlSerializer
        public static string XmlSerializer<T>(T obj) where T : class, new()
        {
            return XmlSerializer<T>(obj, "");
        }

        public static string XmlSerializer(object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = null;

            serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            serializer.Serialize(sw, obj);
            sw.Close();
            return sb.ToString();
        }

        public static string XmlSerializer<T>(T obj, string defaultnamespace) where T : class, new()
        {
            System.Xml.Serialization.XmlSerializer serializer = null;
            if (!string.IsNullOrEmpty(defaultnamespace))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), defaultnamespace);
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            }
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            serializer.Serialize(sw, obj);
            sw.Close();
            return sb.ToString();
        }

        public static string XmlSerializerPure(object obj)
        {
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //Add an empty namespace and empty value
            ns.Add("", "");
            //Create the serializer
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //Serialize the object with our own namespaces (notice the overload)
            StringBuilder sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            // Remove the <?xml version="1.0" encoding="utf-8"?>
            settings.OmitXmlDeclaration = true;
            var sw = new System.IO.StringWriter(sb);
            XmlWriter writer = XmlWriter.Create(sw, settings);
            serializer.Serialize(writer, obj, ns);
            sw.Close();
            return sb.ToString();
        }

        public static string XmlSerializerToFile<T>(T obj, string defaultnamespace, string filepath) where T : class, new()
        {
            var content = XmlSerializer(obj);
            System.IO.File.WriteAllText(filepath, content);
            return content;
        }

        public static string XmlSerializerToFile<T>(T obj, string filepath) where T : class, new()
        {
            return XmlSerializerToFile(obj, "", filepath);
        }

        public static T XmlDeserialize<T>(string xmlstring) where T : class, new()
        {
            return XmlDeserialize<T>(xmlstring, "");
        }

        public static T XmlDeserialize<T>(string xmlstring, string defaultnamespace) where T : class, new()
        {

            System.Xml.Serialization.XmlSerializer serializer = null;
            if (!string.IsNullOrEmpty(defaultnamespace))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), defaultnamespace);
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            }

            System.IO.StringReader sr = new System.IO.StringReader(xmlstring);
            var obj = serializer.Deserialize(sr) as T;
            sr.Close();
            return obj;
        }

        public static T XmlDeserializeFromFile<T>(string filepath, string defaultnamespace) where T : class, new()
        {
            if (!System.IO.File.Exists(filepath))
            {
                throw new System.IO.FileNotFoundException("file not find!", filepath);
            }
            System.Xml.Serialization.XmlSerializer serializer = null;
            if (string.IsNullOrEmpty(defaultnamespace))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), defaultnamespace);
            }

            System.IO.StreamReader sr = new System.IO.StreamReader(filepath);
            var obj = serializer.Deserialize(sr) as T;
            sr.Close();
            return obj;
        }

        public static T XmlDeserializeFromFile<T>(string filepath) where T : class,new()
        {

            return XmlDeserializeFromFile<T>(filepath, "");
        }
        #endregion

        #region BinarySerializer
        public static Byte[] BinarySerializer<T>(T obj) where T : class, new()
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            binFormatter.Serialize(ms, obj);
            ms.Flush();
            ms.Close();
            return ms.ToArray();
        }
        public static void BinarySerializerToFile<T>(T obj, string file) where T : class, new()
        {

            BinaryFormatter binFormatter = new BinaryFormatter();
            var stream = new System.IO.FileStream(file, System.IO.FileMode.OpenOrCreate);
            binFormatter.Serialize(stream, obj);
            stream.Flush();
            stream.Close();
        }

        public static T BinaryDeserialize<T>(byte[] bytes) where T : class, new()
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            var obj = binFormatter.Deserialize(ms) as T;
            ms.Flush();
            ms.Close();
            return obj;
        }
        public static T BinaryDeserializeFromFile<T>(string file) where T : class, new()
        {
            if (!System.IO.File.Exists(file))
            {
                throw new System.IO.FileNotFoundException("The file can't be found;");
            }

            BinaryFormatter binFormatter = new BinaryFormatter();
            var stream = new System.IO.FileStream(file, System.IO.FileMode.Open);
            var obj = binFormatter.Deserialize(stream) as T;
            stream.Flush();
            stream.Close();
            return obj;
        }
        #endregion


    }
}
