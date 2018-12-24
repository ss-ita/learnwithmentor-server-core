namespace LearnWithMentor.Constants
{
    public static class Roles
    {
        public const string Mentor = "Mentor";
        public const string Student = "Student";
        public const string Admin = "Admin";
        public const string Blocked = "blocked";
        public const int BlockedIndex = -1;
    }

    public static class ImageRestrictions
    {
        public const int MaxSize = 1024 * 1024 * 1; // 1MB
        public static readonly string[] Extensions = { ".jpeg", ".jpg", ".png" };
    }

    public static class Token
    {
        public const string SecretString = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
    }
    public static class Cors
    {
        public const string policyName = "MyPolicy";
    }
    public static class Logger
    {
        public const string logFileName = "logger.txt";
    }

    public static class Email
    {
        public const string BaseEmail = "learnwithmentor@gmail.com";
        public const string EmailPassword = "learnwithmentor2018";
        public const string SmtpClient = "smtp.gmail.com";
        public const int SmtpClientPort = 587;
    }

    public static class Facebook
    {
        public const string AppId = "318651702058203";
        public const string AppSecret = "635231a956d251de549421d85c7d546e";
    }
}
