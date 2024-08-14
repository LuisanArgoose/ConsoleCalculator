

function onButtonClick(value) {
    const display = document.getElementById('display');
    if (display.value == 'Error') {
        display.value = '';
    }
    display.value += value;
   
}

function onEqualClick() {
    const display = document.getElementById('display');
    const expression = display.value;

    fetch(`/api/calculator/calculate?expression=${encodeURIComponent(expression)}`)
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw new Error(errorData.error || 'Bad request');
                });
            }
            return response.json();
        })
        .then(result => {
            display.value = result;
        })
        .catch(error => {
            display.value = "Error";
        });
}
function onClear() {
    document.getElementById('display').value = '';
}

function onClearEntry() {
    const display = document.getElementById('display');
    display.value = display.value.slice(0, -1); 
}

