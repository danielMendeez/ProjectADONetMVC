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

        //Get all Products
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

        //Insert new Product
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

        //Verify if a name Product exists
        public bool VerifyExistNameProduct(string NombreProducto)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_VerifyExistsNameProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Parámetro de entrada
                command.Parameters.AddWithValue("@Nombre", NombreProducto);

                // Parámetro de retorno
                SqlParameter returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();
                int result = (int)returnParameter.Value;
                connection.Close();

                return result == 1; // true si existe, false si no
            }
        }

        //Verify if a Product exists by ID
        public bool VerifyExistIdProduct(int ProductoID)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_VerifyExistsIdProduct", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Parámetro de entrada
                command.Parameters.AddWithValue("@ProductoID", ProductoID);

                // Parámetro de retorno
                SqlParameter returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();
                int result = (int)returnParameter.Value;
                connection.Close();

                return result == 1; // true si existe, false si no
            }
        }

        //Get Product by ID
        public List<Producto> GetProductByID(int ProductID)
        {
            List<Producto> productList = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID", ProductID);

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Producto
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Nombre = dr["nombre"].ToString(),
                        Precio = Convert.ToDecimal(dr["precio"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"])
                    });
                }

            }

            return productList;
        }

        //Update Product
        public bool UpdateProduct(Producto producto)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductoID", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Cantidad", producto.Cantidad);

                connection.Open();
                // use ExecuteNonQuery() for insert, update and delete querys
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete Product
        public string DeleteProduct(int ProductoID)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductoID", ProductoID);
                command.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                // use ExecuteNonQuery() for insert, update and delete querys
                command.ExecuteNonQuery();
                result = command.Parameters["@OutputMessage"].Value.ToString();
                connection.Close();
            }

            return result;
        }
    }
}