using DocumentFormat.OpenXml.Wordprocessing;
using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BUS
{
    public class CustomerBUS
    {
        private CustomerDAO _customerDAO;

        public CustomerBUS()
        {
            _customerDAO = new CustomerDAO();
        }

        public ObservableCollection<CustomerDTO> getAll() { return _customerDAO.getAll(); }

        public int addCustomer(CustomerDTO customerDTO)
        {
            return _customerDAO.insertCustomer(customerDTO);
        }

        public string getNameById(int cusID)
        {
            return _customerDAO.getNameById(cusID);
        }

        public CustomerDTO findCustomerById(int cusID)
        {
            return _customerDAO.getCustomerById(cusID);
        }

        public Tuple<ObservableCollection<CustomerDTO>, int> findCustomersBySearch(int currentPage = 1, int rowsPerPage = 9,
                string keyword = "")
        {
            var origin = _customerDAO.getAll();

            var list = origin
                .Where((item) =>
                {
                    bool checkName = item.CusName.ToLower().Contains(keyword.ToLower());

                    return checkName;
                });

            var items = list.Skip((currentPage - 1) * rowsPerPage)
                    .Take(rowsPerPage);

            var oc = new ObservableCollection<CustomerDTO>();
            foreach (var item in items.ToList())
                oc.Add(item);

            var result = new Tuple<ObservableCollection<CustomerDTO>, int>(
                   oc, list.Count()
            );

            return result;
        }

        public void delCustomerById(int id)
        {
            _customerDAO.delCustomerById(id);
        }
    }
}
