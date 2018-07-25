using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprodutor_Video
{
    public class M3uLine
    {

        public string TvgID { get; set; }
        public string TvgName { get; set; }
        public string TvgLogo { get; set; }
        public string GroupTitle { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public Guid Id { get; internal set; }

        public override string ToString()
        {
            return string.Format(@"#EXTINF:-1 tvg-id=""{0}"" tvg-name=""{1}"" tvg-logo=""{2}"" group-title=""{3}"", {4}{5}{6}",
                new string[] { TvgID, TvgName, TvgLogo, GroupTitle, Name, Environment.NewLine, URL });
        }

    }
}
