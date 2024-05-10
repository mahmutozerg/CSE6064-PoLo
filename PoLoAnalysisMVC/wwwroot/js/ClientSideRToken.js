
export function refreshAccessToken() {
    var refreshToken = getCookie('Refresh');

    $.ajax({
        url: 'Login/RefreshToken',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ refreshToken: refreshToken }),
        success: function (data) {
            // Update the access token in cookies with the new token received from the server
            document.cookie = `Session=${data.accessToken}; Secure; SameSite=Strict;`; // Update expiration if needed
        },
        error: function (xhr, status, error) {
            console.error('Error refreshing access token:', error);
            // Handle error (e.g., show error message)
        }
    });
}

export function checkAccessTokenAndRefresh() {
    refreshAccessToken(); // Initial refresh
    setInterval(refreshAccessToken, 5 * 60 * 1000); // Refresh every 5 minutes (5 * 60 * 1000 ms)
}

function getCookie(name) {
    var cookieName = name + '=';
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');

    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i].trim();
        if (cookie.indexOf(cookieName) === 0) {
            return cookie.substring(cookieName.length, cookie.length);
        }
    }

    return '';
}
