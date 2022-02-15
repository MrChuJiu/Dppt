namespace Easy.Core.Flow.EFTemporal
{
    public class ExampleContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SimpleConfig
            modelBuilder
                .Entity<Employee>()
                .ToTable("Employees", b => b.IsTemporal());
            #endregion

            #region AdvancedConfig
            modelBuilder
                .Entity<Employee>()
                .ToTable(
                    "Employees",
                    b => b.IsTemporal(
                        b =>
                        {
                            b.HasPeriodStart("ValidFrom");
                            b.HasPeriodEnd("ValidTo");
                            b.UseHistoryTable("EmployeeHistoricalData");
                        }));
            #endregion

            modelBuilder
                .Entity<Employee>(
                    b =>
                    {
                        b.Property(e => e.Name).HasMaxLength(100);
                        b.Property(e => e.Position).HasMaxLength(100);
                        b.Property(e => e.Department).HasMaxLength(100);
                        b.Property(e => e.Address).HasMaxLength(1024);
                        b.Property(e => e.AnnualSalary).HasPrecision(10, 2);
                    });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseSqlServer("Server=.; Database=TemporalTables; Trusted_Connection=True; Connection Timeout=600;MultipleActiveResultSets=true;");
    }


    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public decimal AnnualSalary { get; set; }
    }
}
