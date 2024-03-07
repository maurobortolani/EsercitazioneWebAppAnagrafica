using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static WebApplication1.Pages.testModel;
using System.Data.SqlClient;
using System.Globalization;

namespace WebApplication1.Pages
{
    public class modificaModel : PageModel
    {
        public string errorMessage = "";
		public anagrafica dato = new anagrafica();

		/*
         * questa viene eseguita al caricamento della pagina
         */
		public void OnGet() 
        {
            String id = "" + Request.Query["id"];

            try
            {
				using (SqlConnection connection = new SqlConnection(configurazioni.connectionString))
				{
					connection.Open();
					String sql = $"SELECT * FROM Anagrafica WHERE id like '{id}'";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{								
								dato.id = reader.GetInt32(0);
								dato.Nome = reader.GetString(1);
								dato.Cognome = reader.GetString(2);
								dato.telefono = reader.GetString(3);
								dato.DataNascita = reader.GetDateTime(4).ToString("dd/MM/yyyy");
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

					String sql = $"UPDATE Anagrafica " +
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
