using System;
using System.IO;
using System.Data;

namespace WindowsFormsApp1
{
    public static class DataStore
    {
        private static string dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");

        public static void Load(DataSet1 ds)
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    ds.ReadXml(dataFilePath);
                }
            }
            catch (Exception)
            {
                // Ignore errors here; caller may show messages
            }
        }

        public static void Save(DataSet1 ds)
        {
            try
            {
                ds.WriteXml(dataFilePath, System.Data.XmlWriteMode.WriteSchema);
            }
            catch (Exception)
            {
                // Ignore errors here; caller may show messages
            }
        }
    }
}
