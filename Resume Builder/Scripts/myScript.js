$("button").click(function(){
    const handleFormSubmit = event => {

        // Stop the form from submitting since we’re handling that with AJAX.
        event.preventDefault();

    // TODO: Call our function to get the form data.
    const data = {};

    // Demo only: print the form data onscreen as a formatted JSON object.
    const dataContainer = document.getElementsByClassName('results__display')[0];

    // Use `JSON.stringify()` to make the output valid, human-readable JSON.
    dataContainer.textContent = JSON.stringify(data, null, "  ");

    // ...this is where we’d actually do something with the form data..

    
})
const form = document.getElementsByClassName('contact-form')[0];
form.addEventListener('submit', handleFormSubmit);