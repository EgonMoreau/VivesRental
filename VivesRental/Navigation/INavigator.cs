using System;
using VivesRental.Core;

namespace VivesRental.Navigation
{
    public interface INavigator
    {
        INavigatableViewModel CurrentViewModel { get; set; }
        Object Data { get; set; }
        void NavigateTo<T>() where T : INavigatableViewModel;
        void NavigateTo<T>(Object data) where T : INavigatableViewModel;


    }
}