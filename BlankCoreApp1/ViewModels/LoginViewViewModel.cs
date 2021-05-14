using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using BlankCoreApp1.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace BlankCoreApp1.ViewModels
{
    public class LoginViewViewModel : BindableBase
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _boolCancel;
        public bool BoolCancel
        {
            get { return _boolCancel; }
            set { SetProperty(ref _boolCancel, value); }
        }

        /// <summary>
        /// 비동기 작업시 login 버튼 액션을 막기 위함
        /// </summary>
        //
        //private bool _iswork;

        private bool _isWork;
        public bool IsWork
        {
            get { return _isWork; }
            set { SetProperty(ref _isWork, value); }
        }

        private readonly IEventAggregator _eventAggregator;

        public ICommand LoginCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public LoginViewViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            //LoginCommand = new DelegateCommand(OnLogin, CanLogin) //new DelegateCommand(OnLogin, () => !string.IsNullOrEmpty(ID)) // 이렇게도 써도 됨
            //    .ObservesProperty(() => ID)
            //    .ObservesProperty(() => Password);

            // 비동기 호출
            LoginCommand = new DelegateCommand(async() => await OnLoginAsync(), CanLogin) //new DelegateCommand(OnLogin, () => !string.IsNullOrEmpty(ID)) // 이렇게도 써도 됨
                .ObservesProperty(() => ID)
                .ObservesProperty(() => Password)
                .ObservesProperty(() => IsWork);

            CancelCommand = new DelegateCommand(async () => await OnCancel())
                //.ObservesCanExecute(() => !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Password)); // 이거 안됨
               .ObservesCanExecute(() => BoolCancel);
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Password) && !IsWork;
        }

        private async Task OnLoginAsync()
        {
            IsWork = true;
            BoolCancel = !BoolCancel;
            //((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();

            await Task.Delay(2000);
            MessageBox.Show("Login Complete");

            _eventAggregator.GetEvent<LoginEvent>().Publish(new LoginEventArgs() { LoginID = ID });

            IsWork = false;
        }

        private bool CanCancel()
        {
            return !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Password);
        }

        private async Task OnCancel()
        {
            await Task.Delay(2000);
            _eventAggregator.GetEvent<LoginEvent>().Publish(new LoginEventArgs() { LoginID = ID + " Cancel" });

            MessageBox.Show("Cancel Complete");
        }
    }
}
