function showOverlay() {
    document.getElementById("overlay").style.display = "flex"
}
function hideOverlay() {
    document.getElementById("overlay").style.display = "none"
}
function showEditPage() {
    document.getElementById("overlay").style.display = "flex"
}
function verifyAccount() {
    let pass = document.getElementById("verifyPass");
    const password = "Qwerty123.";
    const msg = document.getElementById("verifyMsg");

    if (pass.value === password) {
        document.getElementById("deactivate").disabled = false;
        document.getElementById("delete").disabled = false;
        msg.hidden = true;
    } else {
        msg.hidden = false;
    }
}
