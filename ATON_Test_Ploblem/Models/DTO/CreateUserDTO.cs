namespace ATON_Test_Ploblem.Models.DTO
{
    public class CreateUserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; } // 0 - женщина, 1 - мужчина, 2 - неизвестно
        public bool Admin { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
