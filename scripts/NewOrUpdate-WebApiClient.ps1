kiota generate -l CSharp `
    -c WebApiClient `
    -n Bammemo.Web.Client.WebApis.Client `
    -d http://localhost:5146/openapi/v1.json `
    -s Microsoft.Kiota.Serialization.Json.JsonSerializationWriterFactory `
    -s Microsoft.Kiota.Serialization.Text.TextSerializationWriterFactory `
    -s Microsoft.Kiota.Serialization.Multipart.MultipartSerializationWriterFactory `
    --ds Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory `
    --exclude-backward-compatible `
    -o ./src/Bammemo.Web/Bammemo.Web.Client/WebApis/Client