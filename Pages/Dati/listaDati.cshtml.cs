using Microsoft.AspNetCore.Components.Forms;
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
		
		public class datiString
		{
			public string listaTime = "";
			public string listaDati = "";
		}
		public class dati
		{
			public int id;
			public string Data;
			public string Valore;
		}

		public List<dati> lista = new List<dati>();
		public datiString datiString1 = new datiString();
		public void OnGet()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM Dati ORDER BY Data";

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
				datiString1.listaTime += "[";
				for (int i = 0; i < lista.Count; i++)
				{
					datiString1.listaDati += lista[i].Valore.ToString();
					datiString1.listaTime += "" + lista[i].Data.ToString() + "";
					if (i < lista.Count - 1)
					{
						datiString1.listaDati += ", ";
						datiString1.listaTime += ", ";
					}
				}
				datiString1.listaTime += "]";
				//datiString1.listaTime = "'Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'";
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}
	}
}

