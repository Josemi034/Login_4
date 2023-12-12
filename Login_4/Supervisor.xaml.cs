using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Login_4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Supervisor : ContentPage
    {
        // Variable para controlar el estado del inicio de sesión
        bool isUserLoggedIn = false;

        // Clase que define la estructura de los elementos en la lista
        public class MyTableList
        {
            public int Id_usuario { get; set; }
            public string Nombre_user { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
        }

        // Objeto para manejar la conexión a la base de datos
        SqlConnection sqlConnection;
        public Supervisor()
        {
            InitializeComponent();

            // Configuración de la cadena de conexión a la base de datos
            string srvrdbname = "LOGIN_PMD";
            string srvrname = "D20B5043";
                    string srvrusername = "Jose";
            string srvrpassword = "josemiguelbaezmendez";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            // Inicialización de la conexión a la base de datos
            sqlConnection = new SqlConnection(sqlconn);
        }

        // Método para validar la conexión a la base de datos
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open(); // Abre la conexión
                await App.Current.MainPage.DisplayAlert("Alert", "Conexión Establecida Correctamente!✔", "Ok");
                sqlConnection.Close(); // Cierra la conexión
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        // Método para obtener datos de la base de datos y mostrarlos en la ListView
        private async void Getbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<MyTableList> myTableLists = new List<MyTableList>(); // Lista para almacenar los datos obtenidos
                sqlConnection.Open(); // Abre la conexión

                string queryString = "Select * from dbo.usuario"; // Consulta SQL
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader(); // Ejecuta la consulta

                while (reader.Read()) // Recorre las filas obtenidas
                {
                    // Crea un objeto MyTableList y asigna sus propiedades
                    myTableLists.Add(new MyTableList
                    {
                        Id_usuario = Convert.ToInt32(reader["id_usuario"]),
                        Nombre_user = reader["nombre_user"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Email = reader["email"].ToString(),
                    });
                }

                reader.Close();
                sqlConnection.Close(); // Cierra la conexión

                MyListView.ItemsSource = myTableLists; // Asigna los datos a la ListView
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", ex.Message, "Ok");
                throw;
            }
        }
        // Método para realizar una búsqueda en la base de datos
        private void Buscar_Clicked(object sender, EventArgs e)
        {
            // ... (implementación para buscar en la base de datos)
            if (string.IsNullOrWhiteSpace(id_usuario.Text))
            {
                resultadoBusqueda.Text = "Por favor, introduzca un criterio de búsqueda.";
                resultadoBusqueda.TextColor = Color.Red;
                resultadoBusqueda.HorizontalTextAlignment = TextAlignment.Center;
                resultadoBusqueda.FontAttributes = FontAttributes.Bold;
                resultadoBusqueda.FontSize = 22;
                return;
            }

            string consultaSQL = "Select * from dbo.usuario WHERE 1 = 1";

            if (int.TryParse(id_usuario.Text, out int idUsuarioValue))
            {
                consultaSQL += " AND id_usuario = @valorBusqueda";
            }
            else
            {
                consultaSQL += " AND (nombre_user LIKE @valorBusqueda OR telefono = @valorBusqueda OR email = @valorBusqueda)";
            }

            SqlCommand comando = new SqlCommand(consultaSQL, sqlConnection);

            if (!int.TryParse(id_usuario.Text, out _))
            {
                comando.Parameters.AddWithValue("@valorBusqueda", "%" + id_usuario.Text + "%");
            }
            else
            {
                comando.Parameters.AddWithValue("@valorBusqueda", id_usuario.Text);
            }

            sqlConnection.Open();

            SqlDataReader registro = comando.ExecuteReader();

            if (registro.Read())
            {
                nombre_user.Text = registro["nombre_user"].ToString();
                telefono.Text = registro["telefono"].ToString();
                email.Text = registro["email"].ToString();
                resultadoBusqueda.Text = "Resultado de la búsqueda:";
            }
            else
            {
                resultadoBusqueda.Text = "No se encontraron resultados.";
                resultadoBusqueda.TextColor = Color.Red;
                resultadoBusqueda.HorizontalTextAlignment = TextAlignment.Center;
                resultadoBusqueda.FontAttributes = FontAttributes.Bold;
                resultadoBusqueda.FontSize = 22;
            }

            sqlConnection.Close();

        }
        // Método para limpiar los campos de búsqueda y resultados
        private void LimpiarButton_Clicked(object sender, EventArgs e)
        {
            // ... (implementación para limpiar campos)
            id_usuario.Text = string.Empty;
            nombre_user.Text = string.Empty;
            telefono.Text = string.Empty;
            email.Text = string.Empty;
            resultadoBusqueda.Text = string.Empty;
        }
        // Método para manejar el botón de "Salir"
        private async void Salir_Clicked(object sender, EventArgs e)
        {

            if (!isUserLoggedIn)
            {
                var respuesta = await DisplayAlert("Confirmación", "¿Seguro que deseas cerrar sesión?", "Sí", "No");

                if (respuesta)
                {
                    isUserLoggedIn = false;

                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                    await Navigation.PushAsync(new MainPage());
                }
            }
        }
    }

}

