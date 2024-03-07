using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static WebApplication1.Pages.PagineTest.testModel;
using System.Data.SqlClient;
using System.Globalization;

namespace WebApplication1.Pages.PagineTest
{
    public class aggiungiModel : PageModel
    {
        public string errorMessage = "";
        public anagrafica dato = new anagrafica();

        public void OnGet()
        {

        }

        public void OnPost()
        {
            dato.Nome = Request.Form["nome"];
            dato.Cognome = Request.Form["cognome"];
            dato.DataNascita = Request.Form["datanascita"];
            dato.telefono = Request.Form["telefono"];

            try
            {
                using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
                {
                    connection.Open();
                    DateTime dt = DateTime.ParseExact(dato.DataNascita, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string sql = $"INSERT INTO Anagrafica " +
                        $"(Nome, Cognome, Telefono, DataNascita) VALUES " +
                        $"('{dato.Nome.Trim()}', '{dato.Cognome.Trim()}', " +
                        $"'{dato.telefono.Trim()}', '{dt.ToString("MM/dd/yyyy")}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            Response.Redirect("test");
        }
    }
}
