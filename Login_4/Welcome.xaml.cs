using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Login_4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Welcome : ContentPage
    {
        public Welcome()
        {
            InitializeComponent();
        }

        // Este método se ejecuta cuando se hace clic en un botón con el nombre "btncrud"
        private async void btncrud_Clicked(object sender, EventArgs e)
        {
            // Navega a una nueva página llamada "CRUD"
            await Navigation.PushAsync(new CRUD());
        }

        // Este método se ejecuta cuando se hace clic en un botón con el nombre "Salir"
        private void Salir_Clicked(object sender, EventArgs e)
        {
            // Salimos de la aplicación
            // Cerrar la aplicación
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}