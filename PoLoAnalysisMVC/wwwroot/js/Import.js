
let uploadForm, uploadInput, uploadBtn, courseDropdown, successPopup, failPopup;

document.addEventListener("DOMContentLoaded", function() {
     uploadForm = document.querySelector('.upload');
     uploadInput = document.getElementById('excel-file');
     uploadBtn = document.getElementById('upload-btn');
     courseDropdown = document.getElementById('course-dropdown');
     successPopup = document.getElementById('success-popup');
     failPopup = document.getElementById('failure-popup');
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
    
        await fetch('https://localhost:7273/api/ExcelFile/UploadExcel', {
            method: 'POST',
            body: formData,
            headers: {
                "Authorization": `Bearer ${accessToken}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText);
                }
                return response.json();
            })
            .then(data => {
                showPopUp(successPopup,"File Uploaded Successfully"); 
            })
            .catch(error => {
                console.error('Error uploading file:', error);
                showPopUp(failPopup);

            });
    } else {
        console.error('No file selected or course not selected.');
        showPopUp(failPopup);
    }
    function showPopUp(popup) {
    
       popup.style.display = "block";
       setTimeout(function() {
           popup.style.display = "none";
       }, 4000); 
    }
}

