

function onButtonClick(value) {
    const display = document.getElementById('display');
    display.value += value;
}

function onEqualClick() {
    const display = document.getElementById('display');
    const expression = display.value;

    fetch('/api/CalculatorController/calculate?expression=' + encodeURIComponent(expression), {
        method: 'GET'
    })
        .then(response => response.json())
        .then(result => {
            display.value = result;
        })
        .catch(error => {
            display.value = 'Error';
            console.error('Error:', error);
        });
}
function onClear() {
    document.getElementById('display').value = '';
}

function onClearEntry() {
    const display = document.getElementById('display');
    display.value = display.value.slice(0, -1); // Remove the last character
}
