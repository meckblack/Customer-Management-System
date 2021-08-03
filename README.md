# Customer-Management-System
This project uses the following:

- ASP.NET Core backend (Version 5).
- MSSQL Server.
- SMTP mail server (Gmail).
- REST API via Swagger.
- JWT auth library

Create a database called CustomerManagementSystem on your MSSQL server and update the connection string details in appsettings.json

BUILD and RUN the project for the default system admin to be seeded into the database.
Navigate to http://localhost:5001/swagger/v1/swagger.json to view the swagger documenetation

For the email service to work, navigate to Helpers/AppConfig file and replace the EmailSender with a correct email address and EmailPassword with the correct password.


