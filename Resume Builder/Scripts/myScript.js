//var output= document.getElementById('output');
//var ajaxhttp = new XMLHttpRequest(); //adds functionality for ajax request
//var url = "https://my-json-server.typicode.com/typicode/demo/db";
////var url = "https://raw.githubusercontent.com/sidharthaprusty/ResumeBuilder/master/info.json";

////Initializes Ajax request
//ajaxhttp.open("GET", url, true);
//ajaxhttp.setRequestHeader("Content-Type", "application/json");
//ajaxhttp.onreadystatechange = function () {
//    if (ajaxhttp.readyState == 4 && ajaxhttp.status == 200)
//    {
//        var jContent = ajaxhttp.responseText;
//        console.log(jContent);
//        //console.log(""+jContent);         
        
//    }
//}

//ajaxhttp.send(null);
////console.log(ajaxhttp);

function onSubmit(form) {
    var info = [];
    var name = $("input[name=name]").val();
    var email = $("input[name=email]").val();
    var phone = $("input[name=phone]").val();
    var careerObj = $("input[name=careerObj]").val();

    info["name"] = name;
    info["email"] = email;
    info["phone"] = phone;
    info["careerObj"] = careerObj;
    
    var data = {
        "name": name,
        "email":email,
        "phone": phone,
        "careerObj":careerObj
    };
    console.log(info);
    return false; //don't submit
}


var currentTab = 0; // Current tab is set to be the first tab (0)
showTab(currentTab); // Display the current tab

function showTab(n) {
    // This function will display the specified tab of the form ...
    var x = document.getElementsByClassName("tab");
    x[n].style.display = "block";
    // ... and fix the Previous/Next buttons:
    if (n == 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n == (x.length - 1)) {
        document.getElementById("nextBtn").innerHTML = "Submit";
    } else {
        document.getElementById("nextBtn").innerHTML = "Next";
    }
    // ... and run a function that displays the correct step indicator:
    fixStepIndicator(n)
}

function nextPrev(n) {
    // This function will figure out which tab to display
    var x = document.getElementsByClassName("tab");
    // Exit the function if any field in the current tab is invalid:
    if (n == 1 && !validateForm()) return false;
    // Hide the current tab:
    x[currentTab].style.display = "none";
    // Increase or decrease the current tab by 1:
    currentTab = currentTab + n;
    // if you have reached the end of the form... :
    if (currentTab >= x.length) {
        //...the form gets submitted:
        document.getElementById("info").submit();
        return false;
    }
    // Otherwise, display the correct tab:
    showTab(currentTab);
}

function validateForm() {
    // This function deals with validation of the form fields
    var x, y, i, valid = true;
    x = document.getElementsByClassName("tab");
    y = x[currentTab].getElementsByTagName("input");
    // A loop that checks every input field in the current tab:
    for (i = 0; i < y.length; i++) {
        // If a field is empty...
        if (y[i].value == "") {
            // add an "invalid" class to the field:
            y[i].className += " invalid";
            // and set the current valid status to false:
            valid = false;
        }
    }
    // If the valid status is true, mark the step as finished and valid:
    if (valid) {
        document.getElementsByClassName("step")[currentTab].className += " finish";
    }
    return valid; // return the valid status
}

function fixStepIndicator(n) {
    // This function removes the "active" class of all steps...
    var i, x = document.getElementsByClassName("step");
    for (i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(" active", "");
    }
    //... and adds the "active" class to the current step:
    x[n].className += " active";
}

