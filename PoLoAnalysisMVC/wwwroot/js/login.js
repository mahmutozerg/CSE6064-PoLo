
function sendLoginRequest() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    login(username, password);
}
async function Login(studentNo,passwd)
{

    try {
        var payload = {
            "UserName": studentNo,
            "Password": passwd
        }
        const response = await fetch(`https://localhost:7248/api/CatsLogin/Login`, body: JSON.stringify(payload));
        const content = await rawResponse.json();

        console.log("içerdeyim")
        if (response.ok) {
            console.log(content);
        } else {
            console.error("Request failed with status " + response.status);
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }

}