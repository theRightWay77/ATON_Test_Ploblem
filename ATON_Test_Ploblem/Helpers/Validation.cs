using System.Text.RegularExpressions;

namespace ATON_Test_Ploblem.Helpers
{
    public static class Validation
    {
        public static bool IsValidLogin(string login)
        {
            return !string.IsNullOrEmpty(login) && Regex.IsMatch(login, @"^[a-zA-Z0-9]+$");
        }

        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"^[a-zA-Z0-9]+$");
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[a-zA-Zа-яА-ЯёЁ]+$");
        } 

        public static bool IsValidGender(int gender)
        {
            return gender == 0 || gender == 1 || gender == 2;
        }
    }
}
