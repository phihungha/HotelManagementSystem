using Microsoft.EntityFrameworkCore;

namespace HotelManagementSoftware.Data
{
    public class Database : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<ReservationCancelFeePercent> ReservationCancelFeePercents => Set<ReservationCancelFeePercent>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<HousekeepingRequest> HousekeepingRequests => Set<HousekeepingRequest>();
        public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();
        public DbSet<MaintenanceItem> MaintenanceItems => Set<MaintenanceItem>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Configuration> Configurations => Set<Configuration>();

        public Database()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Database=HotelDB;Trusted_Connection=True;");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<Gender>()
                .HaveConversion<string>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraints & indexes
            modelBuilder.Entity<Employee>()
                .HasIndex(i => new { i.Cmnd })
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(i => new { i.UserName })
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(i => new { i.PhoneNumber })
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(i => new { i.IdNumber })
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(i => new { i.PhoneNumber })
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(i => new { i.CardNumber })
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(i => new { i.RoomNumber })
                .IsUnique();

            modelBuilder.Entity<RoomType>()
                .HasIndex(i => new { i.Name })
                .IsUnique();

            modelBuilder.Entity<ReservationCancelFeePercent>()
                .HasIndex(i => new { i.DayNumberBeforeArrival })
                .IsUnique();

            // Enum to string conversion
            modelBuilder.Entity<Reservation>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Room>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<MaintenanceRequest>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<HousekeepingRequest>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Customer>()
                .Property(i => i.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Customer>()
                .Property(i => i.IdNumberType)
                .HasConversion<string>();

            modelBuilder.Entity<Employee>()
                .Property(i => i.EmployeeType)
                .HasConversion<string>();

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(i => i.OpenEmployee)
                .WithMany(i => i.OpenedMaintenanceRequests)
                .HasForeignKey(i => i.OpenEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(i => i.CloseEmployee)
                .WithMany(i => i.ClosedMaintenanceRequests)
                .HasForeignKey(i => i.CloseEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HousekeepingRequest>()
                .HasOne(i => i.OpenEmployee)
                .WithMany(i => i.OpenedHousekeepingRequests)
                .HasForeignKey(i => i.OpenEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HousekeepingRequest>()
                .HasOne(i => i.CloseEmployee)
                .WithMany(i => i.ClosedHousekeepingRequests)
                .HasForeignKey(i => i.CloseEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
