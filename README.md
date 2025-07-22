# Student Registration System

A simple Windows Forms application developed in **C#** with **SQL Server** backend to manage student registrations for **Skills International School**.

## Features

- ✅ Login Form with authentication
- ✅ Register new students (Create)
- ✅ View student details by Registration Number (Read)
- ✅ Update existing student details (Update)
- ✅ Delete student records (Delete)
- ✅ Clear form fields
- ✅ Logout and Exit options

## Technologies Used

- **C# (.NET Framework)**
- **Windows Forms (WinForms)**
- **SQL Server** (Database)
- **ADO.NET** (Database connectivity)

## Database Information

- **Database Name:** `Student`
- **Table Name:** `Registration`

| Field Name  | Datatype    |
| ----------- | ----------- |
| regNo       | Integer     |
| firstName   | varchar(50) |
| lastName    | varchar(50) |
| dateOfBirth | dateTime    |
| gender      | varchar(50) |
| address     | varchar(50) |
| email       | varchar(50) |
| mobilePhone | Integer     |
| homePhone   | Integer     |
| parentName  | varchar(50) |
| nic         | varchar(50) |
| contactNo   | Integer     |

## Setup Instructions

1. Clone or download the project.
2. Open the solution in **Visual Studio**.
3. Configure the connection string in `RegisterForm.cs` (update SQL Server name if needed):

```csharp
private readonly SqlConnection _connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=Student;Integrated Security=True");
```
