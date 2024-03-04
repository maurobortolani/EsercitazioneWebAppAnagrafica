using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static WebApplication1.Pages.testModel;
using System.Data.SqlClient;
using System.Globalization;

namespace WebApplication1.Pages
{
    public class aggiungiModel : PageModel
    {
		public string errorMessage = "";
		String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;" +
				"AttachDbFilename=C:\\Users\\bortolanim\\Desktop\\WebApplication1\\db.mdf;" +
				"Integrated Security=True;" +
				"Connect Timeout=30";
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
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					DateTime dt = DateTime.ParseExact(dato.DataNascita, "dd/MM/yyyy", CultureInfo.InvariantCulture);
	
					String sql = $"INSERT INTO Anagrafica " +
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
