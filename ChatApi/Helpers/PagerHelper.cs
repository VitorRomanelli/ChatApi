namespace ChatApi.Helpers
{
    public class Pager
    {
        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }

        public Pager(int totalItems, int? page, int pageSize = 15)
        {
            int num = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int num2 = ((!page.HasValue) ? 1 : page.Value);
            int num3 = num2 - 5;
            int num4 = num2 + 4;
            if (num3 <= 0)
            {
                num4 -= num3 - 1;
                num3 = 1;
            }

            if (num4 > num)
            {
                num4 = num;
                if (num4 > 10)
                {
                    num3 = num4 - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = num2;
            PageSize = pageSize;
            TotalPages = num;
            StartPage = num3;
            EndPage = num4;
        }
    }
}
