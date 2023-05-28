using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Appointment.Models
{
    public enum AppointmentType
    {
        ClinicalExaminiation=1,
        HomeExamination
    }

    public class Appointment
    {
        [Key]
        public int appointmentID { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor? doctor { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient? patient { get; set; }
        public ICollection<DailyAvailbility> availableDays { get; set; } = new HashSet<DailyAvailbility>();

        [NotMapped]
        public DailyAvailbility? dailyAvailbility { get; set; }

        [EnumDataType(typeof(AppointmentType))]
        public AppointmentType AppointmentType { get; set; }

        public string? MedicalHistory { get; set; }


        }

    }

