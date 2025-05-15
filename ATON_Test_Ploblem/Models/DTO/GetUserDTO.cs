namespace ATON_Test_Ploblem.Models.DTO
{
    public class GetUserDTO
    {
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; } // 0 - женщина, 1 - мужчина, 2 - неизвестно
        public bool Admin { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string RevokedBy { get; set; }
    }
}
