using SingleExperience.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleExperience.Repository
{
    public class SingleExperienceRepository : ISingleExperienceRepository
    {
        private readonly Context context;

        public SingleExperienceRepository(Context context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

    }
}
