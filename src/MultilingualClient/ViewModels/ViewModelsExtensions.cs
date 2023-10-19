namespace MultilingualClient.ViewModels;

public static class ViewModelsExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        
        ////Onboarding
        builder.Services.AddTransient<MainViewModel>();
        //builder.Services.AddTransient<InverterViewModel>();
        //builder.Services.AddTransient<FirstSyncViewModel>();
        //builder.Services.AddTransient<EnergyCalculationParameterViewModel>();
        //builder.Services.AddTransient<InvestmentAndLoanViewModel>();
        ////ROI
        //builder.Services.AddTransient<RoiViewModel>();
        ////Energy
        //builder.Services.AddTransient<EnergyViewModel>();
        ////More
        //builder.Services.AddTransient<MoreViewModel>();
        return builder;
    }
}

