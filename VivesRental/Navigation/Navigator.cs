using System;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Core;

namespace VivesRental.Navigation
{
    public class Navigator : ObservableObject, INavigator
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigatableViewModel _currentViewModel;

        private object _data;


        public Navigator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public object Data
        {
            get => _data;
            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }

        public INavigatableViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public void NavigateTo<T>() where T : INavigatableViewModel
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<T>();
        }

        public void NavigateTo<T>(object data) where T : INavigatableViewModel
        {
            Data = null;
            Data = data;
            CurrentViewModel = _serviceProvider.GetRequiredService<T>();
        }
    }
}