namespace Employee.BusinessLogic.Results
{
    public class PaginatedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
        public int FilteredCount { get; set; }

    }

}
