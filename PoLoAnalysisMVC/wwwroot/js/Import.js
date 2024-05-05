document.addEventListener("DOMContentLoaded", function() {
    const uploadForm = document.querySelector('.upload');
    const uploadInput = document.getElementById('excel-file');
    const uploadBtn = document.getElementById('upload-btn');
    const courseDropdown = document.getElementById('course-dropdown');
    const successPopup = document.getElementById('success-popup');
    const failPopup = document.getElementById('failure-popup');
    uploadBtn.addEventListener('click', function(event) {
        event.preventDefault();

        const file = uploadInput.files[0];
        const courseId = encodeURIComponent(courseDropdown.value);

        if (file && courseId !== "") {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('courseId', courseId);

            fetch('https://localhost:7273/api/ExcelFile/UploadExcel', {
                method: 'POST',
                body: formData
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
    });

    function showPopUp(popup) {
        popup.style.display = "block";
        setTimeout(function() {
            successPopup.style.display = "none";
        }, 4000); // Hide popup after 3 seconds
    }
    
    
});
