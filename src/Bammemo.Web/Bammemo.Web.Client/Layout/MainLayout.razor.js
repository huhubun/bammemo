export function onLoad() {
    // Override design token for SSR:
    // https://github.com/microsoft/fluentui-blazor/issues/1851#issuecomment-2056800498

    bammemo.color.apply();
}
