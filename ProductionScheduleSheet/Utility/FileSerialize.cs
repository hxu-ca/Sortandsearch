using System;
using System.IO;
using System.Xml.Serialization;

namespace ProductionScheduleSheet.Utility
{
    public static class FileSerialize
    {
        public const string FirstWriteExtension = ".bak";
        public static void SafeXmlSerializeToFile<T>(string filePath, T objectToSerialize)
        {
            var firstWrittenFilePath = filePath + FirstWriteExtension;

            // Start by copying the first written path if the regular file is missing for any reason
            if (!File.Exists(filePath) && File.Exists(firstWrittenFilePath))
            {
                File.Copy(firstWrittenFilePath, filePath);
            }

            var xmlSerializer = new XmlSerializer(typeof(T));
            // Write to first file path
            using (var file = new StreamWriter(firstWrittenFilePath, false))
            {
                xmlSerializer.Serialize(file, objectToSerialize);
            }

            // Copy the first written file to the regular file path with overwrite
            File.Copy(firstWrittenFilePath, filePath, true);
        }

        public static T SafeXmlDeserializeFromFile<T>(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            try
            {
                // Try the regular file path
                using (var file = new StreamReader(filePath, false))
                {
                    var settings = (T)xmlSerializer.Deserialize(file);
                    return settings;
                }
            }
            catch (Exception e)
            {
                // Try the first written to path
                var firstWrittenFilePath = filePath + FirstWriteExtension;
                using (var file = new StreamReader(firstWrittenFilePath, false))
                {
                    var settings = (T)xmlSerializer.Deserialize(file);
                    return settings;
                }
            }
        }
    }
}