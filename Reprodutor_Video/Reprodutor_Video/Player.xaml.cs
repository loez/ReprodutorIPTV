using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rox;
namespace Reprodutor_Video
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Player : ContentPage
	{
        public string link { get; set; }
        public string canal { get; set; }

        public Player ()
		{
			InitializeComponent ();
            VPlayer.Source = this.link;        }

        
       

        public void Executar (string linksource,string nomecanal)
        {
           
            this.link = linksource;
            canal = nomecanal;
            DisplayAlert(canal, link, "OK");
           
            //VPlayer.Source = linksource;
            //Assistindo.Text = nomecanal;

        }
        
    }
    
}