using Austin.MecabSharp;
using Microsoft.International.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace JmDict
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JMdict mDic;
        Tagger mTagger = new Tagger();
        Dictionary<string, List<entry>> kLookup, rLookup;
        Dictionary<char, char> mKataToHira = new Dictionary<char, char>();
        Task mLoader;

        public MainWindow()
        {
            mLoader = Task.Run(new Action(loadData));
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await mLoader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to load");
                Application.Current.Shutdown();
                return;
            }

            txtInput.Text = string.Empty;
            txtInput.IsEnabled = true;
            txtInput.TextChanged += txtInput_TextChanged;
        }

        private void loadData()
        {
            using (Stream convStream = typeof(KanaConverter).Assembly.GetManifestResourceStream("Microsoft.International.Converters.KatakanaToHiragana.xml"))
            {
                var doc = XDocument.Load(convStream);
                foreach (var conversion in doc.Root.Element("ConversionTable").Elements("Conversion"))
                {
                    var input = conversion.Attribute("Input").Value;
                    var output = conversion.Attribute("Output").Value;
                    if (input.Length != 1 || output.Length != 1)
                        continue; //assume dictionary entries are precomposed
                    mKataToHira.Add(input[0], output[0]);
                }
            }

            var ser = new XmlSerializer(typeof(JMdict));
            using (var fs = new FileStream(@"JMdict_e.gz", FileMode.Open, FileAccess.Read))
            {
                using (var gz = new GZipStream(fs, CompressionMode.Decompress))
                {
                    var settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Parse;
                    using (var reader = XmlReader.Create(gz, settings))
                    {
                        mDic = (JMdict)ser.Deserialize(reader);
                    }
                }
            }

            kLookup = CreateLookup(e => e.k_ele, k => k.keb);
            rLookup = CreateLookup(e => e.r_ele, r => fromKataToHira(r.reb));

            //Stats(e => e.k_ele, k => k.keb);
            //Stats(e => e.r_ele, r => r.reb);
        }

        static string GetJmPartOfSpeach(Node n)
        {
            switch (n.PartOfSpeech)
            {
                case "助詞":
                    return "particle";
                case "名詞": //noun
                    if (n.PosSub1 == "代名詞")
                        return "pronoun";
                    else if (n.PosSub1 == "一般")
                        return "noun (common) (futsuumeishi)";
                    else if (n.PosSub1 == "接尾")
                    {
                        if (n.PosSub2 == "助数詞")
                            return "counter";
                        return "noun, used as a suffix";
                    }
                    else if (n.PosSub1 == "サ変接続")
                        return "noun or participle which takes the aux. verb suru"; //TODO: need to confirm
                    else if (n.PosSub1 == "形容動詞語幹")
                        return "adjectival nouns or quasi-adjectives (keiyodoshi)";
                    else if (n.PosSub1 == "数")
                        return "numeric";
                    return null;
                case "動詞": //verb
                    if (n.ConjugatedForm == "一段")
                        return "Ichidan verb";
                    else if (n.ConjugatedForm.StartsWith("五段・"))
                    {
                        switch (n.ConjugatedForm.Substring(3, 2))
                        {
                            case "ワ行":
                                return "Godan verb with `u' ending";
                            case "カ行":
                                return "Godan verb with `ku' ending";
                            case "ガ行":
                                return "Godan verb with `gu' ending";
                            case "サ行":
                                return "Godan verb with `su' ending";
                            case "タ行":
                                return "Godan verb with `tsu' ending";
                            case "ナ行":
                                return "Godan verb with `nu' ending";
                            case "バ行":
                                return "Godan verb with `bu' ending";
                            case "マ行":
                                return "Godan verb with `mu' ending";
                            case "ラ行":
                                if (n.ConjugatedForm == "五段・ラ行特殊")
                                    return "Godan verb - -aru special class";
                                else
                                    return "Godan verb with `ru' ending";
                        }
                    }
                    return null;
                case "記号": //symbol
                    return null;
                case "助動詞":
                    return "auxiliary verb";
                case "形容詞":
                    return "adjective (keiyoushi)";
                default:
                    return null;
            }
        }

        string fromKataToHira(string maybeKata)
        {
            char[] chars = maybeKata.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char hira;
                if (mKataToHira.TryGetValue(chars[i], out hira))
                    chars[i] = hira;
            }
            return new string(chars);
        }

        Dictionary<string, List<entry>> CreateLookup<T>(Func<entry, T[]> getReadings, Func<T, string> getReadingText)
        {
            var map = new Dictionary<string, List<entry>>();
            foreach (var e in mDic.entry)
            {
                var readings = getReadings(e);
                if (readings == null)
                    continue;

                foreach (var reading in readings.Select(getReadingText))
                {
                    if (!map.ContainsKey(reading))
                        map.Add(reading, new List<entry>());
                    map[reading].Add(e);
                }
            }

            return map;
        }

        void Stats<T>(Func<entry, T[]> getReadings, Func<T, string> getReadingText)
        {
            var map = new Dictionary<string, List<entry>>();
            foreach (var e in mDic.entry)
            {
                if (e.sense.Where(s => s.misc != null && s.misc.Any(m => m.Contains("archaic") || m.Equals("archaism"))).Count() == e.sense.Length)
                    continue;

                var readings = getReadings(e);
                if (readings == null)
                    continue;

                foreach (var reading in readings.Select(getReadingText))
                {
                    if (!map.ContainsKey(reading))
                        map.Add(reading, new List<entry>());
                    map[reading].Add(e);
                }
            }

            var distro = map.Values.GroupBy(v => v.Count)
                .Select(k => new { Bucket = k.Key, Number = k.Count(), Entries = k.ToArray() })
                .OrderByDescending(e => e.Bucket).ToList();
        }

        class MyEntry
        {
            public k_ele[] k_ele { get; set; }
            public r_ele[] r_ele { get; set; }
            public string[] Meaning { get; set; }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs _)
        {
            var split = mTagger.Parse(txtInput.Text);
            var niceEntries = new List<MyEntry>();

            foreach (var morph in split)
            {
                string jmPos = GetJmPartOfSpeach(morph);
                if (jmPos == null)
                    continue;

                var reading = fromKataToHira(morph.Reading);

                List<entry> ent;
                if (kLookup.TryGetValue(morph.BaseForm, out ent) || rLookup.TryGetValue(reading, out ent))
                {
                    if (ent.Count != 1)
                    {
                        //try to narrow it down by reading
                        var entriesWithCorrectReading = ent.Where(e => e.r_ele != null && e.r_ele.Any(r => r.reb == reading)).ToList();
                        if (entriesWithCorrectReading.Count == 0)
                        {
                            Console.WriteLine();
                        }
                        else
                        {
                            ent = entriesWithCorrectReading;
                        }
                    }

                    var entriesWithTheCorrectPartOfSpeech = ent.Where(e => e.sense.Any(s => s.pos != null && s.pos.Any(p => p.StartsWith(jmPos)))).ToList();

                    if (entriesWithTheCorrectPartOfSpeech.Count == 1)
                    {
                        var n = entriesWithTheCorrectPartOfSpeech[0];
                        var e = new MyEntry();
                        e.k_ele = n.k_ele;
                        e.r_ele = n.r_ele;
                        e.Meaning = n.sense
                            .Where(s => s.pos != null && s.pos.Any(p => p.StartsWith(jmPos)))
                            .SelectMany(s => s.gloss.SelectMany(g => g.Text))
                            .ToArray();
                        niceEntries.Add(e);
                    }
                    else if (ent.Count > 1)
                    {
                        var e = new MyEntry();
                        e.k_ele = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.k_ele ?? new k_ele[0]).ToArray();
                        e.r_ele = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.r_ele ?? new r_ele[0]).ToArray();
                        e.Meaning = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.sense ?? new sense[0])
                            .Where(s => s.pos != null && s.pos.Any(p => p.StartsWith(jmPos)))
                            .SelectMany(s => s.gloss.SelectMany(g => g.Text))
                            .ToArray();
                        niceEntries.Add(e);

                        Console.WriteLine();
                        //todo, support multiple meanings in gui
                    }

                }
                else
                {
                    Console.WriteLine();
                }
            }

            lv.ItemsSource = niceEntries;
        }
    }
}
