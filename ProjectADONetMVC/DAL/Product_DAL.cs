using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using ProjectADONetMVC.Models;
using System.Data;

namespace ProjectADONetMVC.DAL
{
    public class Product_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();

        //Get all products
        public List<Producto> GetAllProducts()
        {
            List<Producto> productList = new List<Producto>();

            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Producto { 
                        Id = Convert.ToInt32(dr["id"]),
                        Nombre = dr["nombre"].ToString(),
                        Precio = Convert.ToDecimal(dr["precio"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"])
                    });
                }

            }

            return productList;
        }

        public bool InsertProduct(Producto producto)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Cantidad", producto.Cantidad);

                connection.Open();
                // use ExecuteNonQuery() for insert, update and delete querys
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            if(id > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}