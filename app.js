const express = require("express");
const app = express();

// enum
const CoffeeStatus = { Ok: '', NoWater: '', NoMilk: '', NoCoffee: '', MakingCappucino: '', MakingLatte: '' };

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

const coffeeRouter = express.Router()
coffeeRouter.use("/create", function(req, res) {
    machines.push({
        name: `Кофемашина #${machines.length}`,
        status: CoffeeStatus.Ok
    });
    res.send("Coffee created");
    console.log("Coffee created");
});
coffeeRouter.get("/:id/:command", function(req, res) {

});
coffeeRouter.post("/:id/:command", function(req, res) {
    if (command == "make") {
        const coffeeType = req.body;
        res.send(`Make ${coffeeType}`);
        var status;
        if (coffeeType == 'latte') status = CoffeeStatus.MakingLatte;
        else status = CoffeeStatus.MakingCappucino;
        makeCoffee(id, status);
    }
});
app.use("/coffee", coffeeRouter);

app.listen(5000);