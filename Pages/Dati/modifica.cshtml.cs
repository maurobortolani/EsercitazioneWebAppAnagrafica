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
								dati dato = new dati();
								dato.id = reader.GetInt32(0);
								dato.Data = reader.GetDateTime(1).ToString("dd/MM/yyyy HH:mm");
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
			dato.Data = Request.Form["nome"];
			dato.Valore = Request.Form["cognome"];

			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					DateTime dt = DateTime.ParseExact(dato.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture);

					string sql = $"UPDATE Anagrafica " +
						$"SET Nome='{dato.Nome.Trim()}', " +
						$"Cognome='{dato.Cognome.Trim()}', " +
						$"Telefono='{dato.telefono.Trim()}', " +
						$"DataNascita='{dt.ToString("MM/dd/yyyy")}' " +
						$"WHERE id like '{dato.id}'";

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
