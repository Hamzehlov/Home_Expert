namespace Home_Expert.ViewModel
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";


        public string Phone { get; set; } = "";
    }
}
