// document.addEventListener("DOMContentLoaded", function() {
//     document.getElementById("export-button").addEventListener("click", getResultFile);
// });
// async function getResultFile() {
//     try {
//         const encodedFileId = encodeURIComponent("2b2338fa-4221-4ebb-af93-b8bfe31ad562");
//
//         const response = await fetch(`https://localhost:7273/api/Result/GetFileByExcelId/${encodedFileId}`);
//
//         if (response.ok) {
//             const blob = await response.blob();
//
//             const url = window.URL.createObjectURL(blob);
//
//             const a = document.createElement("a");
//             a.style.display = "none";
//             a.href = url;
//             a.download = "exported_document.docx";
//             document.body.appendChild(a);
//
//             a.click();
//
//             window.URL.revokeObjectURL(url);
//             document.body.removeChild(a);
//
//         } else {
//             console.error("Request failed with status " + response.status);
//         }
//     } catch (error) {
//         console.error("An error occurred:", error);
//     }
// }

var ColumnChartOptions = {
    series: [{
        name: 'ÖÇ Percantege',
        data: [0.90, 0.93, 0.94, 0.98, 0.97, 0.98]

    }],
    chart: {
        type: 'bar',
        height: 350
    },
    plotOptions: {
        bar: {
            horizontal: false,
            columnWidth: '55%',
            endingShape: 'rounded'
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    xaxis: {
        categories: ['ÖÇ-1', 'ÖÇ-2', 'ÖÇ-3', 'ÖÇ-4', 'ÖÇ-6', 'ÖÇ-7'],
    },
    yaxis: {
        title: {
            text: '% (percentage)'
        }
    },
    fill: {
        opacity: 1
    },
    tooltip: {
        y: {
            formatter: function (val) {
                return "% " + val + " percentage"
            }
        }
    }
};

var barChart = new ApexCharts(document.querySelector("#bar-chart"), ColumnChartOptions);
barChart.render();

var areaChartOptions = {
    series: [{
        name: 'PÇ percantege',
        data: [0.93, 0.93, 0.93, 0.93, 0.93, 0.98]

    }],
    chart: {
        type: 'bar',
        height: 350
    },
    plotOptions: {
        bar: {
            horizontal: false,
            columnWidth: '55%',
            endingShape: 'rounded'
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    xaxis: {
        categories: ['PÇ-3', 'PÇ-4', 'PÇ-5', 'PÇ-6', 'PÇ-7', 'PÇ-8'],
    },
    yaxis: {
        title: {
            text: '% (percentage)'
        }
    },
    fill: {
        opacity: 1
    },
    tooltip: {
        y: {
            formatter: function (val) {
                return "% " + val + " percentage"
            }
        }
    }
};

var areaChart = new ApexCharts(document.querySelector("#area-chart"), areaChartOptions );
areaChart.render();