# Carat Count

## Overview
Carat Count is a .NET Core application designed to manage various aspects of diamond processing and inventory management. It provides functionalities for tracking diamond packets, employee management, client management, invoicing, and more.

## Features
- Diamond packet management: Create, edit, and track diamond packets, including carat weight, clarity, receive date, delivery date, and associated processes.
- Employee management: Manage employee details such as name, email, and address.
- Client management: Maintain client information including name, email, mobile number, and GST details.
- Invoicing: Generate and manage invoices for diamond transactions.
- Authentication and authorization: Secure login system with role-based access control.

## Technologies Used
- .NET Core
- ASP.NET Core
- Entity Framework Core
- MSSQL Server
- HTML/CSS
- JavaScript 

## Getting Started
### Prerequisites
- .NET Core SDK
- MSSQL Server
- Visual Studio or Visual Studio Code (optional, can use any IDE)

### Installation
1. Clone the repository: `git clone https://github.com/mohitkumarmalani/CarataCount.git`
2. Navigate to the project directory: `cd CarataCount`
3. Restore dependencies: `dotnet restore`
4. Set up MSSQL Server and create a new database.
5. Update connection string in `appsettings.json` with your MSSQL database credentials.
6. Run database migrations: `dotnet ef database update`

### Configuration
- Modify the `appsettings.json` file to configure additional settings such as email server details, JWT secret, etc.

### Usage
1. Build the project: `dotnet build`
2. Run the project: `dotnet run`
3. Access the application in your web browser at `http://localhost:7279`

## Development
### Contributing
Contributions are welcome! Feel free to submit bug reports, feature requests, or pull requests.

## Deployment
1. Build the project in release mode: `dotnet publish -c Release`
2. Deploy the published output to your production server.
3. Set up environment variables for production settings.


## Support
For support or assistance, please contact mmalani9749@conestogac.on.ca or submit a [bug report](https://github.com/mohitkumarmalani/CarataCount/issues).

## License
This project is licensed under the [MIT License](LICENSE).

## Acknowledgements
- Thank you to the .NET Core, Entity Framework Core, and ASP.NET Core communities for providing excellent tools and resources.
- Special thanks to Tulsi Patel and Jenish Patel  for their contributions to the project.

## About the Author
Mohitkumar Malani is a student currently pursuing Computer Engineering at [Conestoga University]. Connect with me on [GitHub](https://github.com/mohitkumarmalani).
