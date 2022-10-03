using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nts_platform_server.Data;
using nts_platform_server.Entities;
using nts_platform_server.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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



        Task<IEnumerable<BusinessTrip>> GetAllBusinessTripAsync();
        Task<Object> AddBusinessTripAsync(BusinessTripModel businessTripModel);
        Task<Object> FinishBusinessTripAsync(BusinessTripModel businessTripModel);


    }

    public class ReportCheckService : IReportCheckService
    {
        private readonly IEfRepository<ReportCheck> _reportCheck;
        private readonly IEfRepository<CheckPlane> _checkPlane;
        private readonly IEfRepository<CheckTrain> _checkTrain;
        private readonly IEfRepository<CheckHostel> _checkHostel;

        private readonly IEfRepository<BusinessTrip> _businessTrip;
        private readonly IEfRepository<UserProject> _userProject;

        public ReportCheckService (
            IEfRepository<ReportCheck> reportCheck,
            IEfRepository<CheckPlane> checkPlane,
            IEfRepository<CheckTrain> checkTrain,
            IEfRepository<CheckHostel> checkHostel,
            IEfRepository<BusinessTrip> businessTrip,
            IEfRepository<UserProject> userProject
        )
        {
            _reportCheck = reportCheck;
            _checkPlane = checkPlane;
            _checkTrain = checkTrain;
            _checkHostel = checkHostel;
            _businessTrip = businessTrip;
            _userProject = userProject;
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

        public async Task<IEnumerable<BusinessTrip>> GetAllBusinessTripAsync()
        {
            var check = await _businessTrip.toListAsync();

            if (check.Any())
            {
                foreach(var item in check)
                {
                    _businessTrip.GetContext().Entry(item).Reference(x => x.UserProject).Load();
                    _businessTrip.GetContext().Entry(item.UserProject).Reference(x => x.Project).Load();
                    _businessTrip.GetContext().Entry(item).Collection(x => x.ReportChecks).Load();
                }
               
                return check;
            }

            return null;
        }

        public async Task<Object> AddBusinessTripAsync(BusinessTripModel businessTripModel)
        {
            var get = _businessTrip.Get()
                .Include(x => x.UserProject)
                    .ThenInclude(x => x.Project)
                .Include(x => x.UserProject)
                    .ThenInclude(x => x.User);

            //Проверить на существование юзера в  данном проекте в базе.
            //Проверить на существование в базе командировки с одинаковым именем и датой отправления
            var check = get.Any(x =>
            x.UserProject.User.Email == businessTripModel.UserProject.User.Email &&
            x.UserProject.Project.Code == businessTripModel.UserProject.Project.Code &&
            x.Name == businessTripModel.Name &&
            x.DateStart.Date == businessTripModel.DateStart.Date
            );

            if(check)
            {
                //В базе уже существует данная командировка
                return null;
            }


            var newBusnesTrip = new BusinessTrip();
            newBusnesTrip.DateStart = businessTripModel.DateStart;
            newBusnesTrip.Descriptions = businessTripModel.Descriptions;
            newBusnesTrip.Name = businessTripModel.Name;
            newBusnesTrip.Spent = businessTripModel.Spent;
            newBusnesTrip.UserProject = _userProject.Get().Where(x => x.Project.Code == businessTripModel.UserProject.Project.Code).FirstOrDefault();

            await _businessTrip.Add(newBusnesTrip);

            return await Task.FromResult(newBusnesTrip);
        }

        public async Task<object> FinishBusinessTripAsync(BusinessTripModel businessTripModel)
        {
            var get = _businessTrip.Get();

            //Проверить на существование юзера в  данном проекте в базе.
            //Проверить на существование в базе командировки с одинаковым именем и датой отправления
            var check = get.Where(x =>
            x.Id == businessTripModel.id
            ).FirstOrDefault();

            if (check != null)
            {
                check.DateEnd = businessTripModel.DateEnd;
                await _businessTrip.Update(check);

                return await Task.FromResult(check);
            }

            return null;
        }
    }
}

