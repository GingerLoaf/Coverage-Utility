# Coverage-Utility
Coverage utility is a tool that crawls any types handed to it and attempts to call each method using good and bad values to gain coverage and identify fragile code. This utility is capable of automatically giving you some code coverage and is very likely to reveal issues that you never knew existed. Coverage Utility allows you to focus your tests on real business logic, and will assist you in making stronger bullet-proof code in less time.

# Demonstration
    [Test]
    public void MyTest()
    {
        var config = new CoverageUtilityConfig();

        // The asserter is responsible for determining if specific conditions are
        // means to be failures, such as encountering a null argument exception or
        // invalid operation exception. Create your own asserter to customize the
        // behavior of coverage utility
        config.Asserter = new NUnitTestAsserter();

        var assembly = Assembly.Load("MyAssembly");

        // Send an assembly to CoverageUtlity to allow it to crawl every type
        // and verify that each one satisfies your asserter
        CoverageUtility.TestAllTypesInAssembly(assembly, config);

        // Send a type to CoverageUtility to test all non-private methods
        CoverageUtility.TestMethods(typeof(MyType), config);

        // There is a generic version for your convenience
        CoverageUtility.TestMethods<MyType>(config);
    }
    
# Covering Specific Use Cases
To enforce certain rules over time for your application, you can be more specific about specific use cases by using the TestSuggestion attribute. Marking a method with TestSuggestion informs CoverageUtility to run a test using your specific argument for a specific parameter. CoverageUtility will still run the good and bad default test on the method and will run suggestions afterword.

Consider the following method
    public void MyTest(string serverAddress, int port)
    {
        var server = new Server();
        server.Address = serverAddress;
        server.Port = port;
        
        server.Start();
    }
    
There are a number of ways this method can be incorrect. serverUri can be null, serverUri can be empty, serverUri can be an invalid URI and port might be negative. To enforce testing rules on the method, we can add our attributes.
    
    [TestSuggestion(Parameter = "serverUri", Value = "NOT A VALID URI")]
    [TestSuggestion(Parameter = "port", Value = "-99")]
    public void MyTest(string serverAddress, int port)
    {
        var server = new Server();
        server.Address = serverAddress;
        server.Port = port;
        
        server.Start();
    }
    
Now when we this method is handed to Coverage-Utility, we will experience an InvalidOperationException when server.Address or server.Port is negative. In order for our tests to begin passing, we have to make the following changes:

    [TestSuggestion(Parameter = "serverUri", Value = "NOT A VALID URI")]
    [TestSuggestion(Parameter = "port", Value = "-99")]
    public void MyTest(string serverAddress, int port)
    {
        if(string.IsNullOrEmpty(serverAddress))
        {
            Logger.Log("serverAddress is null or empty");
            return;
        }
        
        if(port < 0)
        {
            Logger.Log("port is less than 0");
            return;
        }
    
        var server = new Server();
        server.Address = serverAddress;
        server.Port = port;
        
        server.Start();
    }
    
Coverage-Utility has made our code more robust! It is no longer possible for our code to read an error condition and we inform the caller of the exact reasons why their arguments may have not been valid.

# Supported Types
Coverage-Utility is able to automatically instantiate instances of dependencies. If your method asks for a custom class, Coverage-Utility can create an instance of that object to pass into your method for testing. Here are the types that are supported:

<table>
  <tr>
    <th>Name</th>
    <th>Supported</th>
  </tr>
  <tr>
    <td>Normal Type</td>
    <td>Yes</td>
  </tr>
  <tr>
    <td>C# Primitives</td>
    <td>Yes</td>
  </tr>
  <tr>
    <td>Interface</td>
    <td>Yes</td>
  </tr>
  <tr>
    <td>Generic Type</td>
    <td>Yes</td>
  </tr>
  <tr>
    <td>Abstract Type</td>
    <td>No</td>
  </tr>
  <tr>
    <td>Enum</td>
    <td>No</td>
  </tr>
</table>
