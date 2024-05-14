
let uploadForm, uploadInput, uploadBtn, courseDropdown, successPopup, failPopup,failPopUpText;

document.addEventListener("DOMContentLoaded", function() {
     uploadForm = document.querySelector('.upload');
     uploadInput = document.getElementById('excel-file');
     uploadBtn = document.getElementById('upload-btn');
     courseDropdown = document.getElementById('course-dropdown');
     successPopup = document.getElementById('success-popup');
     failPopup = document.getElementById('failure-popup');
     failPopUpText = document.getElementById("failure-popup-text");

})

async function SendRequest() 
{

    const file = uploadInput.files[0];
    const courseId = encodeURIComponent(courseDropdown.value);
    var accessToken = getCookie("Session");
    var refreshToken = getCookie("Refresh")
    
    if (accessToken === '' && refreshToken ==='')
       window.location.href = "logout";

    if (accessToken ==='' && refreshToken !=='')
    {
        await refreshAccessToken();
        accessToken = getCookie("Session");
    }
    
    
    if (file && courseId !== "") {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('courseId', courseId);
        try {
            const response = await fetch('https://localhost:7273/api/ExcelFile/UploadExcel', {
                method: 'POST',
                body: formData,
                headers: {
                    "Authorization": `Bearer ${accessToken}`
                },
            });

            if (!response.ok) {
                throw new Error(await response.text());
            }

            const data = await response.json();
            showPopUp(successPopup, "File Uploaded Successfully");
        } catch (error) {

            const text = error.message;
            const startIndex = text.indexOf('System.Exception:');
            const newlineIndex = text.indexOf('\n', startIndex);
            let substring;

            if (startIndex !== -1 && newlineIndex !== -1) {
                substring = text.substring(startIndex + 'System.Exception:'.length, newlineIndex);
            } else if (startIndex !== -1) {
                substring = text.substring(startIndex + 'System.Exception:'.length);
            } else {
                substring = text;
            }


            failPopUpText.innerText = substring;
            showPopUp(failPopup);
        }


    } else {
        failPopUpText.innerText = "Please Select A Course";
        showPopUp(failPopup);
    }
    function showPopUp(popup) {
    
       popup.style.display = "block";
       setTimeout(function() {
           popup.style.display = "none";
       }, 4000); 
    }
}

