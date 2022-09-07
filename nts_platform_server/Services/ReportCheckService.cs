using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nts_platform_server.Entities;

namespace nts_platform_server.Services
{

    public interface IReportCheckService
    {
        IEnumerable<ReportCheck> GetAll();
        IEnumerable<CheckPlane> GetCheck_Plane();
        IEnumerable<CheckTrain> GetCheck_Train();
        IEnumerable<CheckHostel> GetCheck_Hostel();
        IEnumerable<ReportCheck> GetCheck_Shop();



        Task<IEnumerable<ReportCheck>> GetAllAsync();
        Task<IEnumerable<CheckPlane>> GetCheck_PlaneAsync();
        Task<IEnumerable<CheckTrain>> GetCheck_TrainAsync();
        Task<IEnumerable<CheckHostel>> GetCheck_HostelAsync();
        Task<IEnumerable<ReportCheck>> GetCheck_ShopAsync();

    }

    public class ReportCheckService : IReportCheckService
    {
        private readonly IEfRepository<ReportCheck> _reportCheck;
        private readonly IEfRepository<CheckPlane> _checkPlane;
        private readonly IEfRepository<CheckTrain> _checkTrain;
        private readonly IEfRepository<CheckHostel> _checkHostel;

        public ReportCheckService (
            IEfRepository<ReportCheck> reportCheck,
            IEfRepository<CheckPlane> checkPlane,
            IEfRepository<CheckTrain> checkTrain,
            IEfRepository<CheckHostel> checkHostel
        )
        {
            _reportCheck = reportCheck;
            _checkPlane = checkPlane;
            _checkTrain = checkTrain;
            _checkHostel = checkHostel;
        }

        public IEnumerable<ReportCheck> GetAll()
        {
            return  _reportCheck.Get().ToList();
        }

        // Вернет все чеки только по Самолету
        public IEnumerable<CheckPlane> GetCheck_Plane()
        {
            return _checkPlane.Get().ToList();
        }

        // Вернет все чеки только по Поездам
        public IEnumerable<CheckTrain> GetCheck_Train()
        {
            return _checkTrain.Get().ToList();
        }

        // Вернет все чеки только по Отелям
        public IEnumerable<CheckHostel> GetCheck_Hostel()
        {
            return _checkHostel.Get().ToList();
        }

        // Вернет все чеки только с Магазинов
        public IEnumerable<ReportCheck> GetCheck_Shop()
        {
            var check = _reportCheck.Get()
               .Where(delegate (ReportCheck item)
               {
                   return item.GetType() == typeof(ReportCheck);
               })
               .ToList();

            return check;
        }


        //Асинхронные методы
        // Вернет все чеки со всеми типами данных асинхронно
        public async Task<IEnumerable<ReportCheck>> GetAllAsync()
        {
            var check = await _reportCheck.toListAsync();

            if(check.Any())
            {
                return check;
            }


            return null;
        }

        public async Task<IEnumerable<CheckPlane>> GetCheck_PlaneAsync()
        {
            var check = await _checkPlane.toListAsync();

            if (check.Any())
            {
                return check;
            }

            return null;
        }

        public async Task<IEnumerable<CheckTrain>> GetCheck_TrainAsync()
        {
            var check = await _checkTrain.toListAsync();

            if (check.Any())
            {
                return check;
            }

            return null;
        }

        public async Task<IEnumerable<CheckHostel>> GetCheck_HostelAsync()
        {
            var check = await _checkHostel.toListAsync();

            if (check.Any())
            {
                return check;
            }

            return null;
        }

        public async Task<IEnumerable<ReportCheck>> GetCheck_ShopAsync()
        {
            var check = await _reportCheck.toListAsync();

            check = check.Where(delegate (ReportCheck item)
            {
                return item.GetType() == typeof(ReportCheck);
            });

            if(check.Any())
            {
                return check;
            }

            return null;
        }

    }
}

