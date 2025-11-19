// Program.cs или App.axaml.cs

using ConsoleApp1;

public override void RegisterServices()
{
    var config = ConfigLoader.LoadConfig();
    
    Services.AddSingleton(config);
    Services.AddTransient<IProductRepository, ProductRepository>();
    Services.AddTransient<IProductService, ProductService>();
    Services.AddTransient<MainViewModel>();
}

// Для использования AsyncRelayCommand добавьте пакет:
// CommunityToolkit.Mvvm