window.bammemo.color = (function () {
    const sheet = new CSSStyleSheet();

    // Get real luminance:
    // Microsoft.FluentUI project FluentDesignTheme.razor.js file
    function getLuminance() {
        const luminance = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
        return realLuminance = luminance === null ? "1.0" : luminance;
    }

    function checkIsDarkMode() {
        const isDark = parseFloat(getLuminance()) < 0.5;

        console.info(`isDark: ${isDark}`);

        return isDark;
    }

    function getCommonStyles() {
        return {
            'control-corner-radius': 16 / 2,
            'layer-corner-radius': 16 / 2
        }
    }

    function getLightStyles() {
        return {
            //'neutral-fill-layer-rest': '#FFFFFF',
        }
    }

    function getDarkStyles() {
        return {
            //'neutral-fill-layer-rest': '#333333',
        }
    }

    return {
        apply: function () {
            const styles = {}; //checkIsDarkMode() ? getDarkStyles() : getLightStyles();

            let styleText = '';
            for (const style of [getCommonStyles(), styles]) {
                for (const [key, value] of Object.entries(style)) {
                    styleText += `--${key}: ${value};`
                }
            }

            sheet.replaceSync(`:root { ${styleText} }`);
            document.adoptedStyleSheets = [...document.adoptedStyleSheets, sheet];
        },
        delayedApply: function () {
            setTimeout(this.apply, 50);
        }
    }
}())