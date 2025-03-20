namespace CampAdmin.API.Data
{
    public static class Roles
    {
        public const string Admin = "Admin";


        public static string toList(List<string> roles)
        {
            return string.Join(",", roles);
        }
    }
}
