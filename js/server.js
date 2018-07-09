var https = require("https");
var url = require("url");
var fs = require("fs");
var data = "";
var header = "Type,Brand,Model,Location,storeNumber,serialNumber";
var filePath = "/Program Files/HEAT Software/HEAT/AppServer/ImportInv/";
const options = {
    pfx: fs.readFileSync('cert.pfx'),
    passphrase: 'pass'
};

https.createServer(options, function (request, response) {
    response.writeHead(200, {
        "Content-Type": "text/plain"
    });
    var params = url.parse(request.url, true).query;

    console.log(params.Type);

    data = "\r\n" + params.Type + "," + params.Brand + "," + params.Model + "," + params.Location + "," + params.Store + "," + params.Serial;

    fs.writeFile(filePath + params.Type + "/import.csv", header, function (err) {
        if (err) {
            return console.log(err);
        }
        var dateTime = Date();

        console.log(dateTime + " - Header written to file.");
        writeData();
        
    });
function writeData() {
    fs.appendFile(filePath + params.Type + '/import.csv', data, function (err) {
        if (err) {
            return console.log(err);
        }

        var dateTime = Date();
        console.log(dateTime + " - Data written to file.");
    });
}
    response.write("Ok.");
    response.end();
}).listen(10001);
