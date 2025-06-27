using MVC_FinalProject.Models.Account;

namespace MVC_FinalProject.ViewModels
{
    public class MyAccountVM
    {
        public IEnumerable<SettingVM> Setting { get; set; }
        public UpdateEmail UpdateEmail { get; set; } = new();     
        public UpdateUsername UpdateUsername { get; set; } = new();
    }
}
