using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.IO.Pipelines;
using Szamonkeres.Models;
using Szamonkeres.Models.DTOS;

namespace Szamonkeres.Controllers
{
    [Route("rendeles")]
    [ApiController]
    public class RendelesController
    {
        string ConnectionString = "Server=localhost;database=etterem;User Id=root;password=;";

        [HttpGet("OsszesRendeles")]
        public object OsszesRendeles()
        {
            List<Rendeles> osszes = new List<Rendeles>();
        
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM rendeles", conn);
            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                conn.Close();
                return new { message = "Nem tudjuk lekérdezni a rendeléseket" };
            }

            while(reader.Read())
            {
                Rendeles rendeles = new Rendeles();
                rendeles.Dish = reader.GetString("Dish");
                rendeles.Description = reader.GetString("Description");
                rendeles.OrderTime = reader.GetDateTime("OrderTime");
                rendeles.UpdateTime = reader.GetDateTime("UpdateTime");
                osszes.Add(rendeles);
            }
            conn.Close();
            return osszes;
        }

        [HttpGet("RendelesById")]
        public object RendelesById(int id)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM rendeles WHERE id = @id", conn);
            command.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                conn.Close();
                return new { message = "Nincs ilyen rendelés" };
            }

            var rendeles = new Rendeles
            {
                Dish = reader.GetString("Dish"),
                Description = reader.GetString("Description"),
                OrderTime = reader.GetDateTime("OrderTime"),
                UpdateTime = reader.GetDateTime("UpdateTime")   
            };
            conn.Close();
            return rendeles;
        }

        [HttpPost("UjRendeles")]
        public object UjRendeles(AddNewRendelesDTO addnewrendelesDTO)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("INSERT INTO rendeles (Dish, Description, OrderTime, UpdateTime, vendegId) VALUES (@Dish, @Description, @OrderTime, @UpdateTime, @VendegId)", conn);
            command.Parameters.AddWithValue("@Dish", addnewrendelesDTO.Dish);
            command.Parameters.AddWithValue("@Description", addnewrendelesDTO.Description);
            command.Parameters.AddWithValue("@OrderTime", DateTime.Now);
            command.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            command.Parameters.AddWithValue("@VendegId", addnewrendelesDTO.VendegId);
            command.ExecuteNonQuery();

            conn.Close();
            return new { message = "Rendelés hozzáadva", addnewrendelesDTO = addnewrendelesDTO };
        }

        [HttpPut("RendelesUpdate")]
        public object RendelesUpdate([FromQuery] int id, [FromBody] UpdateRendelesDTO updateRendelesDTO)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("UPDATE rendeles SET Dish = @Dish, Description = @Description, UpdateTime = @UpdateTime WHERE id = @id", conn);
            command.Parameters.AddWithValue("@Dish", updateRendelesDTO.Dish);
            command.Parameters.AddWithValue("@Description", updateRendelesDTO.Description);
            command.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            conn.Close();
            return new { message = "Rendelés frissítve", updateRendelesDTO = updateRendelesDTO };
        }
        [HttpDelete("RendelesDelete")]
        public object RendelesDelete(int id)
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("DELETE FROM rendeles WHERE id = @id", conn);
            command.Parameters.AddWithValue("@id", id);
            int sorok = command.ExecuteNonQuery();
            conn.Close();
            if (sorok > 0)
            {
                return new { message = "Rendelés törölve" };
            }
            else
            {
                return new { message = "Nincs ilyen rendelés" };
            }   
        }
                    
    }
}
