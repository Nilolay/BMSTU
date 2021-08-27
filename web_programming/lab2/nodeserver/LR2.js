const http = require("http");
const fs = require("fs");
const path = require('path');

var winston = require('winston');

const logger = winston.createLogger({
    transports: [
        new winston.transports.File({
            filename: 'combined.log',
            level: 'info'
        }),
        new winston.transports.File({
            filename: 'errors.log',
            level: 'error'
        })
    ]
});

const options = {
    from: new Date() - (24 * 60 * 60 * 1000),
    until: new Date(),
    limit: 10,
    start: 0,
    order: 'desc',
    fields: ['message']
};

logger.add(new winston.transports.Http(options));
//var request = require('request');

//request.get('http://127.0.0.1:3000/*');

http.createServer(function(request,response){

    
	console.log("Url: " + request.url);
    console.log("Тип запроса: " + request.method);
    console.log("User-Agent: " + request.headers["user-agent"]);
    console.log("Все заголовки");
    console.log(request.headers);
    if (request.method == "GET") {
        logger.log('info', 'get request')
        logger.profile('test');
        if (request.url == '/image') {

            let img = __dirname + '\\photo.jpg'   
            fs.access(img, fs.constants.F_OK, err => {
                console.log(err);
            });
            console.log(img);
            fs.readFile(img, function (err, content) {
                response.writeHead(200, { "Content-Type": "image/jpg" });
                response.end(content);
            }); 

        }
        else {
            response.setHeader("Content-Type", "text/html; charset=utf-8;");
            response.write("<H1>Hello world</H1>");
            response.end();
        }
        
        logger.profile('test');
        
        //response.write("<H1>Hello world</H1>");
        //response.write('<img src="' + path.resolve("static") + 'photo.jpg" alt="увы фото нету" height="1181px" width="1417px"><br><img src="photo.jpg" alt="увы фото нету" height="1181px" width="1417px">'); //photo.jpg
        
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
            //response.end("");
             
}).listen(3000, "127.0.0.1",function(){
    console.log("Сервер начал прослушивание запросов на порту 3000");
});


