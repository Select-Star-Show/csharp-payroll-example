# Connect to CockroachDB using .NET

This repo has the exam ples of using .NET with Web Api, Data EF Core  to use CockroachDB.

## Usage

- Clone the repo
- Make sure you hava CockroachDB up and running:
    ```shell
    cockroach start-single-node --insecure
    ```
- Create the Database `payroll` and add the table `employees`:
    ```shell
    cockroach sql --insecure -e "CREATE DATABASE payroll; CREATE TABLE employees (id uuid NOT NULL DEFAULT (gen_random_uuid()), name text  NOT NULL, role text NOT NULL, CONSTRAINT 'PK_employees' PRIMARY KEY (id));"
    ```
- Go the Project:

    ```shell
    cd my-solution-folder
    dotnet restore
    dotnet build
    dotnet run --project Payroll.Web.Api
    ```