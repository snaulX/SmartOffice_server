using SmartOfficeServer;
using System.Text.RegularExpressions;

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
        string regexPath = @"^/([0-9]+)/(\w+)/?\w*$";
        var request = context.Request;
        var response = context.Response;
        response.Headers.ContentLanguage = "ru-RU";
        response.ContentType = "text/plain; charset=utf-8";
        string path = request.Path;
        if (Regex.IsMatch(path, regexPath))
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

app.Map("/coffee", Coffee);

app.Run(async context => await context.Response.WriteAsync("SmartOffice API\nPath not found"));

app.Run();