# bammemo

## 开发

```bash
dotnet ef migrations add Init -o ./Migrations --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
dotnet ef database update --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
```

## 第三方类库

在开发 Bammemo 的过程中，我们使用了大量开源类库、字体、工具等，在此由衷感谢所有开源项目的开发者及维护者，你们的奉献、热爱与智慧是人类进步的基石。

完整依赖列表及许可证，请移步 [third-party](./third-party) 查看。