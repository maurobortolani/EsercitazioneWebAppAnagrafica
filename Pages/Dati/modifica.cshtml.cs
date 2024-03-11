using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;
using static WebApplication1.Pages.Dati.listaDatiModel;

namespace WebApplication1.Pages.Dati
{
    public class modificaModel : PageModel
    {
		public string errorMessage = "";
		public dati dato = new dati();

		/*
			* questa viene eseguita al caricamento della pagina
			*/
		public void OnGet()
		{
			string id = "" + Request.Query["id"];

			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					string sql = $"SELECT * FROM Dati WHERE id like '{id}'";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								dato.id = reader.GetInt32(0);
								dato.Data = reader.GetDateTime(1).ToString("yyyy-MM-ddTHH:mm");
								dato.Valore = reader.GetInt32(2).ToString();
							}
						}
					}
					connection.Close();
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}

		/*
			* questa viene eseguita alla pressione del pulsante "salva"
			*/
		public void OnPost()
		{
			dato.id = Convert.ToInt32(Request.Form["id"]);
			dato.Data = Request.Form["data"];
			dato.Valore = Request.Form["valore"];

			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					DateTime dt = DateTime.ParseExact(dato.Data, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

					string sql = $"UPDATE Dati " +
						$"SET Data='{dt.ToString("MM/dd/yyyy HH:mm")}', " +
						$"Valore='{dato.Valore.Trim()}' " +						
						$"WHERE Id like '{dato.id}'";

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
