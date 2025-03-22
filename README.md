![bammemo Logo](./assets/logo.png)

bammemo（/ˌbæmˈmɛmoʊ/，竹笺）是一个融合博客、微博与便笺理念的个人内容发布和管理平台，受到 [memos](https://github.com/usememos/memos) 的启发。  
名字取自“竹子”（Bambu）和“memo”（便签），灵感源自中国古代的竹简：单片少字，合集成篇，鼓励持续创作。

## 部署

通过镜像部署：

```bash
docker pull ghcr.io/huhubun/bammemo:0.1.1-alpha.1
```

### 配置项

#### 服务器端

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

使用时建议通过环境变量进行设置 `Bammemo__{名称}`（注意是两个下划线），环境变量会覆盖配置文件：

```bash
export Bammemo__ApiUrl="https://example.com/api/"
```


# 前端

通过 HTTP 请求从服务器端获取 `GET /bammemo.json`

```json
{
  "Bammemo": {
    "ApiUrl": "https://example.com/api/"
  }
}
```

前端配置中的值由服务器端自动生成，无需进行干预。

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

由于对前端（WebAssembly）启用了 [AOT](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-build-tools-and-aot?view=aspnetcore-9.0) 和[剪裁](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/configure-trimmer?view=aspnetcore-9.0)，请**务必**在本地进行测试，以确保功能能够正常使用（在 PowerShell 中，需将 `\` 替换为 `\``）：

```bash
docker build -t bammemo:local-dev -f src/Bammemo.Web/Bammemo.Web/Dockerfile .
docker run -v C:\bammemo:/data/bammemo \
          -e Bammemo__ConnectionString="Data Source=/data/bammemo/bammemo.db" \
          -e Bammemo__ApiUrl=http://localhost:8080/api/ \
          -e Bammemo__Username=admin \
          -e Bammemo__Password="BASE64_ENCODE_PASSWORD" \
          -p 8080:8080 \
          bammemo:local-dev
```

请注意，如果在执行 `docker build` 前，曾经对代码进行过编译、运行或发布操作，请移除所有项目的 `obj` 和 `bin` 文件夹，否则会导致容器中打包失败。可以运行 `scripts` 目录下的 `clean.bat` 或在 Visual Studio 中安装 [Command Task Runner](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CommandTaskRunner64) 扩展，并通过“视图”—“其他窗口”—“任务运行程序资源管理器”执行 `clean` 任务。

### Db Migration 

```bash
dotnet ef migrations add Init -o ./Migrations --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
dotnet ef database update --startup-project "../Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj" -c BammemoDbContext
```

> [!WARNING]
> 目前 bammemo 尚未正式发布，数据库结构可能随时更改，并且不会预先通知。

### OpenApi Client

我们使用 [Kiota](https://learn.microsoft.com/zh-cn/openapi/kiota/) 来生成访问 Web Api 的客户端，您需要[安装 Kiota 工具](https://learn.microsoft.com/zh-cn/openapi/kiota/install?tabs=bash)：

```bash
dotnet tool install --global Microsoft.OpenApi.Kiota
```

要更新 Web Api 客户端，请执行（在 PowerShell 中，需将 `\` 替换为 `\``）：

```bash
kiota generate -l CSharp \
    -c WebApiClient \
    -n Bammemo.Web.Client.WebApis.Client \
    -d http://localhost:5146/openapi/v1.json \
    --exclude-backward-compatible \
    -o ./src/Bammemo.Web/Bammemo.Web.Client/WebApis/Client
```

## 第三方资源

在开发 Bammemo 的过程中，我们使用了大量开源类库、字体、工具等，在此由衷感谢所有开源项目的开发者及维护者，你们的奉献、热爱与智慧是人类进步的基石。

完整依赖列表及许可证，请移步 [third-party](./third-party) 查看。