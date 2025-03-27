chcp 65001
@echo off
for %%b in (
    "./src/Bammemo.CodeAnalysis",
    "./src/Bammemo.Data",
    "./src/Bammemo.Service",
    "./src/Bammemo.Service.Abstractions",
    "./src/Bammemo.Web/Bammemo.Web",
    "./src/Bammemo.Web/Bammemo.Web.Client",
    "./src/tests/Bammemo.Service.Test",
) do (
    for %%s in (
        "/bin",
        "/obj",
    ) do (
        echo 正在删除 %%~b%%~s
        rmdir /s/q "%%~b%%~s"
    )
)
