using BethanyPieShop.Models;

namespace BethanyPieShop.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}
