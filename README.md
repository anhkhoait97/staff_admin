#STAFF ADMIN is a C# web API project for managing staff<br />

##Features: <br />

Pattern Des: Unit Of Work, Repository <br />

Scalability: Docker <br />

Multiple Connection: multi dbContext with mongoDB <br />

.NET 8: Developed using the latest .NET 8 framework. <br />

Authentication: Identity <br />

Gateway: Ocelot <br />

Caching: Redis (in-processing) <br />

Searching: ElasticSearch (in-processing) <br />

Log: Serilog (in-processing) <br />

CICD: Github action <br />

##Prerequisites <br />

Before running this project, ensure that you have the following software installed:<br />
Docker - Version 20.10 or higher<br />
Docker Compose - Version 1.29 or higher<br />
.NET SDK 8.0<br />
Visual Studio 2022<br />

##Steppers: <br />
At root folder: <br />
1. run CMD: docker-compose build <br />
2. run CMD: docker-compose up <br />
3. check swagger at GATEWAY: http://localhost:{port}/swagger/index.html <br />