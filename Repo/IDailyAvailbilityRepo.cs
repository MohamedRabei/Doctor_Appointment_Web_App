using Doctor_Appointment.Models;

namespace Doctor_Appointment.Repo
{
    public interface IDailyAvailbilityRepo
    {
        public List<DailyAvailbility> GetAll(int docid);
        public DailyAvailbility GetById(int id);
        public void Insert(DailyAvailbility dailyAvailbility);
        public void Update(int id, DailyAvailbility dailyAvailbility);
        public void Delete(int id);
    }
}
