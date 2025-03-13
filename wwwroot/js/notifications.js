// wwwroot/js/notifications.js

function refreshNotifications() {
  fetch("/api/notification")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok: " + response.status);
      }
      return response.json();
    })
    .then((data) => {
      const notificationList = document.getElementById("notification-list");
      notificationList.innerHTML = "";

      if (data && data.length > 0) {
        data.forEach((notification) => {
          const li = document.createElement("li");
          const p = document.createElement("p");
          const small = document.createElement("small");

          p.textContent = notification.message;
          small.textContent = new Date(notification.createdAt).toLocaleString(
            undefined,
            {
              year: "numeric",
              month: "numeric",
              day: "numeric",
              hour: "numeric",
              minute: "2-digit",
            }
          );

          li.appendChild(p);
          li.appendChild(small);

          if (notification.isRead) {
            li.classList.add("read");
          } else {
            li.classList.add("unread");
          }

          notificationList.appendChild(li);
        });
      } else {
        const li = document.createElement("li");
        li.textContent = "No new notifications";
        notificationList.appendChild(li);
      }
    })
    .catch((error) => {
      console.error("Error fetching notifications:", error);
    });
}

function markNotificationsAsRead() {
  fetch("/api/notification/MarkAsRead", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => response.json())
    .then((data) => {
      console.log("Marked as read:", data);
      refreshNotifications();
    })
    .catch((error) => {
      console.error("Error marking notifications as read:", error);
    });
}

function clearNotifications() {
  fetch("/api/notification/Clear", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => response.json())
    .then((data) => {
      console.log("Cleared notifications:", data);
      refreshNotifications();
    })
    .catch((error) => {
      console.error("Error clearing notifications:", error);
    });
}
// Toggle Notifications function
function toggleNotifications() {
  const dropdown = document.getElementById("notification-dropdown");
  dropdown.classList.toggle("hidden");
}
// Optionally, setup a refresh interval for notifications
document.addEventListener("DOMContentLoaded", function () {
  document
    .getElementById("mark-read-btn")
    .addEventListener("click", markNotificationsAsRead);
  document
    .getElementById("clear-btn")
    .addEventListener("click", clearNotifications);
  refreshNotifications();
  // Refresh notifications every 1 seconds (adjust as necessary)
  setInterval(refreshNotifications, 1000);
});
