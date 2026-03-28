using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> op): base(op) { }

        public DbSet<Users> Users => Set<Users>(); // Read only. Not null.
        public DbSet<Role> Role => Set<Role>(); // Read only. Not null.
        public DbSet<SensorData> SensorData => Set<SensorData>();   // Read only. Not null.
        public DbSet<Alerts> Alerts => Set<Alerts>();   // Read only. Not null.
        public DbSet<Vehicle> Vehicle => Set<Vehicle>();   // Read only. Not null.

        // Entity framework crea el modelo y lo guarda en caché.
        // Nota: Se ejecuta una sola vez, al iniciar el app
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Valor por defecto en la migracion.
            modelBuilder.Entity<Users>(user => {
                user.Property(x => x.RoleId)
                .HasDefaultValueSql("3");

                user.HasOne(u => u.Role)
                .WithMany() // Sin relacion bidireccional.
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // restriccion para evitar borrar un rol con users asociado.
            });

            // Recorre todas las entidades que estan en caché.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Filtrando las que hereden de "EntityBase"
                if (typeof(EntityBase).IsAssignableFrom(entity.ClrType))
                {
                    modelBuilder.Entity(entity.ClrType)
                        .Property(nameof(EntityBase.CreatedAt))
                        .ValueGeneratedOnAdd()                                          // Solo se genera al insertar.
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);    // Despues no lo actualiza mas.
                }
            }
        }

        // Sobreescritura del SaveChangesAsync para setear el createdAt cada vez que se guarde.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetterCreatedAt();
            return await base.SaveChangesAsync(cancellationToken);  // sigue guardando igual que antes.
        }
            
        private void SetterCreatedAt()
        {
            // Solo afecta a las entidades que heredan de EntityBase Devuelve una lista de las propiedades de EntityBase.
            var entries = ChangeTracker.Entries<EntityBase>();

            foreach (var entry in entries) { 
                if (entry.State == EntityState.Added)   // Modo insert.
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)    // Modo update.
                {
                    // Evita actualizar "CreatedAt" al guardar en estado "Modified".
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
