namespace HealthCare.Dtos
{
    public class VisitDTO
    {
        public int VisitId { get; set; }
        public int? PtId { get; set; }
        public int? DoctorId { get; set; }
        public DateOnly? VisitDate { get; set; }
        public string Purpose { get; set; }
    }

}
