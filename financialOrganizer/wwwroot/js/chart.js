window.renderExpenseChart = function (chartData) {
    const canvas = document.getElementById(chartData.canvasId);
    if (!canvas) {
        console.error('Canvas element not found');
        return;
    }

    const ctx = canvas.getContext('2d');

    // Удаляем старый график, если есть
    if (canvas.chartInstance) {
        canvas.chartInstance.destroy();
    }

    // Создаем новый график
    canvas.chartInstance = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: chartData.labels,
            datasets: [{
                data: chartData.data,
                backgroundColor: chartData.colors,
                borderColor: 'white',
                borderWidth: 2,
                hoverOffset: 15
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = context.label || '';
                            if (label) {
                                label += ': ';
                            }
                            const value = context.parsed;
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = Math.round((value / total) * 100);
                            return label + new Intl.NumberFormat('ru-RU', {
                                style: 'currency',
                                currency: 'RUB'
                            }).format(value) + ' (' + percentage + '%)';
                        }
                    }
                }
            }
        }
    });
};

// Функция для очистки диаграммы
window.clearExpenseChart = function (canvasId) {
    const canvas = document.getElementById(canvasId);
    if (canvas && canvas.chartInstance) {
        canvas.chartInstance.destroy();
        canvas.chartInstance = null;
    }
};