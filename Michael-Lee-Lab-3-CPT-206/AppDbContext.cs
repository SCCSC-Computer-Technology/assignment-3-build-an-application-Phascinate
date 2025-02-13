using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Michael_Lee_Lab_3_CPT_206
{
    public class AppDbContext : DbContext
    {
        public DbSet<State> States { get; set; } // Maps to the States table

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Dynamically load connection string
            string connectionString = ConfigurationManager.ConnectionStrings["Michael_Lee_Lab_3_CPT_206.Properties.Settings.StatesDBConnectionString"].ConnectionString;
            
            // Use SQL
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
