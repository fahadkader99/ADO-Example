using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_Example.Models;
using System.Diagnostics;

namespace ADO_Example.Data
{
    // From here We will connect with the DB & read the data

    /*
     * This data layer class holds :
     * - the DB connection string 
     * - all the stored procedure related function to perform the CRUD operation
     *  System.Data.SqlClient - nuGet package needs to be downloaded
     */

    public class Product_DAL
    {
        // Update connection string based on your local SSMS config
        string conString = ConfigurationManager.ConnectionStrings["adoConnection"].ToString();


        /// <summary>
        /// Get all products
        /// This function is fetching all the records from DB to FrontEnd
        /// </summary>
        /// <returns>All the product from DB as a list</returns>

        public List<Product> GetAllProducts()
        {
            // Empty list of product
            List<Product> productList = new List<Product>();

            // Passing store proc name & the connection
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                adapter.Fill(dtProducts);                                       // getting data from db as data table
                connection.Close();

                // converting the data table into a List 
                foreach (DataRow dr in dtProducts.Rows)
                {
                    // Looping through DataTable row & adding each element/value to the Product list
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = Convert.ToString(dr["Remarks"])
                    });
                      
                }

            }
                return productList;
        }


        /*
         * Insert Products
         */
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();              // for Insert, update, delete - ExecuteNonQuery is used - returns 1 for success & 0 for failed
                connection.Close();

                if(id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        /// <summary>
        /// Get products by ID
        ///This function is fetching all the records from DB to FrontEnd based on ID
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns>Product based on product ID as a list</returns>
        public List<Product> GetProductByID(int ProductID)
        {
            // Empty list of product
            List<Product> productList = new List<Product>();

            // Passing store proc name & the connection
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                adapter.Fill(dtProducts);                                       // getting data from db as data table
                connection.Close();

                // converting the data table into a List 
                foreach (DataRow dr in dtProducts.Rows)
                {
                    // Looping through DataTable row & adding each element/value to the Product list
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = Convert.ToString(dr["Remarks"])
                    });

                }

            }
            return productList;
        }


        /*
        * Update Products based on ID
        */
        public bool UpdateProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();              // for Insert, update, delete - ExecuteNonQuery is used - returns 1 for success & 0 for failed
                connection.Close();

                if (id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        /*
        * Delete Product based on ID
        */
        public string DeleteProduct(int ProductID)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PRODUCTID",ProductID);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }



    }
        


}