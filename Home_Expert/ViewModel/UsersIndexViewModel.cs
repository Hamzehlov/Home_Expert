namespace Home_Expert.ViewModel
{
    public class UsersIndexViewModel
    {
        public List<UserDto> Admins { get; set; } = new();
        public List<UserDto> Vendors { get; set; } = new();
        public List<UserDto> Customers { get; set; } = new();

        public int TotalCount => Admins.Count + Vendors.Count + Customers.Count;
        public int ActiveCount => Admins.Concat(Vendors).Concat(Customers).Count(u => u.IsActive);
        public int InactiveCount => TotalCount - ActiveCount;
    }
}
