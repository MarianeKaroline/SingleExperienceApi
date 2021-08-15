using System.Collections;
using System.Threading.Tasks;

namespace SingleExperience.Repository
{
    public interface ISingleExperienceRepository
    {
        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        Task<bool> SaveChangeAsync();

    }
}
