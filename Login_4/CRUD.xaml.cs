using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Login_4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CRUD : ContentPage
    {
        public class MyTableList
        {
            public int Id_usuario { get; set; }
            public string Nombre_user { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
        }
        SqlConnection sqlConnection; // Declara un objeto de conexión

        public CRUD()
        {
            InitializeComponent();

            // Inicializa los parámetros de conexión
            string srvrdbname = "LOGIN_2023";  //Cambiar esto valores jhoan
            string srvrname = "xxx.xxx.xxx.xxx";
            string srvrusername = "JUANCITO";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn); // Crea una nueva instancia de conexión

        }
        // Método para establecer una conexión a la base de datos
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open(); // Abre la conexión a la base de datos
                await App.Current.MainPage.DisplayAlert("Alerta", "Conexión establecida correctamente.✔", "Ok");
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

        // Método para insertar datos en la base de datos
        private async void Postbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open(); // Abre la conexión

                // Prepara y ejecuta la consulta INSERT con parámetros
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.usuario VALUES(@id_usuario, @nombre_user, @telefono, @email)", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("id_usuario", id_usuario.Text));
                    command.Parameters.Add(new SqlParameter("nombre_user", nombre_user.Text));
                    command.Parameters.Add(new SqlParameter("telefono", telefono.Text));
                    command.Parameters.Add(new SqlParameter("email", email.Text));
                    command.ExecuteNonQuery(); // Ejecuta la consulta
                }

                sqlConnection.Close(); // Cierra la conexión
                await App.Current.MainPage.DisplayAlert("Alerta", "¡Felicidades, los datos se han insertado correctamente!", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", ex.Message, "Ok");
                throw;
            }
        }

        // Método para actualizar datos en la base de datos
        private async void Updatebutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open(); // Abre la conexión

                // Obtiene los valores para la actualización
                int IdtoBeUpdated = Convert.ToInt32(id_usuario.Text);
                string NombreTobeUpdated = nombre_user.Text;
                string telefonoTobeUpdated = telefono.Text;
                string EmailTobeUpdated = email.Text;

                // Prepara y ejecuta la consulta UPDATE
                string qerystr = $"UPDATE dbo.usuario SET id_usuario='{IdtoBeUpdated}', nombre_user='{NombreTobeUpdated}', telefono='{telefonoTobeUpdated}', email='{EmailTobeUpdated}' WHERE id_usuario='{IdtoBeUpdated}'";
                using (SqlCommand command = new SqlCommand(qerystr, sqlConnection))
                {
                    command.ExecuteNonQuery(); // Ejecuta la consulta
                }

                sqlConnection.Close(); // Cierra la conexión
                await App.Current.MainPage.DisplayAlert("Alerta", "¡Felicidades, el registro se ha actualizado correctamente!", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", ex.Message, "Ok");
                throw;
            }
        }

        // Método para eliminar datos de la base de datos
        private async void Deletebutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open(); // Abre la conexión

                int idtodelete = Convert.ToInt32(id_usuario.Text);

                // Prepara y ejecuta la consulta DELETE
                using (SqlCommand command = new SqlCommand($"Delete FROM dbo.usuario WHERE id_usuario = {idtodelete}", sqlConnection))
                {
                    command.ExecuteNonQuery(); // Ejecuta la consulta
                }

                sqlConnection.Close(); // Cierra la conexión
                await App.Current.MainPage.DisplayAlert("Alerta", "¡Felicidades, el registro se ha eliminado correctamente!", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", ex.Message, "Ok");
                throw;
            }
        }
        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            SqlCommand comando = new SqlCommand("Select * from dbo.usuario WHERE id_usuario=@id_usuario", sqlConnection);
            comando.Parameters.AddWithValue("@id_usuario", id_usuario.Text);
            sqlConnection.Open();

            SqlDataReader registro = comando.ExecuteReader();

            if (registro.Read()) // Si se encuentra un registro
            {
                // Llena los campos de texto con los valores obtenidos
                nombre_user.Text = registro["nombre_user"].ToString();
                telefono.Text = registro["telefono"].ToString();
                email.Text = registro["email"].ToString();
                sqlConnection.Close();
            }

            sqlConnection.Close();
        }

    }

}