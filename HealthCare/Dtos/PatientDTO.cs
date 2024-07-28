namespace HealthCare.Dtos
{
    public class PatientDTO
    {
        public int PtId { get; set; }
        public string PtName { get; set; }
        public string Gender { get; set; }
        public DateOnly? Dob { get; set; }
    }

}
