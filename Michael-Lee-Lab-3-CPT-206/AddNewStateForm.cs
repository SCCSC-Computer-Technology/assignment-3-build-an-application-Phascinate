using System;
using System.Linq;
using System.Windows.Forms;
using StatesDatabaseClassLibrary;

namespace Michael_Lee_Lab_3_CPT_206
{
    public partial class AddNewStateForm : Form
    {
        private AppDbContext appDbContext; // Database context
        private MainForm mainForm;

        public AddNewStateForm(MainForm mainForm, AppDbContext context)
        {
            InitializeComponent();
            appDbContext = context;
            this.mainForm = mainForm;
        }

        private void AddNewStateForm_Load(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the form without saving
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate user input
                if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(populationTextBox.Text) ||
                    string.IsNullOrWhiteSpace(flagDescriptionTextBox.Text) ||
                    string.IsNullOrWhiteSpace(flowerTextBox.Text) ||
                    string.IsNullOrWhiteSpace(birdTextBox.Text) ||
                    string.IsNullOrWhiteSpace(colorsTextBox.Text) ||
                    string.IsNullOrWhiteSpace(largestCitiesTextBox.Text) ||
                    string.IsNullOrWhiteSpace(capitalTextBox.Text) ||
                    string.IsNullOrWhiteSpace(medianIncomeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(computerJobsPercentTextBox.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create new state object
                var newState = new State
                {
                    StateName = nameTextBox.Text,
                    Population = int.TryParse(populationTextBox.Text, out int population) ? population : 0,
                    FlagDescription = flagDescriptionTextBox.Text,
                    StateFlower = flowerTextBox.Text,
                    StateBird = birdTextBox.Text,
                    Colors = colorsTextBox.Text,
                    LargestCities = largestCitiesTextBox.Text,
                    StateCapital = capitalTextBox.Text,
                    MedianIncome = decimal.TryParse(medianIncomeTextBox.Text, out decimal income) ? income : 0m,
                    ComputerJobsPercent = decimal.TryParse(computerJobsPercentTextBox.Text, out decimal jobsPct) ? jobsPct : 0m
                };

                // Add the new state to the database
                appDbContext.CreateState(newState);

                MessageBox.Show("State added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mainForm.LoadStatesIntoDataGridView(); // Refresh DataGridView
                mainForm.LoadStatesIntoComboBox();

                this.Close(); // Close the form after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding state: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.InnerException.ToString());
            }
        }
    }
}
