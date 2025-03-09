![bammemo Logo](./assets/logo.png)

bammemo（/ˌbæmˈmɛmoʊ/，竹笺）是一个融合博客、微博与便笺理念的个人内容发布和管理平台，受到 [memos](https://github.com/usememos/memos) 的启发。  
名字取自“竹子”（Bambu）和“memo”（便签），灵感源自中国古代的竹简：单片少字，合集成篇，鼓励持续创作。

## 部署

通过镜像部署：

```bash
docker pull ghcr.io/huhubun/bammemo:0.1.1-alpha.1
```

### 配置项

#### 服务器端配置

`appsettings.Production.json`

```json
{
  "Bammemo": {
    "ConnectionString": "Data Source=/bammemo/bammemo.db",
    "ApiUrl": "https://example.com/api/",
    "Username": "YOUR_USERNAME",
    "Password": "BASE64_ENCODE_PASSWORD"
  }
}
```

使用时建议通过环境变量进行设置 `Bammemo__{名称}`（注意是两个下划线），环境变量会覆盖文件配置：

```bash
export Bammemo__ApiUrl="https://example.com/api/"
```


#### 前端配置

`wwwroot/bammemo.json`

```json
{
  "Bammemo": {
    "ApiUrl": "https://example.com/api/"
  }
}
```

使用时建议直接覆盖文件。

#### 释义

| Name                  | Scope                 |  Description                                             |
| --------------------- | --------------------- | -------------------------------------------------------- |
| ConnectionString      | 服务器                 | sqlite 数据库的路径，db 文件可以不提前创建，首次运行时会自动创建 |
| ApiUrl                | 服务器、前端            | API 的地址，域名 + `/api/`                                 |
| Username              | 服务器                 | 管理员登录使用的用户名                                      |
| Password              | 服务器                 | 管理员登录使用的密码，需要设置为 base64 编码后的值             |

## 开发

基于 ASP.NET Core 的 [Blazor Web App](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-9.0) 进行开发，需要 [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)。

```bash
dotnet run --project ./src/Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj
```

### Db Migration 

```bash
dotnet ef migrations add Init -o ./Migrations --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
dotnet ef database update --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
```

> [!WARNING]
> 目前 bammemo 尚未正式发布，数据库结构可能随时更改，并且不会预先通知。

## 第三方资源

在开发 Bammemo 的过程中，我们使用了大量开源类库、字体、工具等，在此由衷感谢所有开源项目的开发者及维护者，你们的奉献、热爱与智慧是人类进步的基石。

完整依赖列表及许可证，请移步 [third-party](./third-party) 查看。