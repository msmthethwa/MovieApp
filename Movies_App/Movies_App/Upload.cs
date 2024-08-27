using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Movies_App
{
    public partial class Upload : Form
    {
        public Upload()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            // Load data into DataGridView on form load
            LoadData();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        // Connection string using Windows Authentication
        string connectionString = "Data Source=DESKTOP-A187SQI;Initial Catalog=MovieDB;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            this.Hide();
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            string Title = upTitle.Text;
            string Genre = upGenre.Text;

            // Use TryParse to safely convert the text to decimal
            bool isPriceValid = decimal.TryParse(upPrice.Text, out decimal Price);

            // Check if Title or Genre is empty and if Price is valid
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Genre) || !isPriceValid)
            {
                MessageBox.Show("All movie data is required and Price must be like 123.45");
            }
            else
            {
                DAL dal = new DAL(connectionString);

                // Insert the movie record into the database
                dal.Insert(Title, Genre, Price);

                // Clear fields
                upTitle.Text = "";
                upGenre.Text = "";
                upPrice.Text = "";

                // Refresh the DataGridView
                LoadData();
            }
        }

        // Method to load data from the database and bind to DataGridView
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MovieID, MovieTitle, MovieGenre, MovieSellingPrice FROM tblMovies";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                // Fill the DataTable with data from the database
                dataAdapter.Fill(dataTable);

                // Clear existing rows in the DataGridView
                dataGridView1.Rows.Clear();

                // Add rows to the DataGridView
                foreach (DataRow row in dataTable.Rows)
                {
                    dataGridView1.Rows.Add(row["MovieID"], row["MovieTitle"], row["MovieGenre"], row["MovieSellingPrice"]);
                }
            }
        }

        int index;
        string id = null;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[index];
            id = row.Cells[0].Value.ToString();
            upTitle.Text = row.Cells[1].Value.ToString();
            upGenre.Text = row.Cells[2].Value.ToString();
            upPrice.Text = row.Cells[3].Value.ToString();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            // Check if a movie has been selected
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please select a movie to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse the MovieID from the selected row
            int mID = int.Parse(id);

            // Get the updated details from the input fields
            string updatedTitle = upTitle.Text;
            string updatedGenre = upGenre.Text;
            decimal updatedPrice;

            // Use TryParse to safely convert the price text to a decimal
            if (!decimal.TryParse(upPrice.Text, out updatedPrice))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            // Create an instance of the DAL class
            DAL dal = new DAL(connectionString);

            // Call the Update method to update the selected movie
            dal.Update(mID, updatedTitle, updatedGenre, updatedPrice);

            // Refresh the DataGridView to show the updated movie details
            LoadData();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            // Check if a movie has been selected
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please select a movie to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse the MovieID from the selected row
            int mID = int.Parse(id);

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this movie?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Create an instance of the DAL class
                DAL dal = new DAL(connectionString);

                // Call the Delete method to delete the selected movie
                dal.Delete(mID);

                // Refresh the DataGridView to remove the deleted movie
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Clear fields
            upTitle.Text = "";
            upGenre.Text = "";
            upPrice.Text = "";

            // Refresh the DataGridView to remove the deleted movie
            LoadData();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string search = searchBar.Text.Trim();

            // Check if the search input is empty
            if (string.IsNullOrWhiteSpace(search))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            // Create an instance of the DAL class
            DAL dal = new DAL(connectionString);

            // Search for movies matching the search term
            DataTable results = dal.SearchMovies(search);

            // Check if any results were found
            if (results.Rows.Count > 0)
            {
                // Clear existing rows in the DataGridView
                dataGridView1.Rows.Clear();

                // Add the search results to the DataGridView
                foreach (DataRow row in results.Rows)
                {
                    dataGridView1.Rows.Add(row["MovieID"], row["MovieTitle"], row["MovieGenre"], row["MovieSellingPrice"]);
                }
            }
            else
            {
                MessageBox.Show("No movies found matching your search.");
            }
        }

    }
}
