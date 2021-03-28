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
        public Task Add(SettingDto setting);
        public Task<Setting> Get(int id);
    }
}
