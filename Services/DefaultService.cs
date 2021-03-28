using Data.Models;
using Data.Repositories;
using Microsoft.Extensions.Logging;
using Services.DTO;
using Services.Exceptions.BadRequest;
using Services.Exceptions.NotFound;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class DefaultService : IDefaultService
    {
        private readonly IGenericRepository<Setting> _repo;
        private readonly ILogger _logger;
        public DefaultService(IGenericRepository<Setting> repo, ILogger<DefaultService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger;
        }
        public async Task<IEnumerable<Setting>> GetAll()
        {
            var settings = await _repo.GetAllAsync();
            return settings;
        }
        public async Task<Setting> Get(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id argument cannot be null or 0.");
                throw new ArgumentNullException(nameof(id));
            }

            var setting = await _repo.FirstOrDefaultAsync(x => x.Id == id);

            if(setting == null)
            {
                _logger.LogError("A Setting with that Id was not found.");
                throw new SettingNotFound();
            }
            return setting;
        }
        public async Task<Setting> Add(SettingDto setting)
        {
            if(setting == null)
            {
                _logger.LogError("Setting argument cannot be null.");
                throw new ArgumentNullException(nameof(setting));
            }

            var findSetting = _repo.FirstOrDefaultAsync(x => x.Key == setting.Key).Result;
            if (findSetting != null)
            {
                _logger.LogError("A Setting with that Key already exist.");
                throw new KeyAlreadyExistException();
            }

            var newSetting = new Setting
            {
                Key = setting.Key,
                Value = setting.Value
            };

            _repo.Add(newSetting);
            await _repo.SaveChangesAsync();

            return newSetting;
        }
        public async Task<Setting> Update(SettingDto setting, int id)
        {
            if (setting == null)
            {
                _logger.LogError("Setting argument cannot be null.");
                throw new ArgumentNullException(nameof(setting));
            }

            if (id <= 0)
            {
                _logger.LogError("Id argument cannot be null or 0.");
                throw new ArgumentNullException(nameof(id));
            }

            var findSetting = _repo.FirstOrDefaultAsync(x => x.Id == id).Result;
            if (findSetting == null)
            {
                _logger.LogError("A Setting with that Id was not found.");
                throw new SettingNotFound();
            }

            findSetting.Key = setting.Key;
            findSetting.Value = setting.Value;

            _repo.Update(findSetting);
            await _repo.SaveChangesAsync();

            return findSetting;
        }
        public async Task<Setting> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id argument cannot be null or 0.");
                throw new ArgumentNullException(nameof(id));
            }

            var findSetting = _repo.FirstOrDefaultAsync(x => x.Id == id).Result;
            if (findSetting == null)
            {
                _logger.LogError("A Setting with that Id was not found.");
                throw new SettingNotFound();
            }

            _repo.Delete(findSetting);
            await _repo.SaveChangesAsync();

            return findSetting;
        }
    }
}
