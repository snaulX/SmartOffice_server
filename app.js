const express = require("express");
const app = express();

// enum
const CoffeeStatus = { 
    Ok: 'Исправна', 
    NoWater: 'Нет воды', 
    NoMilk: 'Нет молока', 
    NoCoffee: 'Нет кофейных зёрен',
    MakingCappucino: 'Делает капучино',
    MakingLatte: 'Делает латте' 
};

var machines = [];
var computers = [];

function sleep(ms) {
    return new Promise((resolve) => {
      setTimeout(resolve, ms);
    });
  }

async function makeCoffee(index, coffeeStatus) {
    const cm = machines[index];
    if (cm.status == CoffeeStatus.Ok) {
        cm.status = coffeeStatus;
        await sleep(5000);
        cm.status = CoffeeStatus.Ok;
    }
}

const coffeeRouter = express.Router();
coffeeRouter.use("/create", function(req, res) {
    machines.push({
        name: `Кофемашина #${machines.length}`,
        status: CoffeeStatus.Ok
    });
    res.send("Coffee created");
    console.log("Coffee created");
});
// Handle GET requests like /coffee/0/status
coffeeRouter.get("/:id/:command", function(req, res) {
    const command = req.params.command;
    console.log(`GET request - index: ${req.params.id}; command: ${command}`);
    const cm = machines[req.params.id];
    if (command == "name") {
        res.send(cm.name);
    } else if (command == "status") {
        res.send(cm.status);
    }
});
// Handle POST requests like /coffee/0/make with request body: capuccino
coffeeRouter.post("/:id/:command", express.text(), function(req, res) {
    const command = req.params.command;
    console.log(`POST request - index: ${req.params.id}; command: ${command}; data: ${req.body}`);
    if (command == "make") {
        const coffeeType = req.body;
        console.log(`Make ${coffeeType}`);
        res.send(`Make ${coffeeType}`);
        var status;
        if (coffeeType == 'latte') status = CoffeeStatus.MakingLatte;
        else status = CoffeeStatus.MakingCappucino;
        makeCoffee(req.params.id, status);
    } else if (command == "status") {
        machines[req.params.id].status = req.body;
res.send("Status changed");
    } else if (command == "name") {
        machines[req.params.id].name = req.body;
    }
});
app.use("/coffee", coffeeRouter);

app.listen(5000);