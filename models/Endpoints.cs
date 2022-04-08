namespace models
{
    public static class Endpoints
    {
        public static class Address
        {
            const string address = "address";
            public static string Get(int id)
                => $"{address}/{id}";
        }
    }
}
