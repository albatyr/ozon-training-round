const int min = 1;
const int maxTests = 1000;
const int maxOrders = 5 * 100000;
const int maxTrucks = maxOrders;
const int maxArrival = 1000000000;
const int maxTruckStart = 1000000000;

using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

if (!int.TryParse(input.ReadLine(), out var testsCount) || testsCount is < min or > maxTests)
    return;

for (var i = 0; i < testsCount; i++)
{
    if (!TryReadAndValidateTestData(input, out var testData))
        return;

    var orders = AssignOrdersToTrucks(testData);

    output.WriteLine(string.Join(' ', orders.Select(x => x.TruckId)));
}

bool TryReadAndValidateTestData(StreamReader input, out TestData data)
{
    data = null;

    if (!int.TryParse(input.ReadLine(), out var ordersCount) || ordersCount is < min or > maxOrders)
        return false;

    var arrivalsLine = input.ReadLine()?.Split(' ');
    if (arrivalsLine == null || arrivalsLine.Length != ordersCount)
        return false;

    var orders = new SortedSet<Order>(new OrderComparer());

    for (var i = 0; i < ordersCount; i++)
    {
        if (!int.TryParse(arrivalsLine[i], out var arrival) || arrival is < min or > maxArrival)
            return false;

        orders.Add(new Order { Id = i + 1, Arrival = arrival });
    }

    if (!int.TryParse(input.ReadLine(), out var trucksCount) || trucksCount is < min or > maxTrucks)
        return false;

    var trucks = new SortedSet<Truck>(new TruckComparer());

    for (var i = 0; i < trucksCount; i++)
    {
        var truckData = input.ReadLine()?.Split(' ');
        if (truckData == null || truckData.Length != 3)
            return false;

        if (!int.TryParse(truckData[0], out var start) || start is < min or > maxTruckStart)
            return false;

        if (!int.TryParse(truckData[1], out var end) || end is < min || start > end)
            return false;

        if (!int.TryParse(truckData[2], out var capacity) || capacity < min || capacity > ordersCount)
            return false;

        trucks.Add(new Truck { Id = i + 1, Start = start, End = end, Capacity = capacity });
    }

    data = new TestData { Orders = orders, Trucks = trucks };
    return true;
}

List<Order> AssignOrdersToTrucks(TestData data)
{
    foreach (var order in data.Orders)
    {
        var suitableTruck = data.Trucks
            .FirstOrDefault(t => t.Start <= order.Arrival && t.End >= order.Arrival && t.Capacity > 0);

        if (suitableTruck == null)
        {
            order.TruckId = -1;
            continue;
        }

        order.TruckId = suitableTruck.Id;
        suitableTruck.Capacity--;
    }

    return data.Orders.OrderBy(x => x.Id).ToList();
}

class TestData
{
    public SortedSet<Order> Orders { get; set; }
    public SortedSet<Truck> Trucks { get; set; }
}

class Truck
{
    public int Id { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Capacity { get; set; }
}

class Order
{
    public int Id { get; set; }
    public int Arrival { get; set; }
    public int TruckId { get; set; } = -1;
}

class OrderComparer : IComparer<Order>
{
    public int Compare(Order x, Order y)
    {
        var result = x.Arrival.CompareTo(y.Arrival);

        if (result != 0) return result;

        return x.Id.CompareTo(y.Id);
    }
}

class TruckComparer : IComparer<Truck>
{
    public int Compare(Truck x, Truck y)
    {
        var result = x.Start.CompareTo(y.Start);

        if (result != 0) return result;

        return x.Id.CompareTo(y.Id);
    }
}