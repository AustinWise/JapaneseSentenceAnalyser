using Austin.MecabSharp;
using Microsoft.International.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JmDict
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Tagger mTagger = new Tagger();

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
                        return "noun, used as a suffix";
                    return null;
                case "動詞": //verb
                    if (n.ConjugatedForm == "一段")
                        return "Ichidan verb";
                    else if (n.ConjugatedForm.StartsWith("五段・"))
                    {
                        switch (n.ConjugatedForm.Substring(3))
                        {
                            case "ラ行":
                                return "Godan verb with `ru' ending";
                        }
                    }
                    return null;
                case "記号": //symbol
                    return null;
                default:
                    return null;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            var kLookup = CreateLookup(e => e.k_ele, k => k.keb);
            var rLookup = CreateLookup(e => e.r_ele, r => r.reb);

            var split = mTagger.Parse("私は亀に興味があります。");
            var niceEntries = new List<MyEntry>();

            foreach (var morph in split)
            {
                string jmPos = GetJmPartOfSpeach(morph);
                if (jmPos == null)
                    continue;

                List<entry> ent;
                if (kLookup.TryGetValue(morph.BaseForm, out ent) || rLookup.TryGetValue(morph.BaseForm, out ent))
                {
                    var reading = KanaConverter.KatakanaToHiragana(morph.Reading);
                    var entriesWithCorrectReading = ent.Where(e => e.r_ele != null && e.r_ele.Any(r => r.reb == reading)).ToList();

                    if (entriesWithCorrectReading.Count == 0)
                        continue;

                    var entriesWithTheCorrectPartOfSpeech = entriesWithCorrectReading.Where(e => e.sense.Any(s => s.pos != null && s.pos.Any(p => p.StartsWith(jmPos)))).ToList();

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
                    else if (entriesWithCorrectReading.Count > 1)
                    {
                        //todo, support multiple meanings in gui
                    }

                }
            }

            lv.ItemsSource = niceEntries;

            //Stats(e => e.k_ele, k => k.keb);
            //Stats(e => e.r_ele, r => r.reb);


            Console.WriteLine();
        }

        static Dictionary<string, List<entry>> CreateLookup<T>(Func<entry, T[]> getReadings, Func<T, string> getReadingText)
        {
            var map = new Dictionary<string, List<entry>>();
            foreach (var e in App.sDic.entry)
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

        static void Stats<T>(Func<entry, T[]> getReadings, Func<T, string> getReadingText)
        {
            var map = new Dictionary<string, List<entry>>();
            foreach (var e in App.sDic.entry)
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
    }
}
