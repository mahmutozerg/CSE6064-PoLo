function openNav() {
    document.getElementById("mySidenav").style.width = "300px";
    document.getElementById("main").style.marginLeft = "300px";
}

/* Set the width of the side navigation to 0 and the left margin of the page content to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
}
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