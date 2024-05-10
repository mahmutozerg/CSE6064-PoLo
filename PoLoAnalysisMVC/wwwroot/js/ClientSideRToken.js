let inactivityTimeout;

async function refreshAccessToken() {
    var refreshToken = getCookie('Refresh');
    if (refreshToken === '')
        window.location.href = "logout";

    var url = 'Login/RefreshToken?refreshToken=' + refreshToken;

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Network response was not ok.');
        }

        const data = await response.json();

        document.cookie = `Session=${data.accessToken}; Secure; SameSite=Strict; expires=${data.accessTokenExpiration}`;
        document.cookie = `Refresh=${data.refreshToken}; Secure; SameSite=Strict; expires=${data.refreshTokenExpiration}`;
    } catch (error) {
        console.error('Error refreshing access token:', error);
    }
}


function checkAccessTokenAndRefresh() {
    resetInactivityTimer(); 

    document.addEventListener('click', resetInactivityTimer);
    document.addEventListener('keypress', resetInactivityTimer);

}

function resetInactivityTimer() {
    clearTimeout(inactivityTimeout);
    inactivityTimeout = setTimeout(promptRefresh,  10*60*1000); 
}

async function promptRefresh() {
    if (confirm("You've been inactive for a while. Are you still there")) {
        await refreshAccessToken();
        resetInactivityTimer();
    } else {
        window.location.href = "logout"
    }
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
