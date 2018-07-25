using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reprodutor_Video
{
    public class M3uParser
    {
        private const string LINE_PREFIX = "#EXTINF:-1";

        private const string REGEX_KEY_VALUE = @"(?i)(?<=\b{0}="")[^""]*";
        private const string REGEX_CHANNEL_NAME = @"(?<=\"",)[^\n]*";

        private const string TVGID_KEY = "tvg-id";
        private const string TVGNAME_KEY = "tvg-name";
        private const string TVGLOGO_KEY = "tvg-logo";
        private const string GROUPTITLE_KEY = "group-title";

        public List<M3uLine> ParsedLines { get; set; }

        public M3uParser(StreamReader m3uFileStream)
        {
            ParsedLines = ParseFile(m3uFileStream);
        }

        public List<M3uLine> ParseFile(StreamReader m3uFileStream)
        {
            var m3uLines = new List<M3uLine>();
            string line;

            Guid? lastAddedID = null;

            while ((line = m3uFileStream.ReadLine()) != null)
            {
                if (line.StartsWith(LINE_PREFIX))
                {
                    var parsedLine = ParseLine(line);
                    parsedLine.Id = Guid.NewGuid();
                    m3uLines.Add(parsedLine);

                    if(string.IsNullOrWhiteSpace(parsedLine.TvgID))
                    {
                        parsedLine.TvgID = parsedLine.TvgName;
                    }

                    lastAddedID = parsedLine.Id;
                }
                else if(lastAddedID != null)
                {
                    m3uLines.First(n => n.Id == lastAddedID).URL = line;
                    lastAddedID = null;
                }
            }

            return m3uLines;
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

        public void PrefixGroupToName(List<string> prefixes)
        {
            // for each selected which its group-title contains the prefix 
            foreach (var line in ParsedLines.Where(n => prefixes.Any(p => n.GroupTitle.Contains(p))))
            {
                string matchingPrefix = prefixes.First(p => line.GroupTitle.Contains(p));

                if (matchingPrefix == "CA French")
                {
                    matchingPrefix = "QC";
                    line.GroupTitle = "QC";
                }

                string reformatedName = string.Join(": ", matchingPrefix, line.TvgName);

                line.Name = reformatedName;
            }
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
    }
}
