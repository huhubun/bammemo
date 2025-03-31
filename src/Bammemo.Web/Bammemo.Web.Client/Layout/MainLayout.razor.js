export function onLoad() {
    // Override design token for SSR:
    // https://github.com/microsoft/fluentui-blazor/issues/1851#issuecomment-2056800498

    const sheet = new CSSStyleSheet();
    const styles = {};

    let styleText = '';
    for (const style of [getCommonStyles(), styles]) {
        for (const [key, value] of Object.entries(style)) {
            styleText += `--${key}: ${value};`
        }
    }

    sheet.replaceSync(`:root { ${styleText} }`);
    document.adoptedStyleSheets = [...document.adoptedStyleSheets, sheet];
}

function getCommonStyles() {
    return {
        'control-corner-radius': 16 / 2,
        'layer-corner-radius': 16 / 2
    }
}