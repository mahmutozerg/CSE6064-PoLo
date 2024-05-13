let successPopUp,failurePopUp
document.addEventListener("DOMContentLoaded", function() {
    successPopUp =document.getElementById("success-popup");
    failurePopUp =document.getElementById("failure-popup");
    

});

async function sendDeleteRequestAsync(courseId)
{

    var accessToken = getCookie("Session");
    var refreshToken = getCookie("Refresh")

    if (accessToken === '' && refreshToken ==='')
        window.location.href = "logout";

    if (accessToken ==='' && refreshToken !=='')
    {
        await refreshAccessToken();
        accessToken = getCookie("Session")
    }
    
    await fetch('/Admin/DeleteCourse/'+courseId, {
        method: 'DELETE',
        headers: {
            "Authorization": `Bearer ${accessToken}`
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            return response;
        })
        .then(data => {
            document.getElementById("success-popup-text").innerText = courseId+" Successfully Deleted"

            showPopUp(successPopUp);
        })
        .catch(error => {
            document.getElementById("failure-popup-text").innerText = courseId+" Failed to Delete"
            console.error('Error Deleting Course:', error);
            showPopUp(failurePopUp);

        });
}
    function showPopUp(popup) {
        popup.style.display = "block";
        setTimeout(function() {
            popup.style.display = "none";
            setTimeout(function() {
                window.location.reload(); // Refresh the page
            }, 0); // Wait for 1000 milliseconds before refreshing
        }, 4000);
    }

async function showConfirmationPopUp(id) {

    if (confirm("You are going to delete " + id + " are you sure")) {
        await sendDeleteRequestAsync(id)
    }

}

