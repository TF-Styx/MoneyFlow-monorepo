using Microsoft.EntityFrameworkCore;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.EntityModel;
using System.Diagnostics;

namespace MoneyFlow.Infrastructure.Repositories
{
    public class GendersRepository : IGendersRepository
    {
        private readonly Func<ContextMF> _factory;

        public GendersRepository(Func<ContextMF> factor)
        {
            _factory = factor;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> CreateAsync(string genderName)
        {
            using (var context = _factory())
            {
                var genderEntity = new Gender
                {
                    GenderName = genderName,
                };

                await context.AddAsync(genderEntity);
                await context.SaveChangesAsync();

                var idGender = await context.Genders.Where(x => x.GenderName == genderName).Select(x => x.IdGender).FirstOrDefaultAsync();

                return idGender;
            }
        }
        public int Create(string genderName)
        {
            using (var context = _factory())
            {
                var genderEntity = new Gender
                {
                    GenderName = genderName,
                };

                context.Add(genderEntity);
                context.SaveChanges();

                //var idGender = context.Genders.FirstOrDefault(x => x.GenderName == genderName).IdGender;
                var idGender = context.Genders.Where(x => x.GenderName == genderName).Select(x => x.IdGender).FirstOrDefault();

                return idGender;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<List<GenderDomain>> GetAllAsync()
        {
            using (var context = _factory())
            {
                var genderList = new List<GenderDomain>();
                var genderEntities = await context.Genders.ToListAsync();

                foreach (var item in genderEntities)
                {
                    if (item == null)
                    {
                        Debug.WriteLine("<-- Не удалось получить Gender -->");
                        continue;
                    }

                    var (genderDomain, message) = GenderDomain.Create(item.IdGender, item.GenderName);

                    if (genderDomain == null)
                    {
                        Debug.WriteLine($"<-- Не удалось создать GenderDomain при получении из БД с Id {item.IdGender}. Ошибка:{message} -->");
                        continue;
                    }

                    genderList.Add(genderDomain);
                }

                return genderList;
            }
        }
        public List<GenderDomain> GetAll()
        {
            using (var context = _factory())
            {
                var genderList = new List<GenderDomain>();
                var genderEntities = context.Genders.ToList();

                foreach (var item in genderEntities)
                {
                    if (item == null)
                    {
                        Debug.WriteLine("<-- Не удалось получить Gender -->");
                        continue;
                    }

                    var genderDomain = GenderDomain.Create(item.IdGender, item.GenderName);

                    if (genderDomain.GenderDomain == null)
                    {
                        Debug.WriteLine($"<-- Не удалось создать GenderDomain при получении из БД с Id {item.IdGender}. Ошибка:{genderDomain.Message} -->");
                        continue;
                    }

                    genderList.Add(genderDomain.GenderDomain);
                }

                return genderList;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<GenderDomain> GetAsync(int idGender)
        {
            using (var context = _factory())
            {
                var genderEntity = await context.Genders.FirstOrDefaultAsync(x => x.IdGender == idGender);
                var genderDomain = GenderDomain.Create(genderEntity.IdGender, genderEntity.GenderName);

                if (genderDomain.GenderDomain == null)
                {
                    Debug.WriteLine($"<-- Не удалось создать GenderDomain с таким IdGender {idGender} -->");
                    return null;
                }

                return genderDomain.GenderDomain;
            }
        }
        public GenderDomain Get(int idGender)
        {
            using (var context = _factory())
            {
                var genderEntity = context.Genders.FirstOrDefault(x => x.IdGender == idGender);
                var genderDomain = GenderDomain.Create(genderEntity.IdGender, genderEntity.GenderName);

                if (genderDomain.GenderDomain == null)
                {
                    Debug.WriteLine($"<-- Не удалось создать GenderDomain с таким IdGender {idGender} -->");
                    return null;
                }

                return genderDomain.GenderDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<GenderDomain?> GetAsync(string genderName)
        {
            using (var context = _factory())
            {
                var genderEntity = await context.Genders.FirstOrDefaultAsync(x => x.GenderName.ToLower() == genderName.ToLower());

                if (genderEntity == null) { return null; }

                var genderDomain = GenderDomain.Create(genderEntity.IdGender, genderEntity.GenderName);

                if (genderDomain.GenderDomain == null)
                {
                    Console.WriteLine($"Не удалось создать GenderDomain с таким GenderName {genderName}");
                    return null;
                }

                return genderDomain.GenderDomain;
            }
        }
        public GenderDomain? Get(string genderName)
        {
            using (var context = _factory())
            {
                var genderEntity = context.Genders.FirstOrDefault(x => x.GenderName.ToLower() == genderName.ToLower());

                if (genderEntity == null) { return null; }

                var genderDomain = GenderDomain.Create(genderEntity.IdGender, genderEntity.GenderName);

                if (genderDomain.GenderDomain == null) 
                {
                    Debug.WriteLine($"Не удалось создать GenderDomain с таким GenderName {genderName}");
                    return null; 
                }

                return genderDomain.GenderDomain;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<int> UpdateAsync(int idGender, string genderName)
        {
            using (var context = _factory())
            {
                var entity = await context.Genders.FirstOrDefaultAsync(x => x.IdGender == idGender);

                if (entity == null) { return -1; }

                entity.GenderName = genderName;

                context.Update(entity);
                context.SaveChanges();

                return entity.IdGender;
            }
        }
        public int Update(int idGender, string genderName)
        {
            using (var context = _factory())
            {
                var entity = context.Genders.FirstOrDefault(x => x.IdGender == idGender);

                if (entity == null) { return -1; }

                entity.GenderName = genderName;

                context.Update(entity);
                context.SaveChanges();

                return entity.IdGender;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task DeleteAsync(int idGender)
        {
            using (var context = _factory())
            {
                await context.Genders.Where(x => x.IdGender == idGender).ExecuteDeleteAsync();
            }
        }
        public void Delete(int idGender)
        {
            using (var context = _factory())
            {
                context.Genders.Where(x => x.IdGender == idGender).ExecuteDelete();
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
