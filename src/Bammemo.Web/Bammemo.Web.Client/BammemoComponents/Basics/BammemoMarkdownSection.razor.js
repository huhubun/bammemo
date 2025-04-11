export function highlight() {
    const checkInterval = 100;
    const maxAttempts = 600;
    let attempts = 0;

    const intervalId = setInterval(() => {
        if (window.hljs) {
            clearInterval(intervalId);

            const preTags = document.getElementsByTagName('pre');
            for (let i = 0; i < preTags.length; i++) {
                const codeTag = preTags[i].getElementsByTagName('code')[0];
                if (codeTag) {
                    hljs.highlightElement(codeTag);
                }
            }
        } else {
            attempts++;

            if (attempts >= maxAttempts) {
                clearInterval(intervalId);
                console.error('Highlight.js 加载超时，无法执行代码高亮。');
            }
            else if (attempts % 60 == 0) {
                console.info(`正在等待 Highlight.js 的加载，当前 ${attempts}/${maxAttempts}。`);
            }
        }
    }, checkInterval);
}

export function addCopyButton() {
    var snippets = document.querySelectorAll('.snippet');
    var numberOfSnippets = snippets.length;
    for (var i = 0; i < numberOfSnippets; i++) {
        let copyButton = snippets[i].getElementsByClassName("hljs-copy")
        if (copyButton.length === 0) {
            let code = snippets[i].getElementsByTagName('code')[0].innerText;
            snippets[i].innerHTML = snippets[i].innerHTML + '<button class="hljs-copy">Copy</button>';

            copyButton[0].addEventListener("click", function () {
                navigator.clipboard.writeText(code);

                this.innerText = 'Copied!';
                let button = this;
                setTimeout(function () {
                    button.innerText = 'Copy';
                }, 1000)
            });
        }
    }
}