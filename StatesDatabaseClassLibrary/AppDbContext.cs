using Microsoft.EntityFrameworkCore;
using StatesDatabaseClassLibrary;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace StatesDatabaseClassLibrary // Change this to match your Class Library namespace
{
    public class AppDbContext : DbContext
    {
        public DbSet<State> States { get; set; } // Replace 'State' with your actual table model

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Set the MDF file path dynamically
                string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "StatesDB2.mdf");

                // Ensure the file exists
                if (!File.Exists(databasePath))
                {
                    throw new FileNotFoundException($"Database file not found at: {databasePath}");
                }

                // Connection string for LocalDB
                string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                                             AttachDbFilename={databasePath};
                                             Integrated Security=True;
                                             Connect Timeout=30;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public void CreateState(State state)
        {
            States.Add(state);
            SaveChanges();
        }

        public void DeleteState(State state)
        {
            if (States.Contains(state))
            {
                States.Remove(state);
                SaveChanges();
            }
        }

        public State GetStateByName(string stateName)
        {
            return States.FirstOrDefault(s => s.StateName == stateName);
        }

        public State GetStateByID(int stateID)
        {
            return States.FirstOrDefault(s => s.StateID == stateID);
        }

    }
}