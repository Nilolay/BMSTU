const http = require("http");
const fs = require("fs");
const path = require('path');

http.createServer(function(request,response){
     
	console.log("Url: " + request.url);
    console.log("Тип запроса: " + request.method);
    console.log("User-Agent: " + request.headers["user-agent"]);
    console.log("Все заголовки");
    console.log(request.headers);
    if (request.method == "GET") {
        response.setHeader("Content-Type", "text/html; charset=utf-8;");
        response.write("<H1>Hello world</H1>");
    }
    else if (request.method == "POST") {

        var txt = path.resolve("static") + "\log.txt";
        in_data = request.url.toString().split('in_data=')[1].replace('%22', '"').replace('%22', '"').replace('%2C', ',').replace('+', ' ');
        if (in_data) {
            response.end();
        }
        console.log(in_data);
        fs.writeFile(txt, in_data, function (err) {
            if (err) {
                console.log('Could not find or open file for writing\n');
            } else {
                response.writeHead(200, { 'Content-Type': 'text/html' });
                response.end();
            }
        });
        
    }
    else if (request.method == "OPTIONS") {
        response.statusCode = 200;
        response.setHeader("Allow", "GET,POST,OPTIONS,PUT,DELETE");
    }
    else if (request.method == "PUT") {

        //var txt = path.resolve("static") + "\log.txt";
        //in_data = request.url.toString();
        in_data = request.headers['in_data'].toString();
        var txt = path.resolve(in_data);
        if (fs.existsSync(txt)) {
            //file exists
            fs.writeFile(txt, in_data, function (err) {
                if (err) {
                    console.log('Could not find or open file for writing\n');
                } else {
                    response.writeHead(200, { 'Content-Type': 'text/html' });
                    response.end();
                }
            });

        }
        else {
            console.log('Could not find or open file for writing\n');
            response.statusCode = 501;
            response.end();
        }

    }
    else if (request.method == "DELETE") {
        var input_data = request.headers['del'].toString();
        var txt = path.resolve(input_data);
        fs.unlink(txt, function (err) {
        if (err) throw err;
            console.log("file deleted");
        });
        
            }
            response.end("");
             
}).listen(3000, "127.0.0.1",function(){
    console.log("Сервер начал прослушивание запросов на порту 3000");
});


