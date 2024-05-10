let exportForm;

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("export-button").addEventListener("click", getResultFile);
    exportForm = document.querySelector('.export');
    courseDropdown = document.getElementById('course-dropdown');

});
async function getResultFile() {
    try {
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
     
        
        const response = await fetch(`https://localhost:7273/api/Result/GetFileByCourseId/${courseId}`, {
            method: 'GET',
            headers: {
                "Authorization": `Bearer ${accessToken}`
            },
        });

        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }
        console.log(response)
        const blob = await response.blob();

        const blobUrl = URL.createObjectURL(blob);

        const link = document.createElement('a');

        link.href = blobUrl;

        link.download = `${courseId}_result.zip`;

        document.body.appendChild(link);

        link.click();

        document.body.removeChild(link);

        URL.revokeObjectURL(blobUrl);
    } catch (error) {
        console.error('An error occurred:', error);
    }
}

