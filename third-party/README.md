- [nuget packages](./nuget_packages/README.md)
- [fonts](./fonts/README.md)

## 生成

使用 [nuget-license](https://github.com/sensslen/nuget-license) 对依赖的 nuget 包的许可证进行整理。

```bash
nuget-license -i Bammemo.sln -fo third-party/nuget_packages/README.md -d third-party/nuget_packages/
```