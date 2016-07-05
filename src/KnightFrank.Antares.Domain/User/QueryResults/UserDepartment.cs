namespace KnightFrank.Antares.Domain.User.QueryResults
{
    public class UserDepartment
    {
        public UserDepartment(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}