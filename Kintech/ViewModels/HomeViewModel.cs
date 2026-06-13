using Kintech.Models;

namespace Kintech.ViewModels.Home;

public class HomeViewModel
{
    public List<Kintech.Models.Product> FeaturedProducts { get; set; } = [];
    public List<Kintech.Models.Product> NewArrivals { get; set; } = [];
    public List<Kintech.Models.Category> Categories { get; set; } = [];
}

