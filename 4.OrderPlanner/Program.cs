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

    var orders = new List<Order>(ordersCount);

    for (var i = 0; i < ordersCount; i++)
    {
        if (!int.TryParse(arrivalsLine[i], out var arrival) || arrival is < min or > maxArrival)
            return false;

        orders.Add(new Order { Id = i + 1, Arrival = arrival });
    }

    if (!int.TryParse(input.ReadLine(), out var trucksCount) || trucksCount is < min or > maxTrucks)
        return false;

    var trucks = new List<Truck>(trucksCount);

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
    var orders = data.Orders.OrderBy(x => x.Arrival).ToList();
    var trucks = data.Trucks.OrderBy(x => x.Start).ThenBy(x => x.Id).ToList();

    var truckIndex = 0;
    
    foreach (var order in orders)
    {        
        while (truckIndex < trucks.Count && trucks[truckIndex].Start <= order.Arrival)
        {
            var truck = trucks[truckIndex];

            if (truck.End >= order.Arrival && truck.Capacity > 0)
            {
                order.TruckId = truck.Id;
                truck.Capacity--;
                break;
            }

            truckIndex++;
        }
    }
    
    return orders.OrderBy(x => x.Id).ToList();
}

class TestData
{
    public List<Order> Orders { get; set; }
    public List<Truck> Trucks { get; set; }
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
