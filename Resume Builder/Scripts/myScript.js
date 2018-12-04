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

function jsonBuilder(){
    var achivements = [];
    var skills = [];
    var name = $("input[name=name]").val();
    var email = $("input[name=email]").val();
    var phone = $("input[name=phone]").val();
    
    var sex = $("#sex :selected").text();
    var father = $("input[name=father]").val();
    var dob = $("input[name=dob]").val();
    var maritalSts = $("#maritalSts :selected").text();
    var nationality = $("#nationality :selected").text();
    var careerObj = $.trim($("#careerObj").val());

    //map array values to var
    var achievements = $("input[name='achievements[]']").map(function () { return $(this).val(); }).get();
    var skills = $("input[name='skills[]']").map(function () { return $(this).val(); }).get();

    var academics10 = [];
    var academics12 = [];
    var academicsGr = [];
    var academicsPG = [];

    var board10 = $("#board10 :selected").text();
    var school = $("input[name=school]").val();
    var year10 = $("input[name=year10]").val();
    var per10 = $("input[name=per10]").val();
    
    academics10 = [board10, school, year10, per10];

    var board12 = $("#board12 :selected").text();
    var jrCollege = $("input[name=jrCollege]").val();
    var studyField = $("#studyField :selected").text();
    var year12 = $("input[name=year12]").val();
    var per12 = $("input[name=per12]").val();

    academics12 = [board12, jrCollege, studyField, year12, per12];

    var university = $("input[name=university]").val();
    var gradCollege = $("input[name=gradCollege]").val();
    var gradDegree = $("input[name=gradDegree]").val();
    var yearGr = $("input[name=yearGr]").val();
    var perGrad = $("input[name=perGrad]").val();

    academicsGr = [university, gradCollege, gradDegree, yearGr, perGrad];

    var pguniversity = $("input[name=pguniversity]").val();
    var pgCollege = $("input[name=pgCollege]").val();
    var pgDegree = $("input[name=pgDegree]").val();
    var yearPG = $("input[name=yearPG]").val();
    var perPG = $("input[name=perPG]").val();

    academicsPG = [pguniversity, pgCollege, pgDegree, yearPG, perPG];


    var data = {
        "name": name,
        "email":email,
        "phone": phone,
        "careerObj": careerObj,
        "sex":sex,
        "father": father,
        "dob": dob,
        "maritalSts": maritalSts,
        "nationality": nationality,
        "careerObj": careerObj,
        "achievements": [achievements],
        "skills": [skills],
        "academics10": academics10,
        "academics12": academics12,
        "academicsGr": academicsGr,
        "academicsPG": academicsPG
    };
    var jsonVal = JSON.stringify(data);
    console.log(data);
    console.log(jsonVal)
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
        jsonBuilder();
        //document.getElementById("info").submit();
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

