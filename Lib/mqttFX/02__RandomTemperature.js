var Thread = Java.type("java.lang.Thread");

function execute(action) {
    out("Test Script: " + action.getName());
    for (var i = 0; i < 10; i++) {
        PublishRandomTemperature();
        Thread.sleep(500);
    }
    action.setExitCode(0);
    action.setResultText("done.");
    out("Test Script: Done");
    return action;
}

function PublishRandomTemperature() {
    out("Temperature");
    var random = (Math.random() * ((30-22) +1) + 22) * 100;
    var temp = Math.round(random) / 100;
    var evnt = {
        Key : "jipa_temperature",
        Site:"MQTTFX",
        Value:temp,
        Timestamp:"1476265939000"
    };

    var msg = JSON.stringify(evnt); 
    out(msg);
    mqttManager.publish("bir57/sensors/temperature", msg);
}


function out(message){
     output.print(message);
}