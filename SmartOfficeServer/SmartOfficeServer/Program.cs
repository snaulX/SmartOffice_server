using SmartOfficeServer;
using System.Text.RegularExpressions;

const string RegexPath = @"^/([0-9]+)/(\w+)/?\w*$";

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

List<CoffeeMachine> coffeeMachines = new List<CoffeeMachine>();

void AddCoffeeMachine(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        coffeeMachines.Add(new CoffeeMachine());
        await context.Response.WriteAsync("Coffee machine added");
    });
}

void Coffee(IApplicationBuilder app)
{
    app.Map("/create", AddCoffeeMachine);

    app.Run(async context =>
    {
        var request = context.Request;
        var response = context.Response;
        response.Headers.ContentLanguage = "ru-RU";
        response.ContentType = "text/plain; charset=utf-8";
        string path = request.Path;
        if (Regex.IsMatch(path, RegexPath))
        {
            string[] splitPath = path.Split('/');
            CoffeeMachine machine = coffeeMachines[int.Parse(splitPath[1])];
            string method = splitPath[2];
            if (method == "status")
            {
                if (request.Method == "GET")
                {
                    await response.WriteAsync(machine.GetStatus());
                }
                else
                {
                    machine.Status = Enum.Parse<CoffeeMachineStatus>(splitPath[3], ignoreCase: true);
                }
            }
            else if (method == "name")
            {
                if (request.Method == "GET")
                {
                    await context.Response.WriteAsync(machine.Name);
                }
                else
                {
                    machine.Name = splitPath[3];
                }
            }
            else if (method == "make")
            {
                await machine.MakeCoffee(response);
            }
            else
            {
                await context.Response.WriteAsync("Function for CoffeeMachine not found");
            }
        }
    });
}

List<Computer> computers = new List<Computer>();

void AddComputer(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        computers.Add(new Computer());
        await context.Response.WriteAsync("Computer was added");
    });
}

void Computer(IApplicationBuilder app)
{
    app.Map("/create", AddComputer);

    app.Run(async context =>
    {
        var request = context.Request;
        var response = context.Response;
        response.Headers.ContentLanguage = "ru-RU";
        response.ContentType = "text/plain; charset=utf-8";
        string path = request.Path;
        if (Regex.IsMatch(path, RegexPath))
        {
            string[] splitPath = path.Split('/');
            Computer computer = computers[int.Parse(splitPath[1])];
            string method = splitPath[2];
            if (method == "on")
            {
                computer.IsWorking = true;
                await response.WriteAsync("Computer turned on");
            }
            else if (method == "off")
            {
                computer.IsWorking= false;
                await response.WriteAsync("Computer turned off");
            }
            else if (method == "name")
            {
                if (request.Method == "GET")
                {
                    await response.WriteAsync(computer.Name);
                }
                else
                {
                    computer.Name = splitPath[3];
                }
            }
            else if (method == "status")
            {
                await response.WriteAsync(computer.IsWorking.ToString());
            }
        }
    });
}

app.Map("/coffee", Coffee);
app.Map("/computer", Computer);

app.Run(async context => await context.Response.WriteAsync("SmartOffice API\nPath not found"));

app.Run();