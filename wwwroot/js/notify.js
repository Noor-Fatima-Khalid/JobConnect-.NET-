const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// receive from server
connection.on("ReceiveNotification", function (msg) {
    const list = document.getElementById("notificationsList");

    if (list) {
        const card = document.createElement("div");
        card.className = "card mb-2";
        card.innerHTML = `
            <div class="card-body">
                <p class="card-text">${msg}</p>
            </div>
        `;
        list.prepend(card);
    }

    // turns notification icon red
    const icon = document.getElementById("notificationIcon");
    if (icon) {
        icon.classList.add("text-danger");
    }
});

// Start the connection
connection.start()
    .then(() => console.log("SignalR connected."))
    .catch(function (err) {
        console.error("SignalR connection error: ", err.toString());
    });