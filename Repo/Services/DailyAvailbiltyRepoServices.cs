using Doctor_Appointment.Data;
using Doctor_Appointment.Models;

namespace Doctor_Appointment.Repo.Services
{
    public class DailyAvailbiltyRepoServices : IDailyAvailbilityRepo
    {
        public ApplicationDbContext Context { get; }
        public DailyAvailbiltyRepoServices(ApplicationDbContext context)
        {
            Context = context;
        }

        public List<DailyAvailbility> GetAll(int docid)
        {
            return Context.dailyAvailbilities.Where(d => d.DoctorID == docid).ToList();
        }

        public DailyAvailbility GetById(int id)
        {
            return Context.dailyAvailbilities.FirstOrDefault(d => d.DoctorID == id);

        }
        public void Insert(DailyAvailbility dailyAvailbility)
        {
            Context.dailyAvailbilities.Add(dailyAvailbility);
            Context.SaveChanges();
        }


        public void Update(int id, DailyAvailbility dailyAvailbility)
        {
            var upd_daily = Context.dailyAvailbilities.FirstOrDefault(d => d.Dayid == id);

            upd_daily.DoctorID = dailyAvailbility.DoctorID;
            upd_daily.date = dailyAvailbility.date;
            upd_daily.starttime = dailyAvailbility.starttime;
            upd_daily.endtime = dailyAvailbility.endtime;
            upd_daily.Isavailable = dailyAvailbility.Isavailable;

            Context.dailyAvailbilities.Update(upd_daily);
        }
        public void Delete(int id)
        {
            var del = Context.dailyAvailbilities.FirstOrDefault(d => d.Dayid == id);
            Context.dailyAvailbilities.Remove(del);
        }
    }
}