﻿using SmartGenealogy.ViewModels;

namespace SmartGenealogy.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}