let exportForm, failurePopUp,successPopUp,failpopupText;

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("export-button").addEventListener("click", getResultFile);
    exportForm = document.querySelector('.export');
    courseDropdown = document.getElementById('course-dropdown');
    failurePopUp = document.getElementById("failure-popup");
    successPopUp = document.getElementById("success-popup");
    failpopupText = document.getElementById("failure-popup-text");

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

        if (!response.ok)
        {
            throw new Error(await response.text())
            
        }else
        {
            showPopUp(successPopUp)
        }
        const blob = await response.blob();

        const blobUrl = URL.createObjectURL(blob);

        const link = document.createElement('a');

        link.href = blobUrl;

        link.download = `${courseId.replace("","_").replace("%20","_")}_result.zip`;

        document.body.appendChild(link);

        link.click();

        document.body.removeChild(link);

        URL.revokeObjectURL(blobUrl);
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


        failpopupText.innerText = substring;
        showPopUp(failPopup);    }

    function showPopUp(popup) {

        popup.style.display = "block";
        setTimeout(function() {
            popup.style.display = "none";
        }, 4000);
    }
}

