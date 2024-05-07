let exportForm;

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("export-button").addEventListener("click", getResultFile);
    exportForm = document.querySelector('.export');
    courseDropdown = document.getElementById('course-dropdown');

});
async function getResultFile() {
    try {
        const courseId = encodeURIComponent(courseDropdown.value);
        const cookie = getCookie("Session")

        var response =fetch(`https://localhost:7273/api/Result/GetFileByCourseId/${courseId}`, {
            method: 'GET',
            headers: {
                "Authorization": `Bearer ${cookie}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText);
                }
                return response.blob();
            })
            .then(data => {
                console.log('File uploaded successfully:', data);
            })
            .catch(error => {
                console.error('Error uploading file:', error);

            });

        if (response.ok) {
            const blob = await response.blob();

            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.style.display = "none";
            a.href = url;
            a.download = "exported_document.docx";
            document.body.appendChild(a);

            a.click();

            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

        } else {
            console.error("Request failed with status " + response.status);
        }
    } catch (error) {
        console.error("An error occurred:", error);
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
