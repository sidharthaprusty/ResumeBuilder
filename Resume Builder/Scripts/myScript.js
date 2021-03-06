﻿function jsonBuilder(){
    var achivements = [];
    var skills = [];
    var name = $("input[name=Cname]").val();
    var email = $("input[name=Cemail]").val();
    var phone = $("input[name=Cphone]").val();
    
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
    
    academics10 = ["10", "", board10, school, per10, year10];

    var board12 = $("#board12 :selected").text();
    var jrCollege = $("input[name=jrCollege]").val();
    var studyField = $("#studyField :selected").text();
    var year12 = $("input[name=year12]").val();
    var per12 = $("input[name=per12]").val();

    academics12 = ["12", studyField, board12, jrCollege, per12, year12];

    var university = $("input[name=university]").val();
    var gradCollege = $("input[name=gradCollege]").val();
    var gradDegree = $("input[name=gradDegree]").val();
    var yearGr = $("input[name=yearGr]").val();
    var perGrad = $("input[name=perGrad]").val();

    academicsGr = ["Graduation", gradDegree, university, gradCollege, perGrad, yearGr];

    var pguniversity = $("input[name=pguniversity]").val();
    var pgCollege = $("input[name=pgCollege]").val();
    var pgDegree = $("input[name=pgDegree]").val();
    var yearPG = $("input[name=yearPG]").val();
    var perPG = $("input[name=perPG]").val();

    if ($('#postGradCheck').prop("checked") == true)
        academicsPG = ["Post Graduation", pgDegree, pguniversity, pgCollege, perPG, yearPG];
    
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
        "academics": [academics10, academics12, academicsGr, academicsPG],
    };
    var jsonVal = JSON.stringify(data);
    return jsonVal;
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
        
        var formData = jsonBuilder();
        var id = window.location.href.slice(window.location.href.lastIndexOf('/') + 1);//.split('&');
        
        $.ajax({
            type: "POST",
            url: "/Home/BuildResume",
            data: "formData=" + formData + "&id=" + id,
            success: function (response) {
                alert("Hurray!!! Sit back and relax while we build the resume for you.");
                document.getElementById("info").submit();
            },
            failure: function (response) {
                alert("Failure!!! We are taking you back to where it started.");
                location.reload();
            },
            error: function (response) {
                alert("Oops!!! Something went wrong.");
                location.reload();
            }
        });
        
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

//*******Dynamic Input Field for Achievements *********//
$(document).ready(function () {
    var i = 1;
    var j = 1;
    $("#postGraduation").hide();
    $('#add').click(function () {
        if (i <= 3)
        {
            i++;
            $('#dynamic_field').append('<tr id="row' + i + '"><td><input type="text" name="achievements[]" placeholder="Enter your achievements" class="form-control name_list" /></td><td><button type="button" name="remove" id="' + i + '" class="btn btn-danger btn_remove">X</button></td></tr>');
        }
        else
            alert("Whoa!!! List down your top 4 achievements. Keep the rest to surprise your interviewer");
    });
    $(document).on('click', '.btn_remove', function () {
        var button_id = $(this).attr("id");
        $('#row' + button_id + '').remove();
    });

    $('#addSkill').click(function () {
        if (j <= 3) {
            j++;
            $('#dynamic_field_skill').append('<tr id="row' + j + '"><td><input type="text" name="skills[]" placeholder="Enter your skills" class="form-control name_list" /></td><td><button type="button" name="remove" id="' + j + '" class="btn btn-danger btn_remove">X</button></td></tr>');
        }
        else
            alert("Whoa!!! List down your top 4 Skills. Let the ball stay in your court always");
    });

    $(document).on('click', '.btn_remove', function () {
        var button_id = $(this).attr("id");
        $('#row' + button_id + '').remove();
    });

    /****Career Objective validation****/
    var $limitNum = 400;
    var maxChars = $("#careerObj");
    $('textarea[name="careerObj"]').keydown(function () {
        var $this = $(this);
        length = new Number(maxChars.val().length);
        counter = $limitNum - length;
        $("#sessionNum_counter").text(counter);
        if ($this.val().length > $limitNum) {
            $this.val($this.val().substring(0, $limitNum));
        }
    });

    /****Post Graduation option Show/Hide*****/
    $("input[name$='postGradCheck']").click(function () {
        $("#postGraduation").toggle();
    });
});