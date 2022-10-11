using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using nts_platform_server.Data;
using nts_platform_server.Entities;
using nts_platform_server.Models;
using static iTextSharp.text.pdf.AcroFields;
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

        Task<Object> AddNewCheck(IFormFile File_checkBankPhotoByte, IFormFile File_ticketPhotoByte, IFormFile File_borderTicketPhotoByte, IFormFile File_billPhotoByte,ReportCheckNewModel reportCheckNewModel);
        Task<Object> EditCheck(IFormFile File_checkBankPhotoByte, IFormFile File_ticketPhotoByte, IFormFile File_borderTicketPhotoByte, IFormFile File_billPhotoByte, ReportCheckEditModel reportCheckEditModel);
        Task<bool> DeleteCheckAsync(ReportCheckNewModel reportCheckNewModel);
        Task<IEnumerable<ReportCheck>> GetSelectedChecksAsync(BusinessTripModel businessTripModel);



        Task<IEnumerable<BusinessTrip>> GetAllBusinessTripAsync();
        Task<Object> AddBusinessTripAsync(BusinessTripModel businessTripModel);
        Task<Object> EditBusinessTripAsync(BusinessTripEditModel businessTripModel);
        Task<bool> DeleteBusinessTripAsync(BusinessTripModel businessTripModel);
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

        public async Task<IEnumerable<ReportCheck>> GetSelectedChecksAsync(BusinessTripModel businessTripModel)
        {
            var check = await _businessTrip.Get()
                .Where(x => x.Id == businessTripModel.id)
                .FirstOrDefaultAsync();

            if(check != null)
            {
                _businessTrip.GetContext().Entry(check).Collection(x => x.ReportChecks).Load();
                return check.ReportChecks;
            }

            return null;
        }

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

        public async Task<object> AddNewCheck(IFormFile File_checkBankPhotoByte, IFormFile File_ticketPhotoByte, IFormFile File_borderTicketPhotoByte, IFormFile File_billPhotoByte, ReportCheckNewModel reportCheckNewModel)
        {
            switch (reportCheckNewModel.Discriminator)
            {
                case "ReportCheck":
                    {
                        ReportCheck addNewReport = new ReportCheck();
                        addNewReport.Name = reportCheckNewModel.Name;
                        addNewReport.Date = reportCheckNewModel.Date;
                        addNewReport.Value = reportCheckNewModel.Value;
                        addNewReport.Descriptions = reportCheckNewModel.Descriptions;
                        addNewReport.Discriminator = reportCheckNewModel.Discriminator;

                        BusinessTrip findBuisnessTrip = null;

                        //Проверить существует ли данная командировка в таблице командировок по "Id"
                        if (reportCheckNewModel.BusinessTrip != null)
                        {
                            findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckNewModel.BusinessTrip.Id).FirstOrDefault();

                            if (findBuisnessTrip != null)
                            {
                                //нужен данный код для автоматического подсчета потраченных средств
                                _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                addNewReport.BusinessTripId = reportCheckNewModel.BusinessTrip.Id;
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }

                        addNewReport.CheckBankPhotoName = reportCheckNewModel.CheckBankPhotoName;

                        if (File_checkBankPhotoByte != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                File_checkBankPhotoByte.CopyTo(ms);
                                addNewReport.CheckBankPhotoByte = ms.ToArray();
                            }
                        }

                        await _reportCheck.Add(addNewReport);

                       


                        return await Task.FromResult(addNewReport);
                    }
                case "CheckPlane":
                    {
                        CheckPlane addNewReport = new CheckPlane();
                        addNewReport.Name = reportCheckNewModel.Name;
                        addNewReport.Date = reportCheckNewModel.Date;
                        addNewReport.Value = reportCheckNewModel.Value;
                        addNewReport.Descriptions = reportCheckNewModel.Descriptions;
                        addNewReport.Discriminator = reportCheckNewModel.Discriminator;

                        BusinessTrip findBuisnessTrip = null;

                        //Проверить существует ли данная командировка в таблице командировок по "Id"
                        if (reportCheckNewModel.BusinessTrip != null)
                        {
                            findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckNewModel.BusinessTrip.Id).FirstOrDefault();

                            if (findBuisnessTrip != null)
                            {
                                //нужен данный код для автоматического подсчета потраченных средств
                                _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                addNewReport.BusinessTripId = reportCheckNewModel.BusinessTrip.Id;
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }


                        //Добавление чека на покупку билета
                        if (File_checkBankPhotoByte != null)
                        {
                            addNewReport.CheckBankPhotoName = reportCheckNewModel.CheckBankPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_checkBankPhotoByte.CopyTo(ms);
                                addNewReport.CheckBankPhotoByte = ms.ToArray();
                            }
                        }

                        //Добавление файла бронирование билета
                        if (File_ticketPhotoByte != null)
                        {
                            addNewReport.TicketPhotoName = reportCheckNewModel.TicketPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_ticketPhotoByte.CopyTo(ms);
                                addNewReport.TicketPhotoByte = ms.ToArray();
                            }
                        }

                        //Добавление файла посадочного билета на самолет
                        if (File_borderTicketPhotoByte != null)
                        {
                            addNewReport.BorderTicketPhotoName = reportCheckNewModel.BorderTicketPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_borderTicketPhotoByte.CopyTo(ms);
                                addNewReport.BorderTicketPhotoByte = ms.ToArray();
                            }
                        }

                        await _reportCheck.Add(addNewReport);


                        return await Task.FromResult(addNewReport);
                    }
                case "CheckTrain":
                    {
                        CheckTrain addNewReport = new CheckTrain();
                        addNewReport.Name = reportCheckNewModel.Name;
                        addNewReport.Date = reportCheckNewModel.Date;
                        addNewReport.Value = reportCheckNewModel.Value;
                        addNewReport.Descriptions = reportCheckNewModel.Descriptions;
                        addNewReport.Discriminator = reportCheckNewModel.Discriminator;

                        BusinessTrip findBuisnessTrip = null;

                        //Проверить существует ли данная командировка в таблице командировок по "Id"
                        if (reportCheckNewModel.BusinessTrip != null)
                        {
                            findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckNewModel.BusinessTrip.Id).FirstOrDefault();

                            if (findBuisnessTrip != null)
                            {
                                //нужен данный код для автоматического подсчета потраченных средств
                                _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                addNewReport.BusinessTripId = reportCheckNewModel.BusinessTrip.Id;
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }


                        //Добавление чека на покупку билета на поезд
                        if (File_checkBankPhotoByte != null)
                        {
                            addNewReport.CheckBankPhotoName = reportCheckNewModel.CheckBankPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_checkBankPhotoByte.CopyTo(ms);
                                addNewReport.CheckBankPhotoByte = ms.ToArray();
                            }
                        }

                        //Добавление файла бронирование билета на поезд
                        if (File_borderTicketPhotoByte != null)
                        {
                            addNewReport.BorderTicketPhotoName = reportCheckNewModel.BorderTicketPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_borderTicketPhotoByte.CopyTo(ms);
                                addNewReport.BorderTicketPhotoByte = ms.ToArray();
                            }
                        }

                        await _reportCheck.Add(addNewReport);


                        return await Task.FromResult(addNewReport);
                    }
                case "CheckHostel":
                    {
                        CheckHostel addNewReport = new CheckHostel();
                        addNewReport.Name = reportCheckNewModel.Name;
                        addNewReport.Date = reportCheckNewModel.Date;
                        addNewReport.Value = reportCheckNewModel.Value;
                        addNewReport.Descriptions = reportCheckNewModel.Descriptions;
                        addNewReport.Discriminator = reportCheckNewModel.Discriminator;

                        BusinessTrip findBuisnessTrip = null;

                        //Проверить существует ли данная командировка в таблице командировок по "Id"
                        if (reportCheckNewModel.BusinessTrip != null)
                        {
                            findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckNewModel.BusinessTrip.Id).FirstOrDefault();

                            if (findBuisnessTrip != null)
                            {
                                //нужен данный код для автоматического подсчета потраченных средств
                                _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                addNewReport.BusinessTripId = reportCheckNewModel.BusinessTrip.Id;
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }


                        //Добавление чека на покупку билета на поезд
                        if (File_checkBankPhotoByte != null)
                        {
                            addNewReport.CheckBankPhotoName = reportCheckNewModel.CheckBankPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_checkBankPhotoByte.CopyTo(ms);
                                addNewReport.CheckBankPhotoByte = ms.ToArray();
                            }
                        }

                        //Добавление файла бронирование билета на поезд
                        if (File_billPhotoByte != null)
                        {
                            addNewReport.BillPhotoName = reportCheckNewModel.BillPhotoName;
                            using (var ms = new MemoryStream())
                            {
                                File_billPhotoByte.CopyTo(ms);
                                addNewReport.BillPhotoByte = ms.ToArray();
                            }
                        }
                        await _reportCheck.Add(addNewReport);

                        return await Task.FromResult(addNewReport);
                    }
            }

            return null;
        }



        public async Task<object> EditCheck(IFormFile File_checkBankPhotoByte, IFormFile File_ticketPhotoByte, IFormFile File_borderTicketPhotoByte, IFormFile File_billPhotoByte, ReportCheckEditModel reportCheckEditModel)
        {
            // 1) Найти чек в репозитории
            var check = await _reportCheck.Get().Where(x => x.Id == reportCheckEditModel.old.id).FirstOrDefaultAsync();

            if(check != null)
            {
                //Нашли чек, можем начинать редвактировать

                switch (reportCheckEditModel.edit.Discriminator)
                {
                    case "ReportCheck":
                        {
                            check.Name = reportCheckEditModel.edit.Name;
                            check.Date = reportCheckEditModel.edit.Date;
                            check.Value = reportCheckEditModel.edit.Value;
                            check.Descriptions = reportCheckEditModel.edit.Descriptions;
                            check.Discriminator = reportCheckEditModel.edit.Discriminator;

                            BusinessTrip findBuisnessTrip = null;

                            //Проверить существует ли данная командировка в таблице командировок по "Id"
                            if (reportCheckEditModel.edit.BusinessTrip != null)
                            {
                                findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckEditModel.edit.BusinessTrip.Id).FirstOrDefault();

                                if (findBuisnessTrip != null)
                                {
                                    //нужен данный код для автоматического подсчета потраченных средств
                                    _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                    check.BusinessTripId = reportCheckEditModel.edit.BusinessTrip.Id;
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }

                            check.CheckBankPhotoName = reportCheckEditModel.edit.CheckBankPhotoName;

                            if (File_checkBankPhotoByte != null)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    File_checkBankPhotoByte.CopyTo(ms);
                                    check.CheckBankPhotoByte = ms.ToArray();
                                }
                            }

                            await _reportCheck.Update(check);

                            return await Task.FromResult(check);
                        }
                    case "CheckPlane":
                        {
                            CheckPlane addNewReport = await _checkPlane.Get().Where(x => x.Id == reportCheckEditModel.old.id).FirstOrDefaultAsync();
                            //addNewReport.Id = check.Id;
                            addNewReport.Name = reportCheckEditModel.edit.Name;
                            addNewReport.Date = reportCheckEditModel.edit.Date;
                            addNewReport.Value = reportCheckEditModel.edit.Value;
                            addNewReport.Descriptions = reportCheckEditModel.edit.Descriptions;
                            addNewReport.Discriminator = reportCheckEditModel.edit.Discriminator;

                            BusinessTrip findBuisnessTrip = null;

                            //Проверить существует ли данная командировка в таблице командировок по "Id"
                            if (reportCheckEditModel.edit.BusinessTrip != null)
                            {
                                findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckEditModel.edit.BusinessTrip.Id).FirstOrDefault();

                                if (findBuisnessTrip != null)
                                {
                                    //нужен данный код для автоматического подсчета потраченных средств
                                    _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                    addNewReport.BusinessTripId = reportCheckEditModel.edit.BusinessTrip.Id;
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }


                            //Добавление чека на покупку билета
                            if (File_checkBankPhotoByte != null)
                            {
                                addNewReport.CheckBankPhotoName = reportCheckEditModel.edit.CheckBankPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_checkBankPhotoByte.CopyTo(ms);
                                    addNewReport.CheckBankPhotoByte = ms.ToArray();
                                }
                            }

                            //Добавление файла бронирование билета
                            if (File_ticketPhotoByte != null)
                            {
                                addNewReport.TicketPhotoName = reportCheckEditModel.edit.TicketPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_ticketPhotoByte.CopyTo(ms);
                                    addNewReport.TicketPhotoByte = ms.ToArray();
                                }
                            }

                            //Добавление файла посадочного билета на самолет
                            if (File_borderTicketPhotoByte != null)
                            {
                                addNewReport.BorderTicketPhotoName = reportCheckEditModel.edit.BorderTicketPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_borderTicketPhotoByte.CopyTo(ms);
                                    addNewReport.BorderTicketPhotoByte = ms.ToArray();
                                }
                            }

                            await _reportCheck.Update(addNewReport);


                            return await Task.FromResult(addNewReport);
                        }
                    case "CheckTrain":
                        {
                            CheckTrain addNewReport = await _checkTrain.Get().Where(x => x.Id == reportCheckEditModel.old.id).FirstOrDefaultAsync();
                            //addNewReport.Id = check.Id;
                            addNewReport.Name = reportCheckEditModel.edit.Name;
                            addNewReport.Date = reportCheckEditModel.edit.Date;
                            addNewReport.Value = reportCheckEditModel.edit.Value;
                            addNewReport.Descriptions = reportCheckEditModel.edit.Descriptions;
                            addNewReport.Discriminator = reportCheckEditModel.edit.Discriminator;

                            BusinessTrip findBuisnessTrip = null;

                            //Проверить существует ли данная командировка в таблице командировок по "Id"
                            if (reportCheckEditModel.edit.BusinessTrip != null)
                            {
                                findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckEditModel.edit.BusinessTrip.Id).FirstOrDefault();

                                if (findBuisnessTrip != null)
                                {
                                    //нужен данный код для автоматического подсчета потраченных средств
                                    _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                    addNewReport.BusinessTripId = reportCheckEditModel.edit.BusinessTrip.Id;
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }


                            //Добавление чека на покупку билета на поезд
                            if (File_checkBankPhotoByte != null)
                            {
                                addNewReport.CheckBankPhotoName = reportCheckEditModel.edit.CheckBankPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_checkBankPhotoByte.CopyTo(ms);
                                    addNewReport.CheckBankPhotoByte = ms.ToArray();
                                }
                            }

                            //Добавление файла бронирование билета на поезд
                            if (File_borderTicketPhotoByte != null)
                            {
                                addNewReport.BorderTicketPhotoName = reportCheckEditModel.edit.BorderTicketPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_borderTicketPhotoByte.CopyTo(ms);
                                    addNewReport.BorderTicketPhotoByte = ms.ToArray();
                                }
                            }

                            await _reportCheck.Update(addNewReport);


                            return await Task.FromResult(addNewReport);
                        }
                    case "CheckHostel":
                        {
                            CheckHostel addNewReport = await _checkHostel.Get().Where(x => x.Id == reportCheckEditModel.old.id).FirstOrDefaultAsync();
                            //addNewReport.Id = check.Id;
                            addNewReport.Name = reportCheckEditModel.edit.Name;
                            addNewReport.Date = reportCheckEditModel.edit.Date;
                            addNewReport.Value = reportCheckEditModel.edit.Value;
                            addNewReport.Descriptions = reportCheckEditModel.edit.Descriptions;
                            addNewReport.Discriminator = reportCheckEditModel.edit.Discriminator;

                            BusinessTrip findBuisnessTrip = null;

                            //Проверить существует ли данная командировка в таблице командировок по "Id"
                            if (reportCheckEditModel.edit.BusinessTrip != null)
                            {
                                findBuisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckEditModel.edit.BusinessTrip.Id).FirstOrDefault();

                                if (findBuisnessTrip != null)
                                {
                                    //нужен данный код для автоматического подсчета потраченных средств
                                    _businessTrip.GetContext().Entry(findBuisnessTrip).Collection(x => x.ReportChecks).Load();
                                    addNewReport.BusinessTripId = reportCheckEditModel.edit.BusinessTrip.Id;
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }


                            //Добавление чека на покупку билета на поезд
                            if (File_checkBankPhotoByte != null)
                            {
                                addNewReport.CheckBankPhotoName = reportCheckEditModel.edit.CheckBankPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_checkBankPhotoByte.CopyTo(ms);
                                    addNewReport.CheckBankPhotoByte = ms.ToArray();
                                }
                            }

                            //Добавление файла бронирование билета на поезд
                            if (File_billPhotoByte != null)
                            {
                                addNewReport.BillPhotoName = reportCheckEditModel.edit.BillPhotoName;
                                using (var ms = new MemoryStream())
                                {
                                    File_billPhotoByte.CopyTo(ms);
                                    addNewReport.BillPhotoByte = ms.ToArray();
                                }
                            }

                            await _reportCheck.Update(addNewReport);

                            return await Task.FromResult(addNewReport);
                        }
                }

            }

            return null;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="reportCheckNewModel"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCheckAsync(ReportCheckNewModel reportCheckNewModel)
        {
            //Проверить существует ли Командировка
            var check_buisnessTrip = _businessTrip.Get().Where(x => x.Id == reportCheckNewModel.BusinessTripId);

            if(check_buisnessTrip != null)
            {
                //Нашли командировку
                //Ищем конкретный чек для удаления
                var get = _reportCheck.Get();

                var check = get.
                    Where(x => x.Id == reportCheckNewModel.id)
                   .FirstOrDefault();

                if (check != null)
                {
                    await _reportCheck.Remove(check);

                    return true;
                }

                return false;
            }


            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTripModel"></param>
        /// <returns></returns>
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

        public async Task<Object> EditBusinessTripAsync(BusinessTripEditModel businessTripModel)
        {
            var get = _businessTrip.Get()
                .Include(x => x.UserProject)
                    .ThenInclude(x => x.Project)
                .Include(x => x.UserProject)
                    .ThenInclude(x => x.User);

            //Проверить на существование юзера в  данном проекте в базе.
            //Проверить на существование в базе командировки с одинаковым именем и датой отправления
            var check = get.Where(x =>
            x.UserProject.User.Email == businessTripModel.old.UserProject.User.Email &&
            x.UserProject.Project.Code == businessTripModel.old.UserProject.Project.Code &&
            x.Name == businessTripModel.old.Name &&
            x.DateStart.Date == businessTripModel.old.DateStart.Date
            ).FirstOrDefault();

            if (check != null)
            {
                //В базе уже существует данная командировка

                //Добавить изменения
                if(businessTripModel.edit.Name != "")
                {
                    check.Name = businessTripModel.edit.Name;
                }

                if (businessTripModel.edit.Spent != null)
                {
                    check.Spent = businessTripModel.edit.Spent;
                }

                if (businessTripModel.edit.Descriptions != null)
                {
                    check.Descriptions = businessTripModel.edit.Descriptions;
                }

                check.DateStart = businessTripModel.edit.DateStart;

                if (businessTripModel.edit.UserProject != null)
                {
                    check.UserProject = _userProject.Get().Where(x => x.Project.Code == businessTripModel.edit.UserProject.Project.Code).FirstOrDefault();
                }

                await _businessTrip.Update(check);

                return await Task.FromResult(check);
            }


            return null;
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

        public async Task<bool> DeleteBusinessTripAsync(BusinessTripModel businessTripModel)
        {
            var get = _businessTrip.Get();

            var check = get.Where(x =>
               x.Id == businessTripModel.id
               ).FirstOrDefault();

            if (check != null)
            {
                _businessTrip.GetContext().Entry(check).Collection(x => x.ReportChecks).Load();

                await _businessTrip.Remove(check);

                return true;
            }

            return false;
        }

    
    }
}

