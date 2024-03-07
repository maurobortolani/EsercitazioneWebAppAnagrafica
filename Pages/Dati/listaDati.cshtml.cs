using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication1.Pages.Dati
{
    public class listaDatiModel : PageModel
    {
		public string errorMessage = "";
		public class dati
		{
			public int id;
			public string Data;
			public string Valore;
		}

		public List<dati> lista = new List<dati>();
		public void OnGet()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM Dati";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								dati dato = new dati();
								dato.id = reader.GetInt32(0);
								dato.Data = reader.GetDateTime(1).ToString("dd/MM/yyyy HH:mm");
								dato.Valore = reader.GetInt32(2).ToString();
								lista.Add(dato);
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
	}
}

