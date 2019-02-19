

namespace JHipsterNetSampleApplication.Domain {
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJHipsterDataContext {
        //
        // Résumé :
        //     Saves all changes made in this context to the database.
        //
        // Paramètres :
        //   acceptAllChangesOnSuccess:
        //     Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges
        //     is called after the changes have been sent successfully to the database.
        //
        // Retourne :
        //     The number of state entries written to the database.
        //
        // Exceptions :
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Notes :
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        int SaveChanges(bool acceptAllChangesOnSuccess);
        //
        // Résumé :
        //     Saves all changes made in this context to the database.
        //
        // Retourne :
        //     The number of state entries written to the database.
        //
        // Exceptions :
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Notes :
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        int SaveChanges();
        //
        // Résumé :
        //     Asynchronously saves all changes made in this context to the database.
        //
        // Paramètres :
        //   acceptAllChangesOnSuccess:
        //     Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges
        //     is called after the changes have been sent successfully to the database.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Retourne :
        //     A task that represents the asynchronous save operation. The task result contains
        //     the number of state entries written to the database.
        //
        // Exceptions :
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Notes :
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        //
        // Résumé :
        //     Asynchronously saves all changes made in this context to the database.
        //
        // Paramètres :
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Retourne :
        //     A task that represents the asynchronous save operation. The task result contains
        //     the number of state entries written to the database.
        //
        // Exceptions :
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Notes :
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
