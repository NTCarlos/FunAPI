using Data.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDefaultService
    {
        public Task<IEnumerable<Setting>> GetAll();
        public Task<Setting> Add(SettingDto setting);
        public Task<Setting> Get(int id);
        public Task<Setting> Update(SettingDto setting, int id);
        public Task<Setting> Delete(int id);
    }
}
