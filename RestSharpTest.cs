using RestSharp;

//[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class TestSharpTest
{

    [SetUp]
    public async Task SetUp()
    {
    }

    [TearDown]
    public async Task TearDown()
    {
    }

    [Test]
    public async Task MyTests()
    {
        var client = new RestClient("https://jsonplaceholder.typicode.com/");
        var request = new RestRequest("todos/1", Method.Get);
        var queryResult = client.Execute<List<String>>(request).Data;

        //Console.WriteLine(queryResult);
        Console.WriteLine("Hello world");

    }
}
