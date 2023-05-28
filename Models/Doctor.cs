using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctor_Appointment.Models
{
    public enum Gender
    {
        Female=1,
        Male
    }

    public enum Spectialist
    {
        [Description("Neurology")]
        Neurology = 1,
        [Description("Dentists")]
        Dentists,
        [Description("Ophthalmology")]
        Ophthalmology,
        [Description("Orthopedics")]
        Orthopedics,
        [Description("Cancer Deparment")]
        Cancer_Department,
        [Description("Internal Medicine")]
        Internal_medicine,
        [Description("ENT")]
        ENT

    }

    public enum MedicalDegree
    {
        specialist = 1,

        Advisor,

        professor
    }

    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }

        [Required]
        [MinLength(10)]
        public String FullName { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender gender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [EnumDataType(typeof(Spectialist))]
        public Spectialist specialist { get; set; }

        [Required]
        [EnumDataType(typeof(MedicalDegree))]
        public MedicalDegree Degree { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Clinic_Location { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int Clinic_PhonNum { get; set; }

        public ICollection<DailyAvailbility> availableDays { get; set; } = new HashSet<DailyAvailbility>();
      
        [NotMapped]
        public DailyAvailbility dailyAvailbility { get; set; }
        public bool HomeExamination { get; set; }

        [Required]
        public int Price { get; set; }
        
        public string? WatingTime { get; set; }

        public override string ToString()
        {
            return $"{specialist}";
       
        }

        public string GetImage()
        {
            if (gender == Gender.Male)
            {
                int imageId = (DoctorID % 3) + 1;
                var doctorImage = "/images/team/" + imageId + ".jpg";

                Console.WriteLine($"Doctor Id: ${DoctorID}");
                Console.WriteLine("Doctor Gender: Male");
                Console.WriteLine($"Doctor Image: ${doctorImage}");
                return doctorImage;
            }
            else
            {
                Console.WriteLine($"Doctor Id: ${DoctorID}");
                Console.WriteLine("Doctor Gender: Female");
                Console.WriteLine($"Doctor Image: /images/team/4.jpg");
                return "/images/team/4.jpg";
            }
        }

        public string GetSpecializationDescription()
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])specialist
           .GetType().GetField(specialist.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = attributes.Length > 0 ? attributes[0].Description : specialist.ToString();

            return description;
        }

    }
}
