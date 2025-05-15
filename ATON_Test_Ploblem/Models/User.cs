namespace ATON_Test_Ploblem.Models
{
    public class User
    {
        public Guid Guid;
        public string Login;
        public string Password;
        public string Name;
        public int Gender; // 0 - женщина, 1 - мужчина, 2 - неизвестно
        public DateTime? Birthday;
        public bool Admin;
        public DateTime CreatedOn;
        public string CreatedBy;
        public DateTime ModifiedOn;
        public string ModifiedBy;
        public DateTime? RevokedOn;
        public string RevokedBy;
    }
}
