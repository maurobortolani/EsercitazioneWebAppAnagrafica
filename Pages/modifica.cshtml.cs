using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static WebApplication1.Pages.testModel;
using System.Data.SqlClient;

namespace WebApplication1.Pages
{
    public class modificaModel : PageModel
    {
        public string errorMessage = "";
		String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;" +
				"AttachDbFilename=C:\\Users\\bortolanim\\Desktop\\WebApplication1\\db.mdf;" +
				"Integrated Security=True;" +
				"Connect Timeout=30";
		public anagrafica dato = new anagrafica();

		/*
         * questa viene eseguita al caricamento della pagina
         */
		public void OnGet() 
        {
            String id = "" + Request.Query["id"];

            try
            {
				using (SqlConnection connection = new SqlConnection(connectionString))
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
			dato.Nome = Request.Query["nome"];


			Response.Redirect("test");
        }
    }
}
