# bammemo

```bash
dotnet ef migrations add Init -o ./Migrations --startup-project "../Bammemo.WebApi/Bammemo.WebApi.csproj" -c BammemoDbContext
dotnet ef database update --startup-project "../Bammemo.WebApi/Bammemo.WebApi.csproj" -c BammemoDbContext
```