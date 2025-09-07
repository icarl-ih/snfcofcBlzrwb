using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;

namespace snfcofcBlzrwb
{
    
    public partial class App : Application
    {   
        private readonly IAuthService _authService;
        public User CurrentUser => _authService.GetCurrentUser();
        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _authService.ClearSession();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "XI INICIAL SFCOFC" };
        }
        protected override void OnSleep()
        {
            base.OnSleep();
            _authService.ClearSession(); // ✅ Limpia la sesión al suspender la app
        }

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //    _authService.ClearSession(); // ✅ Limpia la sesión al cerrar completamente
        //}

    }
}
