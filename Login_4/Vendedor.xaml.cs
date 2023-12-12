using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Login_4
{
    // Definición de la clase "Articulo"
    public class Articulo
    {
        public int IDArticulo { get; set; }     // Propiedad para el ID del artículo
        public string Nombre { get; set; }      // Propiedad para el nombre del artículo
        public decimal Precio { get; set; }     // Propiedad para el precio del artículo
    }

    // Definición de la clase "Venta"
    public class Venta
    {
        public int IDArticulo { get; set; }             // Propiedad para el ID del artículo
        public string NombreArticulo { get; set; }      // Propiedad para el nombre del artículo
        public int Cantidad { get; set; }               // Propiedad para la cantidad de artículos vendidos
        public decimal Precio { get; set; }             // Propiedad para el precio unitario del artículo
        public decimal Monto => Cantidad * Precio;      // Propiedad calculada para el monto total de la venta
    }
    [XamlCompilation(XamlCompilationOptions.Compile)] // Atributo que indica la compilación de XAML
    public partial class Vendedor : ContentPage
    {
        private ObservableCollection<Articulo> articulos; // Colección para almacenar los artículos
        private ObservableCollection<Venta> ventas;       // Colección para almacenar las ventas
        SqlConnection sqlConnection;                      // Objeto para manejar la conexión a la base de datos

        public Vendedor()
        {
            InitializeComponent(); // Inicializa la interfaz de usuario

            // Configuración de la cadena de conexión a la base de datos
            string srvrdbname = "LOGIN_PMD";
            string srvrname = "D20B5043";
            string srvrusername = "Jose";
            string srvrpassword = "josemiguelbaezmendez";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn); // Inicializa la conexión a la base de datos

            LoadArticulos(); // Cargar los artículos desde la base de datos

            LoadVentas(); // Cargar las ventas en la inicialización
            VentasListView.ItemsSource = ventas; // Cargar las ventas en el ListView
        }

        // Método para cargar los artículos desde la base de datos y configurar el Picker de artículos
        private void LoadArticulos()
        {
            articulos = new ObservableCollection<Articulo>(); // Inicializa la colección de artículos

            try
            {
                sqlConnection.Open(); // Abre la conexión a la base de datos
                string queryString = "SELECT IDArticulo, Nombre, Precio FROM Articulos"; // Consulta SQL para obtener los artículos
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) // Recorre los resultados de la consulta
                {
                    // Crea objetos Articulo y agrega a la colección
                    articulos.Add(new Articulo
                    {
                        IDArticulo = Convert.ToInt32(reader["IDArticulo"]),
                        Nombre = reader["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"])
                    });
                }

                reader.Close(); // Cierra el lector
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Muestra cualquier error en la consola
                throw;
            }
            finally
            {
                sqlConnection.Close(); // Cierra la conexión a la base de datos en cualquier caso
            }

            ArticuloPicker.ItemsSource = articulos; // Asigna los artículos al Picker para su selección
        }
        // Método que se ejecuta cuando se selecciona un artículo en el Picker
        private void ArticuloPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedArticulo = (Articulo)ArticuloPicker.SelectedItem; // Obtiene el artículo seleccionado del Picker

            if (selectedArticulo != null)
            {
                PrecioLabel.Text = selectedArticulo.Precio.ToString("C"); // Muestra el precio del artículo seleccionado en formato monetario
            }
            else
            {
                PrecioLabel.Text = string.Empty; // Si no se selecciona ningún artículo, se muestra un campo vacío
            }
        }
        // Método que se ejecuta cuando se hace clic en el botón de "Agregar Venta"
        private async void AgregarVenta_Clicked(object sender, EventArgs e)
        {
            if (ArticuloPicker.SelectedItem == null || string.IsNullOrWhiteSpace(CantidadEntry.Text))
                return; // Si no se selecciona un artículo o no se ingresa una cantidad, no hace nada

            var selectedArticulo = (Articulo)ArticuloPicker.SelectedItem; // Obtiene el artículo seleccionado
            int cantidad = Convert.ToInt32(CantidadEntry.Text); // Convierte la cantidad ingresada a un valor entero

            // Crea un nuevo objeto Venta con los detalles de la venta
            var nuevaVenta = new Venta
            {
                IDArticulo = selectedArticulo.IDArticulo,
                NombreArticulo = selectedArticulo.Nombre,
                Cantidad = cantidad,
                Precio = selectedArticulo.Precio
            };

            try
            {
                sqlConnection.Open(); // Abre la conexión a la base de datos

                // Consulta SQL para insertar la nueva venta en la tabla Ventas
                string insertQuery = "INSERT INTO Ventas (IDArticulo, Cantidad, Precio) VALUES (@IDArticulo, @Cantidad, @Precio)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@IDArticulo", nuevaVenta.IDArticulo);
                insertCommand.Parameters.AddWithValue("@Cantidad", nuevaVenta.Cantidad);
                insertCommand.Parameters.AddWithValue("@Precio", nuevaVenta.Precio);
                insertCommand.ExecuteNonQuery(); // Ejecuta la consulta para insertar la venta

                sqlConnection.Close(); // Cierra la conexión a la base de datos
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Muestra cualquier error en la consola
                throw;
            }

            LoadVentas(); // Vuelve a cargar las ventas desde la base de datos
            ResetVentaInputs(); // Limpia los campos de entrada de la venta

            await DisplayAlert("Venta Agregada", "La venta ha sido agregada exitosamente.", "OK"); // Muestra una alerta de éxito
        }
        // Método para cargar las ventas desde la base de datos y actualizar la lista y el total de ventas
        private void LoadVentas()
        {
            ventas = new ObservableCollection<Venta>(); // Inicializa la colección de ventas

            try
            {
                sqlConnection.Open(); // Abre la conexión a la base de datos
                string queryString = "SELECT IDArticulo, Cantidad, Precio FROM Ventas"; // Consulta SQL para obtener las ventas
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                decimal totalVentas = 0; // Variable para almacenar el total de ventas

                while (reader.Read()) // Recorre los resultados de la consulta
                {
                    int idArticulo = Convert.ToInt32(reader["IDArticulo"]);
                    Articulo articulo = articulos.FirstOrDefault(a => a.IDArticulo == idArticulo); // Busca el artículo correspondiente

                    // Agrega una nueva venta a la colección
                    ventas.Add(new Venta
                    {
                        IDArticulo = idArticulo,
                        NombreArticulo = articulo != null ? articulo.Nombre : "Artículo Desconocido",
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        Precio = Convert.ToDecimal(reader["Precio"])
                    });

                    // Suma el monto de la venta al total de ventas.
                    totalVentas += ventas.Last().Monto;
                }

                // Configura el Label para mostrar el total de ventas
                TotalVentasLabel.Text = $"Total de Ventas: {totalVentas.ToString("C")}";
                TotalVentasLabel.FontSize = 18;
                TotalVentasLabel.FontAttributes = FontAttributes.Bold;
                TotalVentasLabel.TextColor = Color.Green;
                TotalVentasLabel.HorizontalTextAlignment = TextAlignment.Center;
                TotalVentasLabel.VerticalTextAlignment = TextAlignment.Center;

                reader.Close(); // Cierra el lector
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Muestra cualquier error en la consola
                throw;
            }
            finally
            {
                sqlConnection.Close(); // Cierra la conexión a la base de datos en cualquier caso
            }

            VentasListView.ItemsSource = ventas; // Actualiza la ListView con las ventas cargadas
        }
        // Método para restablecer los campos de entrada de venta
        private void ResetVentaInputs()
        {
            ArticuloPicker.SelectedItem = null; // Limpia la selección de artículo en el Picker
            PrecioLabel.Text = string.Empty; // Limpia el contenido del Label de precio
            CantidadEntry.Text = string.Empty; // Limpia el contenido del Entry de cantidad
        }

        // Método que se ejecuta cuando se hace clic en el botón de "Listar Ventas"
        private void ListarVentas_Clicked(object sender, EventArgs e)
        {
            LoadVentas(); // Carga nuevamente las ventas desde la base de datos
            VentasListView.ItemsSource = ventas; // Actualiza la ListView con las ventas cargadas
        }

        // Método que se ejecuta cuando se hace clic en el botón de "Imprimir Ticket"
        private async void ImprimirTicket_Clicked(object sender, EventArgs e)
        {
            string contenidoTicket = "Ticket de Ventas\n\n"; // Encabezado del ticket
            decimal montoTotal = 0; // Variable para almacenar el monto total de ventas

            // Recorre las ventas y agrega sus detalles al contenido del ticket
            foreach (Venta venta in ventas)
            {
                contenidoTicket += $"Artículo: {venta.NombreArticulo}\n" +
                                   $"Cantidad: {venta.Cantidad}\n" +
                                   $"Precio Unitario: {venta.Precio:C}\n" +
                                   $"Monto Total: {venta.Monto:C}\n\n";

                montoTotal += venta.Monto; // Suma el monto de cada venta al monto total
            }

            contenidoTicket += $"Monto Total de Ventas: {montoTotal:C}\n"; // Agrega el monto total al ticket

            // Muestra una ventana emergente con el contenido del ticket
            await DisplayAlert("Listado de Venta Actual", contenidoTicket, "Aceptar");
        }

        // Cierre de la clase y del namespace
    }
}
