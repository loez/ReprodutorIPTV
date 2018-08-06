using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Reprodutor_Video;
using System.Net.Http;

namespace Reprodutor_Video
{
    public partial class MainPage : MasterDetailPage
    {
        private const string LINE_PREFIX = "#EXTINF:-1";
        public string logocanal;
        public string nomecanal;
        private const string REGEX_KEY_VALUE = @"(?i)(?<=\b{0}="")[^""]*";
        private const string REGEX_CHANNEL_NAME = @"(?<=\"",)[^\n]*";

        private const string TVGID_KEY = "tvg-id";
        private const string TVGNAME_KEY = "tvg-name";
        private const string TVGLOGO_KEY = "tvg-logo";
        private const string GROUPTITLE_KEY = "group-title";
        public List<M3uLine> ParsedLines { get; set; }

        public MainPage()
        {
            InitializeComponent();      
        }


        public M3uLine ParseLine(string line)
        {
            var m3uLine = new M3uLine();
            m3uLine.TvgID = GetValueFromKey(line, TVGID_KEY);
            m3uLine.TvgName = GetValueFromKey(line, TVGNAME_KEY);
            m3uLine.TvgLogo = GetValueFromKey(line, TVGLOGO_KEY);
            m3uLine.GroupTitle = GetValueFromKey(line, GROUPTITLE_KEY);
            m3uLine.Name = GetValueFromRegex(line, REGEX_CHANNEL_NAME);

            return m3uLine;
        }

        private string GetValueFromKey(string line, string key)
        {
            return GetValueFromRegex(line, string.Format(REGEX_KEY_VALUE, key));
        }

        private string GetValueFromRegex(string input, string regexKey)
        {
            Regex regex = new Regex(regexKey);
            Match match = regex.Match(input);
            return match.Value;
        }

        async private void IR_Clicked(object sender, EventArgs e)
        {
            var wc = new HttpClient();
           
            //var wc = new WebClient { Encoding = System.Text.Encoding.GetEncoding("ISO-8859-1") };
            //WebClient wc = new WebClient();
            HttpResponseMessage response = wc.GetAsync(new Uri("http://bit.ly/alex3d-lista")).Result;
            
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                string resultado = Encoding.GetEncoding("ISO-8859-1").GetString(byteArray, 0, byteArray.Length);



            string conteudo = resultado; //await wc.GetStringAsync(new Uri("http://bit.ly/alex3d-lista"));

            //string conteudo = wc.DownloadString("http://bit.ly/alex3d-lista");


            //m3uParser1 = new M3uParser(conteudo);
            //txtParsedContent.Document.SetText(Windows.UI.Text.TextSetOptions.None,string.Join(Environment.NewLine, m3uParser1.ParsedLines.Select(n => n.URL.ToList)));

            var m3uLines = new List<M3uLine>();
            string line;
            StringReader Strm3u = new StringReader(conteudo);
            List<Infos> Informacoes = new List<Infos>();
            while ((line = Strm3u.ReadLine()) != null)
            {
                if (line.StartsWith(LINE_PREFIX))
                {
                    var parsed = ParseLine(line);
                    logocanal = parsed.TvgLogo.ToString();
                    nomecanal = parsed.Name.ToString();

                }
                else if (line.StartsWith("http"))
                {
                    //Canais.Items.Add(line.ToString());
                    Informacoes.Add(new Infos() { logo = logocanal, canalnome = nomecanal, linkurl = line.ToString() });
                    logocanal = "";
                    nomecanal = "";
                }
            }
            Canais.ItemsSource = Informacoes;
        }

    private void Canais_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Infos clicado = (Infos)e.SelectedItem;
            Player abrirvideo = new Player();
            
                //abrirvideo.Executar(clicado.linkurl, clicado.canalnome);
                
        }
       
    }
}
