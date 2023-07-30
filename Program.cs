
static void PaginateList(int? before = null, int? after = null, int pageSize = 2)
{
    var items = new List<Item>
    {
        new(1, "Item 1"),
        new(2, "Item 2"),
        new(3, "Item 3"),
        new(4, "Item 4"),
        new(5, "Item 5"),
        new(6, "Item 6"),
        new(7, "Item 7"),
        new(8, "Item 8"),
        new(9, "Item 9"),
        new(10, "Item 10"),
    };
    var result = items.Paginate(before, after, pageSize);
    var (firstId, nextId) = result switch
    {
        [] => (0, 0),
        [var one] => (one.Id, one.Id),
        [var one, .., var last] => (one.Id, last.Id)
    };

    var finalData = result.Take(pageSize).ToList();
    Console.WriteLine($"Before: {before}    After: {after}    Requested Page size: {pageSize}    Actual Page size: {finalData.Count}    FirstId: {firstId}    NextId: {nextId} \n\n");

    foreach(var item in finalData)
    {
        Console.WriteLine(item.Id);
    }

    Console.WriteLine($"=====================================================================================================================================================\n\n");
}


Console.WriteLine("Start paginating ...");
PaginateList(null, 1, 10);


public interface IHasIdProperty
{
    int Id { get; set; }
}


record Item(int Id, string Name) : IHasIdProperty
{
    public int Id { get; set; } = Id;

    public string Name { get; set; } = Name;
}

static class CustomPaginator
{
    public  static IList<T> Paginate<T>(this IList<T> allItems,  int? beforeCursor, int? afterCursor, int pageSize = 2) where T : IHasIdProperty
        => allItems
            .OrderBy(s => s.Id)
            .Where(x => beforeCursor == null || x.Id <= beforeCursor)
            .Where(x => afterCursor == null || x.Id >= afterCursor)
            .Take(pageSize + 1)
            .ToList();
}
