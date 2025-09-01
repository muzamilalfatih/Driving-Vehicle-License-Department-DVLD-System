# Driving & Vehicle License Department (DVLD) System

The DVLD Management System is a Windows Forms-based desktop application designed to manage driving license operations, including applications, renewals, and verifications. Built using a clean 3-tier architecture (Presentation Layer, Business Logic Layer, and Data Access Layer), this system ensures a modular, maintainable, and scalable codebase.

## Architecture Overview

The application follows a Three-Tier Architecture, comprising:

1. Presentation Layer – User Interface for interaction  
2. Business Logic Layer – Handles the core functionality and rules  
3. Data Access Layer – Manages database interactions                                

## Technologies Used

- Language: C#  
- Framework: .NET Framework , ADO.net
- Architecture: Three-Tier  
- Database: (SQL Server)

## Getting Started

### Prerequisites

- Visual Studio 2022 or later  
- .NET Framework (4.8)  
- SQL Server installed and running
- Access to SQL Server Management Studio (SSMS) or any SQL query tool

### Installation

1. Clone the repository  
   git clone https://github.com/muzamilalfatih/-driving-license-management-system.git

2. Steps to Set Up the Database

    1. Open the schema.sql and run it.
    2. Open the seed.sql and run it.

3. Open the solution  
   Open the .sln file in Visual Studio.

4. Restore NuGet packages  
   Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution and restore missing packages.
5. Update connection string
6. Build and run  
   Use Build > Build Solution and Debug > Start Debugging to launch the application.
7. Use the following test credentials :

    1. Username : admin.
    2. Password : admin.

## Features

- Licence management: add, update, renew,and search licence 
- User management: register and manage users  
- Transactions: issue ,detain,relase detained licence,  
- Reporting: view and generate activity reports

## Contributing

Contributions are welcome!

1. Fork the repo  
2. Create a branch: git checkout -b feature/YourFeature  
3. Commit your changes: git commit -m "Add feature"  
4. Push the branch: git push origin feature/YourFeature  
5. Open a Pull Request

## Contact

If you have any questions or feedback, feel free to reach out:

- Email: muzamilalfatih123@gmail.com 
- GitHub: https://github.com/muzamilalfatih
