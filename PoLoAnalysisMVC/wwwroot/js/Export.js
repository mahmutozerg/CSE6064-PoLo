let exportForm;

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("export-button").addEventListener("click", getResultFile);
    exportForm = document.querySelector('.export');
    courseDropdown = document.getElementById('course-dropdown');

});
async function getResultFile() {
    try {
        const courseId = encodeURIComponent(courseDropdown.value);
        const cookie = getCookie("Session");

        const response = await fetch(`https://localhost:7273/api/Result/GetFileByCourseId/${courseId}`, {
            method: 'GET',
            headers: {
                "Authorization": `Bearer ${cookie}`
            },
        });

        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }

        const blob = await response.blob();

        const blobUrl = URL.createObjectURL(blob);

        const link = document.createElement('a');

        link.href = blobUrl;

        link.download = 'result.zip';

        document.body.appendChild(link);

        link.click();

        document.body.removeChild(link);

        URL.revokeObjectURL(blobUrl);
    } catch (error) {
        console.error('An error occurred:', error);
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
