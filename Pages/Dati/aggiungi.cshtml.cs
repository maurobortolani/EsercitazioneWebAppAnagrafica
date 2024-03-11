using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static WebApplication1.Pages.Dati.listaDatiModel;
using System.Data.SqlClient;
using System.Globalization;

namespace WebApplication1.Pages.Dati
{
    public class aggiungiModel : PageModel
    {
		public string errorMessage = "";
		public dati dato = new dati();

		public void OnGet()
		{

		}

		public void OnPost()
		{
			dato.Data = Request.Form["data"];
			dato.Valore = Request.Form["valore"];

			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					DateTime dt = DateTime.ParseExact(dato.Data, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

					string sql = $"INSERT INTO Dati " +
						$"(Data, Valore) VALUES " +
						$"('{dt.ToString("MM/dd/yyyy HH:mm")}', '{dato.Valore.Trim()}')";

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

			Response.Redirect("listaDati");
		}
	}
}
