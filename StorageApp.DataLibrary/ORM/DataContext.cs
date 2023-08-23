using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StorageApp.DataLibrary.Models;
using System.Diagnostics;

namespace StorageApp.DataLibrary.ORM
{
    /// <summary>
    /// Контекст данных, который ORM EF предоставляет для работы с базой данных
    /// </summary>
    public class DataContext : DbContext
    {
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Pallet> Pallets { get; set; }

        public DataContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        /// <summary>
        /// Сопоставление моделей и таблиц в БД
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Box>().ToTable("Boxes");
            modelBuilder.Entity<Pallet>().ToTable("Pallets");
        }

        /// <summary>
        /// Настройки подключения к БД
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "storage.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
