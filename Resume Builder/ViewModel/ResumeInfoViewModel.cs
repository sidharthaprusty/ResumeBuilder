using System.ComponentModel.DataAnnotations;

namespace Resume_Builder.ViewModel
{
    public class ResumeInfoViewModel
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [StringLength(10)]
        public string phone { get; set; }

        [StringLength(400)]
        public string careerObj { get; set; }
        public string dob { get; set; }
        public string sex { get; set; }
        public string maritalSts { get; set; }

        [StringLength(50)]
        public string father { get; set; }

        public string nationality { get; set; }
        public string[] acachievements { get; set; }
        public string[] skills { get; set; }
        public string academics10 { get; set; }
        public string academics12 { get; set; }
        public string academicsGr { get; set; }
        public string academicsPG { get; set; }
    }
}