- [nuget packages](./packages/README.md)
- [fonts](./fonts/README.md)

## 生成

使用 [nuget-license](https://github.com/sensslen/nuget-license) 对依赖的 nuget 包的许可证进行整理。

```bash
nuget-license -i Bammemo.sln -fo third-party/packages/README.md -d third-party/packages/
```