namespace API_EF.Helper
{
    public static class TypeSafe
    {

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Employee = "Employee";
        }
        public static class Permistion
        {
            public const int None = 0;
            public const int Read = 1;
            public const int Write = 2;
            public const int UpDate = 3;
            public const int Delete = 4;
        }

        public static class Controller
        {
            public const string Game = "GameController";
            public const string Auth = "AuthController";
        }
        public static class Policy
        {
            public const string ReadPolicy = "ReadPolicy";
            public const string ReadAndWrite = "ReadAndWritePolicy";
            public const string FullPolicy = "FullPolicy";
        }
    }
}
