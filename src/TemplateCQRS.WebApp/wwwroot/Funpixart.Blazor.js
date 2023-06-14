if (!Element.prototype.matches) {
    Element.prototype.matches =
        Element.prototype.msMatchesSelector ||
        Element.prototype.webkitMatchesSelector;
}

if (!Element.prototype.closest) {
    Element.prototype.closest = function (s) {
        var el = this;

        do {
            if (el.matches(s)) return el;
            el = el.parentElement || el.parentNode;
        } while (el !== null && el.nodeType === 1);
        return null;
    };
}

var resolveCallbacks = [];
var rejectCallbacks = [];

window.Funpixart = {
    triggerFileDownload: async (fileName, contentStreamReference) => {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    },

    initChart: function (elementId, option) {
        var chartDom = document.getElementById(elementId);
        var myChart = echarts.init(chartDom);
        myChart.setOption(option);
    }
};