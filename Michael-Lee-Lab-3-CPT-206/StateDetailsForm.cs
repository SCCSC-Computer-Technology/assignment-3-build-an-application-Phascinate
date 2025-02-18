using System;
using System.Linq;
using System.Windows.Forms;
using StatesDatabaseClassLibrary;

namespace Michael_Lee_Lab_3_CPT_206
{
    public partial class StateDetailsForm : Form
    {
        private AppDbContext appDbContext;
        private String stateName; // Store the selected state's ID

        public StateDetailsForm(AppDbContext context, String stateName)
        {
            InitializeComponent();
            appDbContext = context;
            this.stateName = stateName; // Starts at 1, not actually 0
        }

        private void StateDetailsForm_Load(object sender, EventArgs e)
        {
            LoadStateDetails();
        }

        private void LoadStateDetails()
        {
            try
            {
                // Fetch the state from the database
                var state = appDbContext.GetStateByName(stateName);

                if (state != null)
                {
                    // Populate textboxes with state details
                    nameTextBox.Text = state.StateName;
                    populationTextBox.Text = state.Population.ToString();
                    flagDescriptionTextBox.Text = state.FlagDescription;
                    flowerTextBox.Text = state.StateFlower;
                    birdTextBox.Text = state.StateBird;
                    colorsTextBox.Text = state.Colors;
                    largestCitiesTextBox.Text = state.LargestCities;
                    capitalTextBox.Text = state.StateCapital;
                    medianIncomeTextBox.Text = state.MedianIncome.ToString("C"); // Format as currency
                    computerJobsPercentTextBox.Text = (state.ComputerJobsPercent / 100).ToString("P1"); // Format as percentage
                }
                else
                {
                    MessageBox.Show("State not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); // Close the form if no data is found
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading state details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
