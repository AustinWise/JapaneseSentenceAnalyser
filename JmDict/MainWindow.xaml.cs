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
                    else if (n.PosSub1 == "サ変接続")
                        return "noun or participle which takes the aux. verb suru"; //TODO: need to confirm
                    else if (n.PosSub1 == "形容動詞語幹")
                        return "adjectival nouns or quasi-adjectives (keiyodoshi)";
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

        Dictionary<string, List<entry>> kLookup, rLookup;
        Dictionary<char, char> mKataToHira = new Dictionary<char, char>();

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

        public MainWindow()
        {
            InitializeComponent();

            foreach (var line in Properties.Resources.TheKana.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line[0] == '#')
                    continue;
                string[] splits = line.Split(',');
                string romanji = splits[0];
                string hiragana = splits[1];
                string katakana = splits[2];
                mKataToHira.Add(katakana[0], hiragana[0]);
            }

            kLookup = CreateLookup(e => e.k_ele, k => k.keb);
            rLookup = CreateLookup(e => e.r_ele, r => fromKataToHira(r.reb));

            //Stats(e => e.k_ele, k => k.keb);
            //Stats(e => e.r_ele, r => r.reb);
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

        private void txtInput_TextChanged(object sender, TextChangedEventArgs _)
        {
            var split = mTagger.Parse(txtInput.Text);
            var niceEntries = new List<MyEntry>();

            foreach (var morph in split)
            {
                string jmPos = GetJmPartOfSpeach(morph);
                if (jmPos == null)
                    continue;

                List<entry> ent;
                if (kLookup.TryGetValue(morph.BaseForm, out ent) || rLookup.TryGetValue(morph.BaseForm, out ent))
                {

                    if (ent.Count != 1)
                    {
                        //try to narrow it down by reading
                        var reading = KanaConverter.KatakanaToHiragana(morph.Reading);
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
                        e.k_ele = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.k_ele).ToArray();
                        e.r_ele = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.r_ele).ToArray();
                        e.Meaning = entriesWithTheCorrectPartOfSpeech.SelectMany(n => n.sense)
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
