namespace Home_Expert.ViewModel
{
    public class UserDto
    {
        public string Id { get; set; } = "";
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
