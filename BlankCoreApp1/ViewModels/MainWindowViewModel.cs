using System;
using System.Windows.Input;

using BlankCoreApp1.Events;
using BlankCoreApp1.Views;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BlankCoreApp1.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ICommand LoadedCommand { get; set; }

        private string _title = "Prism Application";
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _loginId;
        public string LoginID
        {
            get { return _loginId; }
            set { SetProperty(ref _loginId, value); }
        }

        /// <summary>
        /// 생성자 인젝션
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            LoadedCommand = new DelegateCommand(OnLoaded);
        }

        private void OnLoaded()
        {
            //_eventAggregator.GetEvent<LoginEvent>().Subscribe(ReceivedLogin);

            // cross thread 에러 방지
            //_eventAggregator.GetEvent<LoginEvent>().Subscribe(ReceivedLogin, ThreadOption.UIThread );

            // 필터조건 넣기
            _eventAggregator.GetEvent<LoginEvent>().Subscribe(ReceivedLogin, ThreadOption.UIThread, false, filter=>filter.LoginID.Length > 10);

            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(LoginView));
        }

        private void ReceivedLogin(LoginEventArgs obj)
        {
            this.LoginID = obj.LoginID;
        }
    }
}
