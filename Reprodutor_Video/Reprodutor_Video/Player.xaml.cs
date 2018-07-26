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
        public string link;
        public string canal;
        
        public Player ()
		{
			InitializeComponent ();

        }

        
       

        public void Executar (string linksource,string nomecanal)
        {
           
            link = linksource;
            canal = nomecanal;
            DisplayAlert(canal, link, "OK");
           
            //VPlayer.Source = linksource;
            //Assistindo.Text = nomecanal;

        }
        
    }
    
}