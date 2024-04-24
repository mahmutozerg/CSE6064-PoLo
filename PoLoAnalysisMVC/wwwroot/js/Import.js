document.addEventListener("DOMContentLoaded", function() {
    const uploadForm = document.querySelector('.upload');
    const uploadInput = document.getElementById('excel-file');
    const uploadBtn = document.getElementById('upload-btn');

    uploadBtn.addEventListener('click', function(event) {
        event.preventDefault(); 

        const file = uploadInput.files[0]; 

        if (file) {
            const formData = new FormData(); 
            formData.append('model', file); 

            fetch('https://localhost:7273/api/ExcelFile/UploadExcel', {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Failed to upload file');
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('File uploaded successfully:', data);
                })
                .catch(error => {
                    console.error('Error uploading file:', error);
                });
        } else {
            console.error('No file selected.');
        }
    });
});