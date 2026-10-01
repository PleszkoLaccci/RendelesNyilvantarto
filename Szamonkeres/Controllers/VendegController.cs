using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Runtime.Serialization;

namespace Szamonkeres.Controllers
{
    [Route("vendeg")]
    [ApiController]
    public class VendegController
    {
        string ConnectionString = "Server=localhost;database=etterem;User Id=root;password=;";
        [HttpGet("VendegNameEmailById")]
        public object VendegNameEmailById(int id)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT name, email FROM vendeg WHERE id = @id", conn);
            command.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new { Name = reader["name"], Email = reader["email"] };
            }
            else
            {
                return new { message = "Nincs ilyen vendég" };
            }
        }
        [HttpGet("VendegNameAndEverythingByVendegId")]
        public object VendegNameAndEverythingByVendegId(int id)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT vendeg.name, rendeles.dish, rendeles.description " + "FROM vendeg " + "INNER JOIN rendeles ON rendeles.vendegId = vendeg.id " + "WHERE vendeg.id = @id", conn);
            command.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                conn.Close();
                return new { Name = reader["name"], Dish = reader["dish"], Description = reader["description"] };
            }
            else
            {
                conn.Close();
                return new { message = "Nincs ilyen vendég" };
            }
        }

        [HttpGet("OsszesRendelesSzama")]
        public object OsszesRendelesSzama()
        { 

            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM RENDELES", conn);
            int RendelesekSzama = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            return new { RendelesekSzama = RendelesekSzama};
        }

        [HttpGet("AdottVendegRendelesSzama")]
        public object AdottVendegRendelesSzama(int id)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM rendeles WHERE vendegId = @id", conn);
            command.Parameters.AddWithValue("@id", id);
            int RendelesekSzama = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return new { RendelesekSzama = RendelesekSzama };
        }
    }
}
