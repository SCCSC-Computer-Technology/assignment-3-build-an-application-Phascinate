/*
 * Michael Lee
 * CPT-206
 * Lab 3
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.AxHost;
using StatesDatabaseClassLibrary;

namespace Michael_Lee_Lab_3_CPT_206
{
    public partial class MainForm : Form
    {

        private AppDbContext appDbContext = new AppDbContext(); // Database context

        public MainForm()
        {
            InitializeComponent();
           
            bindingNavigator.AddNewItem = null;  // Hack to prevent the BindingNavigator from trying to create a new row instead of using the AddNewCityForm
            bindingNavigator.DeleteItem = null; // Stops automatic deletion
            bindingNavigatorDeleteItem.Enabled = true; // Re-enables delete button

            // The event for when a user edits a column
            dataGridView.CellEndEdit += dataGridView_CellEndEdit;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.statesTableAdapter.Fill(this.statesDBDataSet.States);
            
            // Load data
            LoadStatesIntoComboBox();
            statesComboBox.SelectedIndex = -1; // Clear selection

        }

        public void LoadStatesIntoComboBox()
        {
            try
            {

                var states = appDbContext.States
                                     .OrderBy(s => s.StateName) // Sort alphabetically
                                     .Select(s => new { s.StateID, s.StateName }) // Fetch only needed columns
                                     .ToList(); // Convert to list for ease of use

                statesComboBox.DataSource = states; // Bind data
                statesComboBox.DisplayMember = "StateName"; // Show state names
                statesComboBox.ValueMember = "StateID"; // Store ID (optional)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading states: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadStatesIntoDataGridView()
        {
            try
            {

                var states = appDbContext.States
                                     .OrderBy(state => state.StateName)
                                     .ToList();

                if (states.Count == 0)
                {
                    MessageBox.Show("No states found in the database.", "Database Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridView.DataSource = null; // Reset DataGridView
                dataGridView.DataSource = states;
                dataGridView.AutoResizeColumns(); // Ensure columns resize properly
                dataGridView.Refresh(); // Force a UI update

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading DataGridView: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            AddNewStateForm addNewStateForm = new AddNewStateForm(this, appDbContext);
            addNewStateForm.Show();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get the selected row index
                int rowIndex = dataGridView.SelectedCells[0].RowIndex;

                // Get the correct column name for StateID
                int stateID = Convert.ToInt32(dataGridView.Rows[rowIndex].Cells["dataGridViewTextBoxColumn1"].Value);

                // Find the state in the database
                var stateToDelete = appDbContext.States.FirstOrDefault(s => s.StateID == stateID);

                if (stateToDelete != null)
                {
                    // Confirm deletion
                    DialogResult confirm = MessageBox.Show($"Are you sure you want to delete {stateToDelete.StateName}?",
                                                           "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        // Remove from database
                        appDbContext.DeleteState(stateToDelete);

                        // Refresh DataGridView
                        LoadStatesIntoDataGridView();

                        MessageBox.Show("State deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Deletion failed. State not found in database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void statesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var selectedState = statesComboBox.SelectedItem;

            if (selectedState != null)
            {
                string stateName = statesComboBox.GetItemText(selectedState); // Gets the display text

                // Create the State Details form
                StateDetailsForm stateDetailsForm = new StateDetailsForm(appDbContext, stateName);
                stateDetailsForm.Show();
            }
            else
            {
                MessageBox.Show("Failed to get selected state!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count - 1) // Ignore new row
            {
                try
                {

                    // Get all values from DataGridView
                    int stateID = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value);
                    string stateName = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                    int population = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn3"].Value);
                    string flagDescription = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                    string stateFlower = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    string stateBird = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn6"].Value.ToString();
                    string colors = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn7"].Value.ToString();
                    string largestCities = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn8"].Value.ToString();
                    string stateCapital = dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn9"].Value.ToString();
                    decimal medianIncome = Convert.ToDecimal(dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn10"].Value);
                    decimal computerJobsPercent = Convert.ToDecimal(dataGridView.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn11"].Value);

                    // Find the existing state in the database
                    var stateToUpdate = appDbContext.States.FirstOrDefault(s => s.StateID == stateID);

                    if (stateToUpdate != null)
                    {
                        // Update the record's values
                        stateToUpdate.StateName = stateName;
                        stateToUpdate.Population = population;
                        stateToUpdate.FlagDescription = flagDescription;
                        stateToUpdate.StateFlower = stateFlower;
                        stateToUpdate.StateBird = stateBird;
                        stateToUpdate.Colors = colors;
                        stateToUpdate.LargestCities = largestCities;
                        stateToUpdate.StateCapital = stateCapital;
                        stateToUpdate.MedianIncome = medianIncome;
                        stateToUpdate.ComputerJobsPercent = computerJobsPercent;

                        // Save changes to the database
                        appDbContext.SaveChanges();

                    }
                    else
                    {
                        MessageBox.Show("Update failed. State not found in database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
