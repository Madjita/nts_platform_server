using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nts_platform_server.Entities;

namespace nts_platform_server.Services
{

    public interface IReportCheckService
    {
        public IEnumerable<ReportCheck> GetAll();
        public IEnumerable<CheckPlane> GetCheck_Plane();
        public IEnumerable<CheckTrain> GetCheck_Train();
        public IEnumerable<CheckHostel> GetCheck_Hostel();
        public IEnumerable<ReportCheck> GetCheck_Shop();
    }

    public class ReportCheckService : IReportCheckService
    {
        private readonly IEfRepository<ReportCheck> _reportCheck;
        private readonly IEfRepository<CheckPlane> _checkPlane;
        private readonly IEfRepository<CheckTrain> _checktrain;
        private readonly IEfRepository<CheckHostel> _checkHostel;

        public ReportCheckService (
            IEfRepository<ReportCheck> reportCheck,
            IEfRepository<CheckPlane> checkPlane,
            IEfRepository<CheckTrain> checktrain,
            IEfRepository<CheckHostel> checkHostel
        )
        {
            _reportCheck = reportCheck;
            _checkPlane = checkPlane;
            _checktrain = checktrain;
            _checkHostel = checkHostel;
        }

        // Вернет все чеки со всеми типами данных
        public IEnumerable<ReportCheck> GetAll()
        {
            return _reportCheck.Get().ToList();
        }

        // Вернет все чеки только по Самолету
        public IEnumerable<CheckPlane> GetCheck_Plane()
        {
            return _checkPlane.Get().ToList();
        }

        // Вернет все чеки только по Поездам
        public IEnumerable<CheckTrain> GetCheck_Train()
        {
            return _checktrain.Get().ToList();
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

    }
}

