using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctor_Appointment.Models
{
    public class DailyAvailbility
    {
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }

        [Key]
        public int Dayid { get; set; }

        public DateTime date { get; set; }

        public TimeSpan starttime { get; set; }

        public TimeSpan endtime { get; set; }

        public bool Isavailable { get; set; }

    }
}
