# bammemo

```bash
dotnet ef migrations add Init -o ./Migrations --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
dotnet ef database update --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
```