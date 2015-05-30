using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace JmDict
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static JMdict sDic;

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            var ser = new XmlSerializer(typeof(JMdict));
            using (var fs = new FileStream(@"JMdict_e.gz", FileMode.Open, FileAccess.Read))
            {
                using (var gz = new GZipStream(fs, CompressionMode.Decompress))
                {
                    var settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    using (var reader = XmlReader.Create(gz, settings))
                    {
                        sDic = (JMdict)ser.Deserialize(reader);
                    }
                }
            }

            base.OnStartup(eventArgs);
        }
    }
}
