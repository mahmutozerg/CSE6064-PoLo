
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
        const cookie = getCookie("Session")

        if (file && courseId !== "") {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('courseId', courseId);

            fetch('https://localhost:7273/api/ExcelFile/UploadExcel', {
                method: 'POST',
                body: formData,
                headers: {
                    "Authorization": `Bearer ${cookie}`
                },
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('File uploaded successfully:', data);
                    showPopUp(successPopup); 
                })
                .catch(error => {
                    console.error('Error uploading file:', error);
                    
                });
        } else {
            console.error('No file selected or course not selected.');
            showPopUp(failPopup);
        }
       function showPopUp(popup) {
           popup.style.display = "block";
           setTimeout(function() {
               successPopup.style.display = "none";
           }, 4000); // Hide popup after 3 seconds
       }
}

function getCookie(name) {
    const cookies = document.cookie.split(';');

    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}

