using System.ComponentModel.DataAnnotations;
using Kintech.Models;

namespace Kintech.ViewModels.Admin;

public class UpdateStatusViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid order ID.")]
    public int OrderId { get; set; }

    [Required]
    public OrderStatus Status { get; set; }  // ✅ enum, not raw string
}