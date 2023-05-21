using DocumentFormat.OpenXml.Bibliography;
using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop.BUS
{
    internal class OrderBUS
    {
        private OrderDAO _orderDAO;

        public OrderBUS()
        {
            _orderDAO = new OrderDAO();
        }
        public Tuple<List<OrderDTO>, int> findOderBySearch(int userID,int currentPage = 1, int rowsPerPage = 6,
             string keyword = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            var origin = _orderDAO.getOrders(userID);

            // TODO: nên handle việc ProName bị null ở đây .
            // 
            var list = origin
       .Where((item) =>
       {
           bool checkName = item.ProName.ToLower().Contains(keyword.ToLower());

           bool checkStartDate = startDate == null || item.OrderDate >= startDate;
           bool checkEndDate = endDate == null || item.OrderDate <= endDate;

           return checkName && checkStartDate && checkEndDate;
       });


            var items = list.Skip((currentPage - 1) * rowsPerPage)
                    .Take(rowsPerPage);

            var result = new Tuple<List<OrderDTO>, int>(
                   items.ToList(), list.Count()
               );


            return result;
        }


        public bool createUser(int proID, int userID, string address, DateTime orderDate, DateTime deliveryDate)
        {
            return _orderDAO.CreateOrder( proID,  userID,  address,  orderDate,  deliveryDate);
        }
/*        public List<OrderDTO>(int userID)
        {
            return OrderDAO.GetOrders(userID);
        }*/
        public OrderDTO GetOne(int data)
        {
            return _orderDAO.GetOne(data);
        }
        public bool DeleteOne(int orderID)
        {
            return _orderDAO.DeleteOne(orderID);
        }
        public bool UpdateOne(int orderID, String newAdress)
        {
            return _orderDAO.UpdateOne(orderID, newAdress);
        }

        public int addShopOrder(ShopOrderDTO shopOrderDTO)
        {
            return _orderDAO.insertShopOrder(shopOrderDTO);
        }

        public void addPurchase(PurchaseDTO purchaseDTO)
        {
            _orderDAO.insertPurchase(purchaseDTO);
        }

        public void delPurchaseById(int id)
        {
            _orderDAO.deletePurchaseById(id);
        }

        public void patchShopOrder(ShopOrderDTO shopOrderDTO)
        {
            _orderDAO.updateShopOrder(shopOrderDTO);
        }

        public async Task<Tuple<ObservableCollection<ShopOrderDTO>, int>> findOrderBySearch(int currentPage, int rowsPerPage, DateTime? startDate, DateTime? endDate)
        {
            var origin = await _orderDAO.getAll();

            var list = origin
                .Where((item) =>
                {
                    bool checkStartDate = startDate == null || item.CreateAt >= startDate;
                    bool checkEndDate = endDate == null || item.CreateAt <= endDate;
                    return checkStartDate && checkEndDate;
                });

            var items = list.Skip((currentPage - 1) * rowsPerPage)
                    .Take(rowsPerPage);

            var oc = new ObservableCollection<ShopOrderDTO>();
            foreach (var item in items.ToList())
                oc.Add(item);

            var result = new Tuple<ObservableCollection<ShopOrderDTO>, int>(
                   oc, list.Count()
            );

            return result;
        }


        // tính lợi nhuận trên một sản phẩm
        public decimal calProductProfit(decimal productPrice)
        {
            float profit = 1.07f;

            decimal result = productPrice * (decimal)profit;

            return result;
        }

        public async Task<int> countTotalOrderbyLastWeek()
        {
            var orders = await _orderDAO.getAll();

            DateTime today = DateTime.Today;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var ordersByLastWeek = orders.Where(
                order => order.CreateAt >= startOfWeek.Date && order.CreateAt <= endOfWeek.Date)
                .ToList();

            return ordersByLastWeek.Count;
        }

        public List<PurchaseDTO> findPurchaseDTOs(int orderID)
        {
            return _orderDAO.getPurchaseDTOs(orderID);
        }

        public void delOrderById(int orderID)
        {
            _orderDAO.deleteOrderById(orderID);
        }

        public List<PurchaseDTO> findPurchasesByCusID(int id)
        {
            return _orderDAO.getPuchasesbyCusID(id);
        }
    }
}
