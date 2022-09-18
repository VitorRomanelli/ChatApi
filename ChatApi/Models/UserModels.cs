using ChatApi.Entities;
using ChatApi.Helpers;

namespace ChatApi.Models
{
    public class UserPaginateModel
    {
        public List<User> Users { get; set; }
        public Pager Pager { get; set; }

        public UserPaginateModel(List<User> suppliers, Pager pager)
        {
            Pager = pager;
            Users = suppliers.Skip((Pager.CurrentPage - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
        }
    }

    public class UserFilterModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
