kiota generate -l CSharp `
    -c WebApiClient `
    -n Bammemo.Web.Client.WebApis.Client `
    -d http://localhost:5146/openapi/v1.json `
    --exclude-backward-compatible `
    -o ./src/Bammemo.Web/Bammemo.Web.Client/WebApis/Client