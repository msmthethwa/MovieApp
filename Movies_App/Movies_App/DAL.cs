using System;
using System.Data;
using System.Data.SqlClient;

namespace Movies_App
{
    internal class DAL
    {
        // Connection string to connect to the SQL Server database
        private readonly string _connectionString;

        // Constructor to initialize the DAL class with a connection string
        public DAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to establish and open a connection to the SQL Server
        public SqlConnection Connect()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Method to insert a new movie record into the tblMovie table
        public void Insert(string title, string genre, decimal price)
        {
            string query = "INSERT INTO tblMovies (MovieTitle, MovieGenre, MovieSellingPrice) " +
                           "VALUES (@Title, @Genre, @Price)";

            using (SqlConnection connection = Connect())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@Price", price);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} rows inserted.");
            }
        }

        // Method to execute an update query on the SQL Server
        public void Update(int movieID, string title, string genre, decimal price)
        {
            // Define the SQL query to update the movie details in the tblMovie table
            string query = "UPDATE tblMovies SET MovieTitle = @Title, MovieGenre = @Genre, MovieSellingPrice = @Price WHERE MovieID = @MovieID";

            using (SqlConnection connection = Connect())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@MovieID", movieID);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) updated.");
            }
        } 

        // Method to delete a movie record from the tblMovie table by its ID
        public void Delete(int movieID)
        {
            string query = "DELETE FROM tblMovies WHERE MovieID = @MovieID";

            using (SqlConnection connection = Connect())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MovieID", movieID);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) deleted.");
            }
        }

        public DataTable SearchMovies(string searchTerm)
        {
            DataTable dataTable = new DataTable();

            // Define the SQL query to search for movies by title or genre
            string query = "SELECT MovieID, MovieTitle, MovieGenre, MovieSellingPrice FROM tblMovies " +
                           "WHERE MovieTitle LIKE @SearchTerm OR MovieGenre LIKE @SearchTerm";

            // Use the Connect method to open a connection
            using (SqlConnection connection = Connect())
            {
                // Create a SqlCommand object to execute the query
                SqlCommand command = new SqlCommand(query, connection);

                // Add parameter to prevent SQL injection
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                // Create a SqlDataAdapter to fill the DataTable with the results
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                // Fill the DataTable with data from the database
                dataAdapter.Fill(dataTable);
            }

            return dataTable;
        }
    }
}
