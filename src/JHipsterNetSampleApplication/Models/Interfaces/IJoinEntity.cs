
namespace JHipsterNetSampleApplication.Models.Interfaces {
    public interface IJoinEntity<TEntity> {
        TEntity Navigation { get; set; }
    }
}
